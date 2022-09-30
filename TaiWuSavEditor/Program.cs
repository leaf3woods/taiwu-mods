using GameData;
using GameData.Common;
using GameData.Domains.World;
using GameData.GameDataBridge;
using TaiwuModdingLib.Core.Plugin;
namespace TaiWuSavEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var dc = DataContextManager.GetCurrentThreadDataContext();
            var test = new List<string>()
            {
                "1",
                "2",
                "3",
            };
            //Clear(test);
            Console.WriteLine(string.Join(",",test.Take(6)));

            //Console.WriteLine($"{dc}");
            Console.WriteLine($"run main loop");
            var info = WorldDomain.GetWorldInfo();
            Console.WriteLine($"{info.TaiwuGivenName}");
        }

        static void Clear(List<string> test)
        {
            var testList = new List<string>();
            //无法赋值
            //test = testList;
            //应该这样
            test.Clear();
            test.AddRange(testList);
        }
    }
}