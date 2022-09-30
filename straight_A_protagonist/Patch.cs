using HarmonyLib;
using GameData;
using TaiwuModdingLib.Core.Plugin;
using GameData.Domains.Character;
using GameData.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Config;
using System;

namespace straight_A_protagonist
{
    [PluginConfig(pluginName: "straight_A_protagonist", creatorId: "senbenmu", pluginVersion: "1,0,0")]
    public class Patch : TaiwuRemakePlugin
    {
        private Harmony _harmony;
        private static PatchConfig _config;

        public override void Dispose()
        {
            try
            {
                _config.SaveConfig();
                _harmony?.UnpatchAll();
                AdaptableLog.Info("harmony patch disposed!");
            }
            catch(Exception ex)
            {
                AdaptableLog.Info("harmony patch dispose failed: " + ex.Message);
            }
        }

        public override void Initialize()
        {
            _harmony = Harmony.CreateAndPatchAll(typeof(Patch));
            AdaptableLog.Info("straight_A_protagonist injection succeed!");
            _config = PatchConfig.LoadConfigFile();
            AdaptableLog.Info("settings load succeed!" + PatchConfig.PathBase);
        }
        /// <summary>
        /// ref List<short> ____featureIds, 引用私有属性
        /// </summary>
        /// <param name="isProtagonist"></param>
        /// <param name="featureGroup2Id"></param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(GameData.Domains.Character.Character), methodName: "GenerateRandomBasicFeatures")]
        public static bool PrefixOfGenerateRandomBasicFeatures( bool isProtagonist, Dictionary<short, short> featureGroup2Id)
        {
            if (!isProtagonist) return true;
            try
            {
                AdaptableLog.Info($"current features's featId are {string.Join(",", featureGroup2Id.Values)}");
                var featureInstance = CharacterFeature.Instance;
                _config.AllAvailableFeatures = featureInstance.Select<CharacterFeatureItem, Feature>(x =>
                {
                    return new Feature
                    {
                        Id = x.TemplateId,
                        GroupId = x.MutexGroupId,
                        Name = x.Name,
                    };
                });
                var customFeatPool = _config.CustomFeatures
                    .Where(x => _config.AllAvailableFeatures.Any(y => x.Id == y.Id))
                    ?? throw new Exception("can't find available feature in custom feature pool!");
                var lockedFeatDic = customFeatPool
                    .Where(x => x.IsLocked && !featureGroup2Id.Any(y => y.Value == x.Id))
                    .ToDictionary(x => x.GroupId, x => x.Id)
                    .Take(_config.FeaturesCount);
                var tempFeatureGroup2Id = featureGroup2Id.Concat(lockedFeatDic).ToDictionary(x => x.Key, x => x.Value);
                featureGroup2Id.Clear();
                foreach (var feature in tempFeatureGroup2Id) featureGroup2Id.TryAdd(feature.Key,feature.Value);
                AdaptableLog.Info($"get all({_config.AllAvailableFeatures.Count()})-custom({customFeatPool.Count()})-locked({lockedFeatDic.Count()}) feats succeed");
                if (!_config.IsOriginPoolGen)
                {
                    PatchConfig.SaveAsJson<IEnumerable<Feature>>(_config.AllAvailableFeatures, "available_features.json");
                    //一个组必定都是基础属性
                    PatchConfig.SaveAsJson<IEnumerable<Feature>>(_config.AllAvailableFeatures
                        .Where(x => featureInstance[x.Id].Basic)
                        , "available_basic_features.json");
                    PatchConfig.SaveAsJson<IEnumerable<Feature>>(_config.AllAvailableFeatures
                        .Where(x => featureInstance[x.Id].CandidateGroupId == 0)
                        , "available_basic_positive_features.json");
                    AdaptableLog.Info("generate origin pool succeed");
                }
                //减去消耗的基础属性数
                var customFeatureCount = _config.FeaturesCount - featureGroup2Id.Count(x => featureInstance[x.Value].Basic);
                var remainsCustomFeatPool = customFeatPool
                    .Where(x => !featureGroup2Id.Any(y => y.Value == x.Id))
                    .ToDictionary(x => x.GroupId, x => x.Id);
                AdaptableLog.Info($"remain custom features/pool count is {customFeatureCount}/{remainsCustomFeatPool.Count}");
                while (customFeatureCount-- > 0)
                {
                    var radomFeature = GetRandomFeatureFromCustomPool(featureGroup2Id, remainsCustomFeatPool);
                    featureGroup2Id.TryAdd(radomFeature.Item1, radomFeature.Item2);
                }
                _config.IsOriginPoolGen = true;
                _config.SaveConfig();
                AdaptableLog.Info("feature add Succeed! detail:" + string.Join(",", featureGroup2Id.Values));
                return false;
            }
            catch(Exception ex)
            {
                AdaptableLog.Warning($"patching feature failed: " + ex.Message);
                return true;
            }
        }

        private static (short,short) GetRandomFeatureFromCustomPool(Dictionary<short,short> currentPool, Dictionary<short,short> customPool)
        {
            var featIds = customPool.Select(x => x.Key).ToArray();
            int randomIndex = 0;
            short featId = 0;
            int tryTimesMax = 100;
            while (tryTimesMax-- > 0)
            {
                randomIndex = new Random().Next(0, customPool.Count - 1);
                featId = featIds[randomIndex];
                //groupid 用于锁定同组元素
                if (currentPool.ContainsValue(_config.IfUnlockSameGroup ? featId : customPool[featId])) continue;
                else break;
            }
#if DEBUG
            AdaptableLog.Info($"random index is {randomIndex}, result is ({featId}, {customPool[featId]})");
#endif
            return (featId, customPool[featId]);
        }
    }

    public class Feature
    {
        public short Id { get; init; }
        public string Name { get; init; }
        public short GroupId { get; init; }
        public bool IsLocked { get; init; } = false;
    }

}