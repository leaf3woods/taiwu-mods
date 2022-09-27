using HarmonyLib;
using GameData;
using TaiwuModdingLib.Core.Plugin;
using GameData.Domains.Character;
using GameData.Utilities;
using System.Collections.Generic;
using System.Linq;
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
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(GameData.Domains.Character.Character), methodName: "GenerateRandomBasicFeatures")]
        public static bool PrefixOfGenerateRandomBasicFeatures(bool isProtagonist, Dictionary<short, short> featureGroup2Id)
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
                var customFeatPool = _config.AllAvailableFeatures
                    .Where(x => _config.CustomFeatures.Any(y => x.Id == y.Id))
                    ?? throw new Exception("can't find available feature in custom feature pool!");
                var lockedFeatDic = customFeatPool
                    .Where(x => x.IsLocked && !featureGroup2Id.Any(y => y.Key == x.Id))
                    .ToDictionary(x => x.Id, x => x.GroupId)
                    .Take(_config.FeaturesCount);
                featureGroup2Id = featureGroup2Id.Concat(lockedFeatDic).ToDictionary(x => x.Key, x => x.Value);
                AdaptableLog.Info($"get all({_config.AllAvailableFeatures.Count()})-custom({customFeatPool.Count()}) feats succeed");
                if (!_config.IsOriginPoolGen)
                {
                    _config.SaveAsJson<IEnumerable<Feature>>(_config.AllAvailableFeatures, "available_features.json");
                    //一个组必定都是基础属性
                    _config.SaveAsJson<IEnumerable<Feature>>(_config.AllAvailableFeatures
                        .Where(x => featureInstance[x.Id].Basic)
                        , "available_basic_features.json");
                    _config.SaveAsJson<IEnumerable<Feature>>(_config.AllAvailableFeatures
                        .Where(x => featureInstance[x.Id].CandidateGroupId == 0)
                        , "available_basic_positive_features.json");
                    AdaptableLog.Info("generate origin pool succeed");
                }
                //减去消耗的基础属性数
                var customFeatureCount = _config.FeaturesCount - featureGroup2Id.Count(x => featureInstance[x.Value].Basic);
                var remainsCustomFeatPool = customFeatPool
                    .Where(x => !featureGroup2Id.Any(y => y.Key == x.Id))
                    .ToDictionary(x => x.Id, x => x.GroupId);
                AdaptableLog.Info($"remain custom features/pool count is {customFeatureCount}/{remainsCustomFeatPool.Count}");
                while (customFeatureCount > 0)
                {
                    var radomFeature = GetRandomFeatureFromCustomPool(remainsCustomFeatPool);
                    featureGroup2Id.Add(radomFeature.Item1, radomFeature.Item2);
                    customFeatureCount--;
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

        private static (short,short) GetRandomFeatureFromCustomPool(Dictionary<short,short> customPool)
        {
            var featIds = customPool.Select(x => x.Key).ToArray();
            int randomIndex = 0;
            short featId = 0;
            while (true)
            {
                randomIndex = new Random().Next(0, customPool.Count - 1);
                featId = featIds[randomIndex];
                //groupid 用于锁定同组元素
                if (customPool.ContainsValue(_config.IfUnlockSameGroup ? featId : customPool[featId])) continue;
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