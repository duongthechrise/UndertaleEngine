using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine.Game1
{
    public class Enemyturn1 : ITurn_battle1
    {
        public bool IsFinished { get; set; }
        private BattleScene battle;
        private float timer;
        KeyboardState _prevkeyboard;
        private int step1 = 0;

        public void Start(BattleScene battle)
        {
            this.battle = battle;
            battle._redheart.Position = new Vector2(320, 320);
            Menu.IsMenu = false;
        }
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (step1)
            {
                case 0:
                    if(timer >= 0f)
                    {
                        battle.Boards["1"].StartResize(new BoardRect(70, 70, 70, 70), 0.5f, BattleBoard.Ease.Linear);
                        step1++;
                        return;
                    }
                    break;
                case 1:
                    if(timer >= 0.75f)
                    {
                       
                        step1++;
                        return;
                    }
                    break;
                case 2:
                    if (timer >= 1.5f)
                    {
                      
                        step1++;
                        return;
                    }
                    break;
                case 3:
                    if (timer >= 2f)
                    {

                        step1++;
                        return;
                    }
                    break;
            }
            if (timer >= 3f)
            {
                IsFinished = true;
                return;
            }
            var k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.B) && !_prevkeyboard.IsKeyDown(Keys.B))
            {
                IsFinished = true;
                System.Diagnostics.Debug.WriteLine("ccc");
            }
            _prevkeyboard = k;
        }
    }
    public class Enemyturn2 : ITurn_battle1
    {
        public bool IsFinished { get; set; }
        private BattleScene battle;
        private float timer;
        KeyboardState _prevkeyboard;

        public void Start(BattleScene battle)
        {
            this.battle = battle;
            battle._redheart.Position = new Vector2(320, 320);
            Menu.IsMenu = false;
        }
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= 4f)
            {
                IsFinished = true;
                return;
            }
            var k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.B) && !_prevkeyboard.IsKeyDown(Keys.B))
            {
                IsFinished = true;
            }
            _prevkeyboard = k;
        }
    }
}
