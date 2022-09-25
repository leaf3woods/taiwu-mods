using HarmonyLib;
using GameData;
using TaiwuModdingLib.Core.Plugin;
using GameData.Domains.World;
using GameData.Domains.Character;
using GameData.Utilities;
using System.Collections.Generic;
using System.Linq;
using Config;
using System;
using System.Reflection.Metadata.Ecma335;

namespace straight_A_protagonist
{
    [PluginConfig(pluginName: "straight_A_protagonist", creatorId: "senbenmu", pluginVersion: "1,0,0")]
    public class Patch : TaiwuRemakePlugin
    {
        private Harmony _harmony;
        private static PatchConfig _patchConfig = null!;

        public override void Dispose()
        {
            try
            {
                _patchConfig.SaveConfig();
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
            _patchConfig = PatchConfig.LoadConfigFile();
            AdaptableLog.Info("settings will generate in: " + PatchConfig.PathBase);
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
                _patchConfig.AvailableFeatures = featureInstance.Select<CharacterFeatureItem, Feature>(x =>
                {
                    return new Feature
                    {
                        Id = x.TemplateId,
                        GroupId = x.MutexGroupId,
                        Name = x.Name,
                    };
                });
                //减去消耗的基础属性数
                var customFeatureCount = _patchConfig.FeaturesCount - featureGroup2Id.Count(x => featureInstance[x.Value].Basic);
                AdaptableLog.Info($"remain custom features count is {customFeatureCount}");
                while (customFeatureCount > 0)
                {
#warning using custom here!!!
                    var radomFeature = GetRandomFeatureFromCustomPool(featureGroup2Id);
                    featureGroup2Id.Add(radomFeature.Item1, radomFeature.Item2);
                    customFeatureCount--;
                }
                AdaptableLog.Info("feature added Successed! detail:" + String.Join(",",featureGroup2Id.Values));
#if DEBUG
                _patchConfig.SaveConfig();
                AdaptableLog.Info("SaveConfig Successed");
#endif
                return false;
            }
            catch(Exception ex)
            {
                AdaptableLog.Error($"patching feature failed: " + ex.Message);
                return true;
            }
        }

        private static (short,short) GetRandomFeatureFromCustomPool(Dictionary<short,short> customPool)
        {
            var groupIds = customPool.Select(x => x.Key).ToArray();
            var groupId = groupIds[new Random().Next(0, groupIds.Count() - 1)];
            return (groupId, customPool[groupId]);
        }
    }

    public class Feature
    {
        public short Id { get; init; }
        public string Name { get; init; }
        public short GroupId { get; init; }
    }

}