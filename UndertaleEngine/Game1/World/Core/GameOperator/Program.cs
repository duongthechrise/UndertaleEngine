using System.Runtime.InteropServices;
using System;
namespace UndertaleEngine.Game1
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {
            SetProcessDPIAware();

            using var game = new Setup();
            game.Run();
        }
    }
}

