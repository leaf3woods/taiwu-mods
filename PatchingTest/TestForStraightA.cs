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
            string testPath = PatchConfig.PathBase+ "patch_settings.json"; 
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
                CustomFeatureIds = new List<short> { 1, 2, 3, 4, 6, 7, 8, 9, 10, 22 },
                IfUseCustomFeaturePool = true
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