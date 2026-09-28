
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace UndertaleEngine.Game1
{
    public class Setup : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private SceneManager _scenemanager;
        KeyboardState _prevKeyboard;


        // Virtual resolution (nội dung gốc 4:3)
        private readonly int _virtualWidth = 640;
        private readonly int _virtualHeight = 480;

        // Cấu hình hiển thị
        private bool _isTogglingFullScreen = false;
        private bool _isFullScreen = false;
        private Point _windowedPosition;
        private Point _windowedSize;
        private DisplayMode _display;

        // Dự phòng kích thước mặc định
        private const int DefaultWindowWidth = 640;
        private const int DefaultWindowHeight = 480;

        private CameraShake _shake = new CameraShake();
        private PixelShake _pixelShake = new PixelShake();


        public Setup()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                HardwareModeSwitch = false
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            GameSettings.LoadSettings();
            Window.Title = "Undertale Engine";

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _renderTarget = new RenderTarget2D(
                GraphicsDevice,
                _virtualWidth,
                _virtualHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8,
                0,
                RenderTargetUsage.PreserveContents
            );
            InitializeDisplay();
            _display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            _scenemanager = new SceneManager(this);
            _scenemanager.Initialize(Content, GraphicsDevice);
            System.Diagnostics.Debug.WriteLine($"[INIT] PrefBackBuffer: {_graphics.PreferredBackBufferWidth}x{_graphics.PreferredBackBufferHeight}, Display: {_display.Width}x{_display.Height}");
        }

        // ---------------- DISPLAY SETUP ----------------
        private void InitializeDisplay()
        {
            _display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            bool full = GameSettings.Current.FullScreen;

            int targetWidth = GameSettings.Current?.WindowWidth > 0
                ? GameSettings.Current.WindowWidth
                : DefaultWindowWidth;
            int targetHeight = GameSettings.Current?.WindowHeight > 0
                ? GameSettings.Current.WindowHeight
                : DefaultWindowHeight;

            // Giới hạn kích thước windowed không vượt quá màn hình
            if (targetWidth >= _display.Width || targetHeight >= _display.Height)
            {
                targetWidth = DefaultWindowWidth;
                targetHeight = DefaultWindowHeight;
            }

            if (full)
            {
                // Fullscreen
                _graphics.PreferredBackBufferWidth = _display.Width;
                _graphics.PreferredBackBufferHeight = _display.Height;
                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();

                GraphicsDevice.Viewport = new Viewport(0, 0, _display.Width, _display.Height);
                Window.Position = Point.Zero;
            }
            else
            {
                // Windowed mode
                _graphics.IsFullScreen = false;
                _graphics.PreferredBackBufferWidth = targetWidth;
                _graphics.PreferredBackBufferHeight = targetHeight;
                _graphics.ApplyChanges();

                GraphicsDevice.Viewport = new Viewport(0, 0, targetWidth, targetHeight);

                int x = (_display.Width - targetWidth) / 2 - 8;
                int y = (_display.Height - targetHeight) / 2 - 20;
                Window.Position = new Point(x, y);

                _windowedSize = new Point(targetWidth, targetHeight);
                _windowedPosition = Window.Position;
            }

            _isFullScreen = full;
        }

        // ---------------- EXITING ----------------
        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            if (!_graphics.IsFullScreen)
            {
                GameSettings.Current.WindowWidth = _graphics.PreferredBackBufferWidth;
                GameSettings.Current.WindowHeight = _graphics.PreferredBackBufferHeight;
                GameSettings.Current.WindowPosX = Window.Position.X;
                GameSettings.Current.WindowPosY = Window.Position.Y;
            }

            GameSettings.Current.FullScreen = _graphics.IsFullScreen;
            GameSettings.SaveSettings();

            base.OnExiting(sender, args);
        }

        // ---------------- UPDATE ----------------
        protected override void Update(GameTime gameTime)
        {
            _shake.Update(gameTime);
            _pixelShake.Update(gameTime);
            var k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.F4) && !_isTogglingFullScreen)
            {
                ToggleFullScreen();
            }
            else if (k.IsKeyUp(Keys.F4))
            {
                _isTogglingFullScreen = false;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                k.IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState ks = Keyboard.GetState();
            _prevKeyboard = ks;
            _scenemanager.Update(gameTime);
            base.Update(gameTime);
        }

        // ---------------- FULLSCREEN TOGGLE ----------------
        // CẤM THẰNG CHÓ NÀO ĐƯỢC ĐỤNG VÀO CỦA BỐ
        private void ToggleFullScreen()
        {
            if (_isTogglingFullScreen) return;
            _isTogglingFullScreen = true;

            bool goingToFull = !_isFullScreen;
            _isFullScreen = goingToFull;
            var display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            if (goingToFull)
            {
                // Lưu lại trạng thái windowed
                _windowedPosition = Window.Position;
                _windowedSize = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                // Fullscreen
                _graphics.PreferredBackBufferWidth = display.Width;
                _graphics.PreferredBackBufferHeight = display.Height;
                _graphics.IsFullScreen = true;
                _graphics.ApplyChanges();

                GraphicsDevice.Viewport = new Viewport(0, 0, display.Width, display.Height);
                Window.Position = Point.Zero;
            }
            else
            {
                // Nếu chưa có giá trị windowedSize hợp lệ, dùng mặc định 640x480
                if (_windowedSize.X <= 0 || _windowedSize.Y <= 0)
                {
                    _windowedSize = new Point(640, 480);
                }

                // Quay lại chế độ cửa sổ
                _graphics.IsFullScreen = false;
                _graphics.PreferredBackBufferWidth = _windowedSize.X;
                _graphics.PreferredBackBufferHeight = _windowedSize.Y;
                System.Diagnostics.Debug.WriteLine($"[Fullscreen Toggle] {goingToFull} -> {_graphics.PreferredBackBufferWidth}x{_graphics.PreferredBackBufferHeight}");

                _graphics.ApplyChanges();

                // Căn giữa cửa sổ
                int x = (_display.Width - _windowedSize.X) / 2 - 8;
                int y = (_display.Height - _windowedSize.Y) / 2 - 20;
                Window.Position = new Point(x, y);

                _windowedPosition = Window.Position;

                System.Diagnostics.Debug.WriteLine($"[Windowed Restore] {_windowedSize.X}x{_windowedSize.Y} at {x},{y}");
            }


            GameSettings.Current.FullScreen = _isFullScreen;
            if (!_isFullScreen)
            {
                GameSettings.Current.WindowWidth = _graphics.PreferredBackBufferWidth;
                GameSettings.Current.WindowHeight = _graphics.PreferredBackBufferHeight;
            }

            GameSettings.SaveSettings();

            System.Diagnostics.Debug.WriteLine($"[Fullscreen Toggle] {_isFullScreen} -> {_graphics.PreferredBackBufferWidth}x{_graphics.PreferredBackBufferHeight}");
        }

        // ---------------- DRAW ----------------
        protected override void Draw(GameTime gameTime)
        {
            // 1) Vẽ scene vào renderTarget 640x480 (như cũ)
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);

            _scenemanager.Draw(GraphicsDevice, _spriteBatch);

            GraphicsDevice.SetRenderTarget(null);

            // 2) Tính scale 4:3 và offset letterbox
            int backW = _graphics.PreferredBackBufferWidth;
            int backH = _graphics.PreferredBackBufferHeight;

            float targetAspect = (float)_virtualWidth / _virtualHeight;      // 4/3
            float screenAspect = (float)backW / backH;

            float scale;
            float offsetX = 0f;
            float offsetY = 0f;

            if (screenAspect > targetAspect)
            {
                // Màn hình rộng hơn 4:3 -> fit theo chiều cao
                scale = (float)backH / _virtualHeight;
                float drawWidth = _virtualWidth * scale;
                offsetX = (backW - drawWidth) * 0.5f;
            }
            else
            {
                // Màn hình hẹp hơn hoặc bằng 4:3 -> fit theo chiều ngang
                scale = (float)backW / _virtualWidth;
                float drawHeight = _virtualHeight * scale;
                offsetY = (backH - drawHeight) * 0.5f;
            }

            // 3) Lấy offset shake (đơn vị pixel 640x480)
            Vector2 shakeOffset = _shake.Offset + _pixelShake.Offset;

            // 4) Tạo transform: shake trong không gian 640x480 -> scale -> dịch vào giữa màn hình
            Matrix transform =
                Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0f) *
                Matrix.CreateScale(scale, scale, 1f) *
                Matrix.CreateTranslation(offsetX, offsetY, 0f);

            // 5) Vẽ ra backbuffer với transform (KHÔNG viewport hack, KHÔNG shakeTarget)
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                transform
            );

            // vẽ texture 640x480 ở (0,0) trong không gian ảo
            _spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Shake(float intensity, float duration)
        {
            _shake.Shake(intensity, duration);
        }
        public void PixelShake(int intensity, float duration)
        {
            _pixelShake.Shake(intensity, duration);
        }
    }
}