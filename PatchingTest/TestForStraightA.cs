using straight_A_protagonist;
using System.Text.Json;
using TaiWuSavEditor;

namespace PatchingTest
{
    [TestClass]
    public class TestForStraightA
    {
        [TestMethod]
        public void TLoadPatchConfig()
        {
            PatchConfig config = PatchConfig.LoadConfigFile();
            string testPath = Path.Combine(PatchConfig.PathBase, "patch_settings.json"); 
            //step 1 no config file loadup
            if(File.Exists(testPath)) File.Delete(testPath);
            config = PatchConfig.LoadConfigFile();
            Assert.IsNotNull(config);
            //step2 format error config file loadup
            if (File.Exists(testPath)) File.Delete(testPath);
            File.WriteAllText(testPath, "format error");
            config = PatchConfig.LoadConfigFile();
            Assert.IsNotNull(config);
            //step3 regular format file loadup 
            if (File.Exists(testPath)) File.Delete(testPath);
            File.Create(testPath).Close();
            File.WriteAllText(testPath, JsonSerializer.Serialize<PatchConfig>(new PatchConfig
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
            }));
            config = PatchConfig.LoadConfigFile();
            Assert.IsNotNull(config);
        }
        [DataRow(true)]
        [TestMethod]
        public void TPatchMainMethod()
        {
            TaiWuSavEditor.Deserializer de = new Deserializer();
            de.JustForTest();
        }
    }
}