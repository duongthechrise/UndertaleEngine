using Microsoft.Xna.Framework;

namespace UndertaleEngine.Game1
{ 
    public interface ITurn_battle1
    {
        bool IsFinished { get; }
        void Start(BattleScene battle);
        void Update(GameTime gameTime);
    }
}
