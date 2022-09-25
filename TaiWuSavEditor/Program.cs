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

            //Console.WriteLine($"{dc}");
            Console.WriteLine($"run main loop");
            var info = WorldDomain.GetWorldInfo();
            Console.WriteLine($"{info.TaiwuGivenName}");
        }
    }
}