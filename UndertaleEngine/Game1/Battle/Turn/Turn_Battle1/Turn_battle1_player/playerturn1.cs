using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine.Game1
{
    public class PlayerTurn1 : ITurn_battle1
    {
        public bool IsFinished { get; private set; }
        private BattleScene battle;
        KeyboardState _prevkeyboard;
        public void Start(BattleScene battle)
        {
            this.battle = battle;
            Menu.IsMenu = true;
            Menu.IsInMenu = true;
        }
        public void Update(GameTime gameTime)
        {
            var k = Keyboard.GetState();
            if(k.IsKeyDown(Keys.C) && !_prevkeyboard.IsKeyDown(Keys.C))
            {
                IsFinished = true;
                System.Diagnostics.Debug.WriteLine("bbb");
            }
            _prevkeyboard = k;
        }
        public class PlayerTurn2 : ITurn_battle1
        {
            public bool IsFinished { get; private set; }
            private BattleScene battle;
            KeyboardState _prevkeyboard;
            public void Start(BattleScene battle)
            {
                this.battle = battle;
                Menu.IsMenu = true;
                battle.Boards["1"].StartResize(new BoardRect(288, 70, 288, 70), 0.5f, BattleBoard.Ease.InOutCubic);
            }
            public void Update(GameTime gameTime)
            {
                var k = Keyboard.GetState();
                if (k.IsKeyDown(Keys.C) && !_prevkeyboard.IsKeyDown(Keys.C))
                {
                    IsFinished = true;
                    System.Diagnostics.Debug.WriteLine("bbb");
                }
                _prevkeyboard = k;
            }
        }
    }
}
