using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine.Game1
{
    public class Enemyturn1 : ITurn_battle1
    {
        public bool IsFinished { get; set; }
        private BattleScene battle;
        private float timer;
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
                    if (timer >= 0f)
                    {
                        battle.Boards["1"].StartResize(new BoardRect(70, 70, 70, 70), 0.5f, BattleBoard.Ease.Linear);
                        step1++;
                        return;
                    }
                    break;
            }
            if (timer >= 2f)
            {
                IsFinished = true;
                return;
            }
        }
    }
    public class Enemyturn2 : ITurn_battle1
    {
        public bool IsFinished { get; set; }
        private BattleScene battle;
        private float timer;
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
                    if (timer >= 0f)
                    {
                        battle.Boards["1"].StartResize(new BoardRect(70, 70, 70, 70), 0.5f, BattleBoard.Ease.Linear);
                        step1++;
                        return;
                    }
                    break;
            }
            if (timer >= 2f)
            {
                IsFinished = true;
                return;
            }
        }
    }
    public class Enemyturn3 : ITurn_battle1
    {
        public bool IsFinished { get; set; }
        private BattleScene battle;
        private float timer;
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
                    if (timer >= 0f)
                    {
                        battle.Boards["1"].StartResize(new BoardRect(70, 70, 70, 70), 0.5f, BattleBoard.Ease.Linear);
                        step1++;
                        return;
                    }
                    break;
            }
            if (timer >= 2f)
            {
                IsFinished = true;
                return;
            }
        }
    }
    public class Enemyturn4 : ITurn_battle1
    {
        public bool IsFinished { get; set; }
        private BattleScene battle;
        private float timer;
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
                    if (timer >= 0f)
                    {
                        battle.Boards["1"].StartResize(new BoardRect(70, 70, 70, 70), 0.5f, BattleBoard.Ease.Linear);
                        step1++;
                        return;
                    }
                    break;
            }
            if (timer >= 2f)
            {
                IsFinished = true;
                return;
            }
        }
    }
}
