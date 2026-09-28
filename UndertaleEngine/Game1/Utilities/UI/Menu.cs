using SharpDX.MediaFoundation.DirectX;
using System;

namespace UndertaleEngine
{
    public static class Menu
    {
        public static bool IsMenu = false;
        public static bool IsInMenu = false;

        //             Fight 
        public static int FightOptsColumn = 1;
        public static int FightSlots = 1;
        // ===================================

        //              Act
        public static int ActOptsRow = 1;
        public static int ActOptsColumn = 1;
        public static int ActSlots = 3;
        // ====================================

        public static int slot = 1;
        public static int page = 1;
        public static bool selectingFightopts = false;
        public static bool selectingActopts = false;

        public static String Enemy1Name = "Red";
        public static String Enemy2Name = "Enemy2";
        public static String Enemy3Name = "Enemy3";
        public static int Enemy1HP = 10000;
        public static bool targetingEnemy1 = false;

        public static void Update()
        {

        }
    }
}
