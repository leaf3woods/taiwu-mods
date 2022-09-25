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
        public static string PathBase = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private PatchConfig() { LoadConfigFile(); }
        public int FeaturesCount { get; set; }
        public int[] CustomFeatureIds { get; set; }
        public bool IfUseCustomFeaturePool { get; set; }
        public IEnumerable<Feature> AvailableFeatures { get; set; }

        private static string _filename = "patch_settings.json";

        public static PatchConfig LoadConfigFile(string filename = null)
        {
            try
            {
                var fullPath = Path.Combine(PathBase, filename ?? _filename);
                using FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Read);
                byte[] buffer = new byte[1024];
                var jsonSb = new StringBuilder();
                while (fs.Read(buffer, 0, buffer.Length) > 0)
                    jsonSb.Append(Encoding.UTF8.GetString(buffer));
                var json = jsonSb.ToString();
                if (!string.IsNullOrEmpty(json))
                    return JsonSerializer.Deserialize<PatchConfig>(jsonSb.ToString());
                else
                    return new PatchConfig
                    {
                        FeaturesCount = 7,
                        CustomFeatureIds = new int[10] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 22 },
                        IfUseCustomFeaturePool = true
                    };
            }
            catch(Exception ex)
            {
                AdaptableLog.Error($"load config file failed: " + ex.Message);
                return new PatchConfig
                {
                    FeaturesCount = 7,
                    CustomFeatureIds = new int[10] { 1, 2, 3, 4, 6, 7, 8, 9, 10, 22 },
                    IfUseCustomFeaturePool = true
                };
            }

        }

        public void SaveConfig(string filename = null)
        {
            var fullPath = Path.Combine(PathBase, filename ?? _filename);
            var json = JsonSerializer.Serialize<PatchConfig>(this ?? throw new Exception("null patch config!"));
            using FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Write);
            var buffer = Encoding.UTF8.GetBytes(json);
            fs.Write(buffer, 0, buffer.Length);
        }

    }
}
