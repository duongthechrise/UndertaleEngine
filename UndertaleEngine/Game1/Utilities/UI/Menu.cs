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
        //               stat
        public static String Enemy1Name = "Red";
        public static String Enemy2Name = "Enemy2";
        public static String Enemy3Name = "Enemy3";
        public static int Enemy1HP = 10000;
        public static double Damage;
        public static int EnemyDef = 20;
        public static int PlayerATK = 30;
        // ======================================

        //       fight Animation config
        public static bool targetingEnemy1 = false;
        public static bool attacking = false;
        public static bool turnfinish = false;
        public static bool performingattack = false;
        public static int direction;
        public static bool slice = false;
        public static bool dodge = false;
        public static bool Miss = false;
        public static bool Hit = false;
        public static bool DealingDamage = false;
        //========================================
        public static void Update()
        {

        }
    }
}
