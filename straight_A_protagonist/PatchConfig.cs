using GameData.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace straight_A_protagonist
{
    public class PatchConfig
    {
        public static string PathBase = Path.Combine(Path.GetFullPath(".."), "Mod", "straight_A_protagonist", "Plugins", "Config");
        public PatchConfig() { if (!Directory.Exists(PathBase)) Directory.CreateDirectory(PathBase); }
        public int FeaturesCount { get; set; }
        public bool IfUseCustomFeaturePool { get; set; }
        public bool IsOriginPoolGen { get; set; } = false;
        public bool IfUnlockSameGroup { get; set; } = false;
        public List<Feature> CustomFeatures { get; set; }
        [JsonIgnore]
        public IEnumerable<Feature> AllAvailableFeatures { get; set; }

        private static string _filename = "patch_settings.json";

        public static PatchConfig LoadConfigFile(string filename = null)
        {
            try
            {
                var fullPath = Path.Combine(PathBase, filename ?? _filename);
                if (!File.Exists(fullPath)) throw new Exception("json file is not exits");
                using FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                var jsonb = new StringBuilder();
                int count = 0; 
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    count = fs.Read(buffer, 0, buffer.Length);
                    if (count > 0) jsonb.Append(Encoding.UTF8.GetString(buffer, 0, count));
                    else break;
                }
                var json = jsonb.ToString();
                if (!string.IsNullOrEmpty(json))
                    return JsonSerializer.Deserialize<PatchConfig>(json);
                else
                    throw new Exception("json file formart error!");
            }
            catch(Exception ex)
            {
                AdaptableLog.Warning($"load config file failed: " + ex.Message);
                var @default = new PatchConfig
                {
                    FeaturesCount = 7,
                    CustomFeatures = new List<Feature>(),
                    IfUseCustomFeaturePool = true,
                    IsOriginPoolGen = false,
                    IfUnlockSameGroup = true,
                };
                SaveConfig(@default);
                return @default;
            }
        }

        public void SaveConfig(string filename = null) => SaveAsJson<PatchConfig>(this, filename ?? _filename);

        private static void SaveConfig(PatchConfig config, string filename = null) => SaveAsJson<PatchConfig>(config, filename ?? _filename);

        public static void SaveAsJson<T>(T obj, string filename)
        {
            var fullPath = Path.Combine(PathBase, filename);
            var json = JsonSerializer.Serialize<T>(obj ?? throw new Exception($"json serialize failed, null {obj}"), new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            });
            using FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            var buffer = Encoding.UTF8.GetBytes(json);
            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
        }
    }
}
