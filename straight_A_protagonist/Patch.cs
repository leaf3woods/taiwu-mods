﻿using HarmonyLib;
using GameData;
using TaiwuModdingLib.Core.Plugin;
using GameData.Domains.Character;
using GameData.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Config;
using System;
using GameData.Domains;

namespace straight_A_protagonist
{
    [PluginConfig(pluginName: "straight_A_protagonist", creatorId: "senbenmu", pluginVersion: "1,0,0")]
    public class Patch : TaiwuRemakePlugin
    {
        private Harmony _harmony;
        private static PatchConfig _config;

        public override void OnModSettingUpdate()
        {
            bool useJsonSettings = true;
            DomainManager.Mod.GetSetting(base.ModIdStr, "IfUseJsonSettings", ref useJsonSettings);
            bool unlockSameGroup = true;
            DomainManager.Mod.GetSetting(base.ModIdStr, "IfUnlockSameGroup", ref unlockSameGroup);
            int featuresCount = 7;
            DomainManager.Mod.GetSetting(base.ModIdStr, "FeaturesCount", ref featuresCount);
            if (_config != null && !useJsonSettings)
            {
                _config.FeaturesCount = featuresCount;
                _config.IfUnlockSameGroup = unlockSameGroup;
                AdaptableLog.Info(MessageWrapper($"正在使用前台配置! 特质数:{featuresCount}, 锁定同组:{!unlockSameGroup} "));
                _config.SaveSettings();
            }

        }
        public override void Dispose()
        {
            try
            {
                _config.SaveSettings();
                _harmony?.UnpatchAll();
                AdaptableLog.Info(MessageWrapper("harmony patch disposed!"));
            }
            catch(Exception ex)
            {
                AdaptableLog.Info(MessageWrapper("harmony patch dispose failed: " + ex.Message));
            }
        }

        public override void Initialize()
        {
            _harmony = Harmony.CreateAndPatchAll(typeof(Patch));
            AdaptableLog.Info(MessageWrapper("straight_A_protagonist injection succeed!"));
            _config = PatchConfig.LoadConfigFile();
            AdaptableLog.Info(MessageWrapper("settings load succeed!" + PatchConfig.PathBase));
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
                AdaptableLog.Info(MessageWrapper($"current features's featId are {string.Join(",", featureGroup2Id.Values)}"));
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
                    AdaptableLog.Info(MessageWrapper("generate origin pool succeed"));
                    _config.IsOriginPoolGen = true;
                    _config.SaveSettings();
                    AdaptableLog.Info(MessageWrapper("generate config file pool succeed"));
                }
                var customFeatPool = _config.CustomFeatures
                    .Where(x => _config.AllAvailableFeatures.Any(y => x.Id == y.Id));
                if(customFeatPool.Count() == 0) throw new Exception("can't find available feature in custom feature pool!");
                var lockedFeatDic = customFeatPool
                    .Where(x => x.IsLocked && !featureGroup2Id.Any(y => y.Value == x.Id))
                    .ToDictionary(x => x.Id, x => x.GroupId)
                    .Take(_config.FeaturesCount);
                var tempFeatureGroup2Id = featureGroup2Id.Concat(lockedFeatDic).ToDictionary(x => x.Key, x => x.Value);
                featureGroup2Id.Clear();
                foreach (var feature in tempFeatureGroup2Id)
                {
                    if (featureGroup2Id.ContainsValue(feature.Value) && !_config.IfUnlockSameGroup) continue;
                    featureGroup2Id.TryAdd(feature.Key, _config.IfUnlockSameGroup ? feature.Key : feature.Value);
                }
                AdaptableLog.Info(MessageWrapper($"get all({_config.AllAvailableFeatures.Count()})-custom({customFeatPool.Count()})"
                    + $"-locked({lockedFeatDic.Join(x => x.Key.ToString(), ",")}) feats succeed"));
                //减去消耗的基础属性数
                var customFeatureCount = _config.FeaturesCount - featureGroup2Id.Count(x => featureInstance[x.Value].Basic);
                var remainsCustomFeatPool = customFeatPool
                    .Where(x => !featureGroup2Id.Any(y => y.Value == x.Id))
                    .ToDictionary(x => x.Id, x => x.GroupId);
                AdaptableLog.Info(MessageWrapper($"remain custom features/pool count is {customFeatureCount}/{remainsCustomFeatPool.Count}"));
                while (customFeatureCount-- > 0)
                {
                    var radomFeature = GetRandomFeatureFromCustomPool(featureGroup2Id, remainsCustomFeatPool);
                    featureGroup2Id.TryAdd(radomFeature.Item1, radomFeature.Item2);
                }
                AdaptableLog.Info(MessageWrapper("feature add Succeed! detail:" + string.Join(",", featureGroup2Id.Values)));
                return false;
            }
            catch(Exception ex)
            {
                AdaptableLog.Warning(MessageWrapper($"patching feature failed: " + ex.Message));
                return true;
            }
        }

        private static (short,short) GetRandomFeatureFromCustomPool(Dictionary<short,short> currentPool, Dictionary<short,short> customPool)
        {
            var featIds = customPool.Keys.ToArray();
            int randomIndex = 0;
            short featId = 0;
            int tryTimesMax = 100;
            while (tryTimesMax-- > 0)
            {
                randomIndex = new Random().Next(0, customPool.Count - 1);
                featId = featIds[randomIndex];
                //groupid 用于锁定同组元素
                if (currentPool.ContainsValue(customPool[featId])) continue;
                else break;
            }
#if DEBUG
            AdaptableLog.Info(MessageWrapper($"random index is {randomIndex}, result is ({featId}, {customPool[featId]})"));
#endif
            return (featId, customPool[featId]);
        }

        public static string MessageWrapper(string message) => $"[mod patching]:" + message;
    }

    public class Feature
    {
        public short Id { get; init; }
        public string Name { get; init; }
        public short GroupId { get; init; }
        public bool IsLocked { get; init; } = false;
    }

}