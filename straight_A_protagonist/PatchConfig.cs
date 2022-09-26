using GameData.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace straight_A_protagonist
{
    public class PatchConfig
    {
        public static string PathBase = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public PatchConfig() { }
        public int FeaturesCount { get; set; }
        public List<Feature> CustomFeatures { get; set; }
        public bool IfUseCustomFeaturePool { get; set; }
        public IEnumerable<Feature> AllAvailableFeatures { get; set; }

        private static string _filename = "patch_settings.json";

        public static PatchConfig LoadConfigFile(string filename = null)
        {
            try
            {
                var fullPath = Path.Combine(PathBase, filename ?? _filename);
                if (File.Exists(fullPath)) throw new Exception("json file is not exits");
                using FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[1024];
                var json = string.Empty;
                while (fs.Read(buffer, 0, buffer.Length) > 0)
                    json += Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                if (!string.IsNullOrEmpty(json))
                    return JsonSerializer.Deserialize<PatchConfig>(json);
                else
                    throw new Exception("json file formart error!");
            }
            catch(Exception ex)
            {
                AdaptableLog.Error($"load config file failed: " + ex.Message);
                var @default = new PatchConfig
                {
                    FeaturesCount = 7,
                    CustomFeatures = new List<Feature>
                    {
                        new Feature
                        {
                            Id = 1,
                            GroupId = 2,
                            IsLocked = false,
                            Name = "exmaple"
                        }
                    },
                    IfUseCustomFeaturePool = true,
                };
                SaveConfig(@default);
                return @default;
            }
        }

        public void SaveConfig(string filename = null)
        {
            var fullPath = Path.Combine(PathBase, filename ?? _filename);
            var json = JsonSerializer.Serialize<PatchConfig>(this ?? throw new Exception("null patch config!"));
            using FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write);
            var buffer = Encoding.UTF8.GetBytes(json);
            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
        }
        private static void SaveConfig(PatchConfig config, string filename = null)
        {
            var fullPath = Path.Combine(PathBase, filename ?? _filename);
            var json = JsonSerializer.Serialize<PatchConfig>(config ?? throw new Exception("null patch config!"));
            using FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            var buffer = Encoding.UTF8.GetBytes(json);
            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
        }
    }
}
