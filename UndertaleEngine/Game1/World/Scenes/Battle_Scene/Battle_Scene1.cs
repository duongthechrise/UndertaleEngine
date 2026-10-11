//Vector2 Shake
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace UndertaleEngine.Game1 
{
    public class BattleScene : IScene, ISceneWithManager
    {
        public OBB A, B;
        private SceneManager _manager;
        public Red_heart _redheart;
        private UISystem uisystem;
        private Bullet_test _bullet;
        public HP_Bar _hpbar;
        private DisplayText _timer;
        private DisplayText _timer2;
        DisplayText star;
        private Texture2D _pixel;
        public BattleBoardSystem _boardsystem;
        public Effect maskEffect;
        public bool DebugHitbox = false;
        public BattleBoard _board;
        public Dictionary<string, BattleBoard> Boards = new();
        public Dictionary<string, BattleBoard> CoverBoards = new();
        public BattleSystem battlesystem;
        DialogText dialog;
        KeyboardState _prevKeyboard;
        private SpriteFont _MarsFont;
        SpriteFont determonofont;
        TypeText dialogue;
        public void SetManager(SceneManager manager)
        {
            _manager = manager;
        }

        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            DebugDraw.Init(graphicsDevice);
            maskEffect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true,
                World = Matrix.Identity,
                View = Matrix.Identity,
                Projection = Matrix.CreateOrthographicOffCenter(
                    0, 640,
                    480, 0,
                    0, 1)
            };

            _boardsystem = new BattleBoardSystem();
            Boards["1"] = new BattleBoard();
            Boards["1"].Position = new Vector2(320, 320);
            Boards["1"].SetRectangle(288, 70, 288, 70);
            _boardsystem.AddBoard(Boards["1"]);
            uisystem = new UISystem();
            uisystem.Load(content, graphicsDevice);
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
            _redheart = new Red_heart(new Vector2(300,330));
            _redheart.LoadContent(content, graphicsDevice);
            A = new OBB(new Vector2(_redheart.Position.X, _redheart.Position.Y), new Vector2(10, 10), 0f);
            _MarsFont = content.Load<SpriteFont>("Fonts/Mars_Needs_Cunnilingus");
            determonofont = content.Load<SpriteFont>("Fonts/Determination_Mono");
            _timer = new DisplayText(_MarsFont, "", new Vector2(100, 100), Color.White, 1.5f);
            _timer2 = new DisplayText(_MarsFont, "", new Vector2(100, 120), Color.White, 1.5f);
            _bullet = new Bullet_test();
            _bullet.LoadContent(content, graphicsDevice);
            B = new OBB(new Vector2(_bullet._position.X, _bullet._position.Y), new Vector2(_bullet._texture.Width, _bullet._texture.Height), 0f);
            dialog = new DialogText(determonofont, "  There's someone here.", new Vector2(62, 270), Color.White, 2.0f, 20, true);
            star = new DisplayText(determonofont, "*", new Vector2(60, 270), Color.White, 2, true);
            dialogue = new TypeText(determonofont, new Vector2(94, 270));
            dialogue.Show("There's someone here.\nWell~speed10~...~wait0.5~~speed20~~ccrimson~~shake~actually no one.");
            dialogue.Shake = true;
            battlesystem = new BattleSystem(this);
            battlesystem.EnqueueTotalTurn1();
            battlesystem.EnqueueTotalTurn2();
        }
        public void Update(GameTime gameTime)
        {
            var k = Keyboard.GetState();
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.N) && !_prevKeyboard.IsKeyDown(Keys.N))
            {
                if (!_redheart.IsInvincible)
                {
                    _redheart.TakeDamage();
                    _manager.PixelShake(2, 0.18f);
                }
            }
            if(ks.IsKeyDown(Keys.Space) && !_prevKeyboard.IsKeyDown(Keys.Space))
                dialogue.Show("There's someone here.\nWell~speed10~...~wait0.5~~speed20~~ccrimson~~shake~actually no one.");
            _prevKeyboard = ks;
            float _movingspeed = 150f;
            float _speed;
            _speed = _movingspeed;
            if (k.IsKeyDown(Keys.LeftShift) || k.IsKeyDown(Keys.X) || k.IsKeyDown(Keys.RightShift))
            {
                _speed = _movingspeed * 0.5f;
            }
            else
            {
                _speed = _movingspeed;
            }
            _timer.SetText($"{_redheart.blinkTimer}");
            _timer2.SetText($"{Menu.slot}");
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            uisystem.Update(gameTime);
            _bullet?.Update(gameTime);
            _redheart?.Update(gameTime);
            //Collision
            A.Position = _redheart.Position;
            B.Position = _bullet._position;
            A.Update();
            B.Update();
            //uisystem._buttonfight.Hitbox.Update();
            if (A.Intersects(B))
            {
                Damaging(5f, 3, 0.12f);
            }
            //bool Collided1 = A.Intersects(uisystem._buttonfight.Hitbox);
            
            //if (Collided1 && !A.PreviousCollisions.Contains(uisystem._buttonfight.Hitbox))
            //{
            //    uisystem._buttonfight.CurrentAnimation = uisystem._buttonfight.IsHover;
            //    _switch.Play(1, 0, 0);
            //}

            A.PreviousCollisions = new HashSet<OBB>(A.CurrentCollisions);
            A.CurrentCollisions.Clear();
            //
            dialog.Update(gameTime);
            dialogue.Update(gameTime);
            foreach (var cover in CoverBoards.Values)
            {
                cover.Update(gameTime);
            }
            _boardsystem.Update(gameTime);
            if (Menu.IsMenu == false)
            {
                Vector2 prevPos = _redheart.Position;

                Vector2 move = _redheart.GetMovement(_speed);

                Vector2 desiredPos = prevPos + move * dt;
                Vector2 constrained = _boardsystem.ConstrainPlayerWithCover(desiredPos);


                if (!_boardsystem.IsInsideAnyCover(constrained))
                {
                    _redheart.Position = constrained;
                }
                else
                {
                    Vector2 safePos = _boardsystem.FindNearestSafePoint(constrained);
                    _redheart.Position = safePos;
                }
            }

            battlesystem.Update(gameTime);
        }
        void Damaging(float damage, int shake, float duration)
        {
            if (!_redheart.IsInvincible)
            {
                _redheart.TakeDamage();
                _manager.PixelShake(shake, duration);
            }
        }
        public void Draw(GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            graphics.Clear(ClearOptions.Stencil, Color.Transparent, 0, 0);

            //UI (đặt dưới board)
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp);

            DrawBackGround(spriteBatch, graphics);
            spriteBatch.End();

            //vẽ viền board để đặt trên vật thể, che đạn
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null
                );
            foreach (var board in Boards.Values)
            {
                board.DrawBorder(spriteBatch, _pixel, Color.White, 5f);
            }

            spriteBatch.End();


            graphics.DepthStencilState = StencilWriteMain;    
            foreach (var board in _boardsystem.Boards)    
            {
                board.DrawMaskPolygon(graphics, maskEffect); 
            }


            graphics.DepthStencilState = DepthStencilState.None;
            graphics.BlendState = BlendState.AlphaBlend;

            //vẽ nền đen board 
            graphics.DepthStencilState = StencilReadMain;   
            graphics.BlendState = BlendState.AlphaBlend;

            foreach (var board in _boardsystem.Boards)
            {
                board.DrawMaskPolygon(graphics, maskEffect); 
            }

            graphics.DepthStencilState = DepthStencilState.None;


            graphics.DepthStencilState = StencilWriteCover;

            foreach (var cover in _boardsystem.CoverBoards)
                cover.DrawMaskPolygon(graphics, maskEffect);

            graphics.DepthStencilState = DepthStencilState.None;
            graphics.BlendState = BlendState.AlphaBlend;
            

            graphics.DepthStencilState = StencilReadCover;  
            graphics.BlendState = BlendState.AlphaBlend;

            foreach (var cover in _boardsystem.CoverBoards)
            {
                cover.DrawMaskPolygon(graphics, maskEffect);
            }

            graphics.DepthStencilState = DepthStencilState.None;


            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                StencilReadCover,  
                null
                );

            DrawBackGround(spriteBatch, graphics);

            spriteBatch.End();




            //vẽ các object bên trong board
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                StencilReadMain,    
                null
            );
            DrawInsideBoard(spriteBatch);
            foreach (var cover in CoverBoards.Values)
            {
                cover.DrawBorder(spriteBatch, _pixel, Color.White, 5f);
            }
            spriteBatch.End();

            graphics.Clear(ClearOptions.Stencil, Color.Transparent, 0, 0);

            //Object trên board
            spriteBatch.Begin(SpriteSortMode.Deferred,
                  BlendState.AlphaBlend,
                  SamplerState.PointClamp);

            spriteBatch.End();

            //Thiên Thượng Thiên Hạ, Duy Ngã Độc Tôn
            spriteBatch.Begin(SpriteSortMode.Deferred,
                  BlendState.AlphaBlend,
                  SamplerState.PointClamp);

            _redheart.Draw(spriteBatch);
            uisystem._buttonfight.fight.Draw(spriteBatch);
            uisystem._buttonact.act.Draw(spriteBatch);
            uisystem._buttonfight.fight.Anim.DrawOut(spriteBatch);
            //DebugDraw.DrawOBB(spriteBatch, A, Color.White);
            //DebugDraw.DrawOBB(spriteBatch, B, Color.White);
            spriteBatch.End();
        }


        void DrawInsideBoard(SpriteBatch spriteBatch)
        {
            uisystem._buttonfight.fight.Anim.Draw(spriteBatch);
            uisystem._buttonfight.fight.Anim.DrawIn(spriteBatch);
            if (Menu.IsInMenu)
            {
                dialogue.Draw(spriteBatch);
                //star.Draw(spriteBatch);
            }
        }
        void DrawBackGround(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Draw(_pixel, new Rectangle(0, 0, 640, 480), Color.CornflowerBlue);
            uisystem.Draw(spriteBatch);
        }
        public void Unload()
        {

        }

        DepthStencilState StencilWriteMain = new DepthStencilState()
        {
            StencilEnable = true,
            StencilFunction = CompareFunction.Always,
            StencilPass = StencilOperation.Replace,
            ReferenceStencil = 1,
            DepthBufferEnable = false
        };

        DepthStencilState StencilWriteCover = new DepthStencilState()
        {
            StencilEnable = true,
            StencilFunction = CompareFunction.Always,
            StencilPass = StencilOperation.Replace,
            ReferenceStencil = 2,
            DepthBufferEnable = false
        };

        DepthStencilState StencilReadMain = new DepthStencilState()
        {
            StencilEnable = true,
            StencilFunction = CompareFunction.Equal,
            ReferenceStencil = 1,
            DepthBufferEnable = false
        };

        DepthStencilState StencilReadCover = new DepthStencilState()
        {
            StencilEnable = true,
            StencilFunction = CompareFunction.Equal,
            ReferenceStencil = 2,
            DepthBufferEnable = false
        };
    }
}
