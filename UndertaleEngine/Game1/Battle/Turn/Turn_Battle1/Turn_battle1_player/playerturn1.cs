using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine.Game1
{
    public class PlayerTurn1 : ITurn_battle1
    {
        public bool IsFinished { get; private set; }
        private BattleScene battle;
        public void Start(BattleScene battle)
        {
            this.battle = battle;
            Menu.IsMenu = true;
            Menu.IsInMenu = true;
            battle.dialogue.Show("* There's someone here.");
        }
        public void Update(GameTime gameTime)
        {
            if (Menu.turnfinish)
            {
                IsFinished = true;
                Menu.turnfinish = false;
            }
        }
        public class PlayerTurn2 : ITurn_battle1
        {
            public bool IsFinished { get; private set; }
            private BattleScene battle;
            public void Start(BattleScene battle)
            {
                this.battle = battle;
                Menu.IsMenu = true;
                Menu.IsInMenu = true;
                battle.Boards["1"].StartResize(new BoardRect(288, 70, 288, 70), 0.3f, BattleBoard.Ease.Linear);
                battle.dialogue.Show("~wait0.25~* There's someone here.");
            }
            public void Update(GameTime gameTime)
            {
                if (Menu.turnfinish)
                {
                    IsFinished = true;
                    Menu.turnfinish = false;
                }
            }
        }
        public class PlayerTurn3 : ITurn_battle1
        {
            public bool IsFinished { get; private set; }
            private BattleScene battle;
            public void Start(BattleScene battle)
            {
                this.battle = battle;
                Menu.IsMenu = true;
                Menu.IsInMenu = true;
                battle.Boards["1"].StartResize(new BoardRect(288, 70, 288, 70), 0.3f, BattleBoard.Ease.Linear);
                battle.dialogue.Show("~wait0.25~* There's someone here.");
            }
            public void Update(GameTime gameTime)
            {
                if (Menu.turnfinish)
                {
                    IsFinished = true;
                    Menu.turnfinish = false;
                }
            }
        }
        public class PlayerTurn4 : ITurn_battle1
        {
            public bool IsFinished { get; private set; }
            private BattleScene battle;
            public void Start(BattleScene battle)
            {
                this.battle = battle;
                Menu.IsMenu = true;
                Menu.IsInMenu = true;
                battle.Boards["1"].StartResize(new BoardRect(288, 70, 288, 70), 0.3f, BattleBoard.Ease.Linear);
                battle.dialogue.Show("~wait0.25~* There's someone here.");
            }
            public void Update(GameTime gameTime)
            {
                if (Menu.turnfinish)
                {
                    IsFinished = true;
                    Menu.turnfinish = false;
                }
            }
        }
    }
}
