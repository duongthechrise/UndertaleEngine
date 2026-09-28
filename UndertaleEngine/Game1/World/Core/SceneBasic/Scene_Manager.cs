
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UndertaleEngine.Game1
{
    public class SceneManager
    {
        private GameMode _currentmode;
        private IScene _currentscene;
        private Setup _setup;
        public SceneManager(Setup setup)
        {
            _setup = setup;
        }
        public void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            SwitchTo(GameMode.Battle, content, graphics);
        }
        public void SwitchTo(GameMode mode, ContentManager content, GraphicsDevice graphics)
        {
            _currentscene?.Unload();
            _currentmode = mode;
            _currentscene = mode switch
            {
                GameMode.Overworld => new OverworldScene(),
                GameMode.Battle => new BattleScene(),
                _ => null
            };
            if (_currentscene is ISceneWithManager m)
                m.SetManager(this);
            _currentscene?.Load(content, graphics);
        }

        public void Update(GameTime gameTime)
        {
            _currentscene.Update(gameTime);
        }
        public void Draw(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            _currentscene?.Draw(device, spriteBatch);
        }
        public void Shake(float intensity, float duration)
        {
            _setup.Shake(intensity, duration);
        }
        public void PixelShake(int intensity, float duration)
        {
            _setup.Shake(intensity, duration);
        }

    }
}
