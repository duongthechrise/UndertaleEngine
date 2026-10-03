using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace UndertaleEngine.Game1
{
    public class SpriteAnimation
    {
        public Texture2D Texture;

        public int StartFrame;       
        public int EndFrame;        
        public float FrameTime;
        public bool Loop;

        public bool IsPlaying = false;
        public bool IsFinished = false;

        private int currentFrame;
        private float timer;

        public int TotalFrames => EndFrame - StartFrame + 1;

        public int FrameWidth => Texture.Width / totalSpriteFrames;
        public int FrameHeight => Texture.Height;

        private int totalSpriteFrames; 

        public float LoopDuration = -1f;
        private float loopTimer = 0f;

        public SpriteAnimation(Texture2D tex, int totalFrames, int startFrame, int endFrame, float frameTime, bool loop = true)
        {
            Texture = tex;
            totalSpriteFrames = totalFrames;

            StartFrame = startFrame;
            EndFrame = endFrame;

            FrameTime = frameTime;
            Loop = loop;

            Reset();
        }

        public void Play()
        {
            IsPlaying = true;
            IsFinished = false;
        }

        public void Pause() => IsPlaying = false;

        public void Reset()
        {
            currentFrame = StartFrame;
            timer = 0;
            IsFinished = false;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPlaying || IsFinished)
                return;
            if (LoopDuration > 0)
            {
                loopTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (loopTimer >= LoopDuration)
                {
                    IsPlaying = false;
                    IsFinished = true;
                    return;
                }
            }

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= FrameTime)
            {
                timer -= FrameTime;
                currentFrame++;

                if (currentFrame > EndFrame)
                {
                    if (Loop)
                        currentFrame = StartFrame;
                    else
                    {
                        currentFrame = EndFrame;
                        IsFinished = true;
                    }
                }
            }
        }

        public Rectangle GetSourceRect()
        {
            return new Rectangle(
                currentFrame * FrameWidth,
                0,
                FrameWidth,
                FrameHeight
            );
        }
    }
}
