using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimationLibrary
{
    public class AnimatedSprite
    {
        private readonly AnimationSpec _spec;
        private float _timeElapsedSinceLastFrameChange; // in milliseconds
        private int _currentFrame;

        public bool IsFinished { get; private set; }
        public int FrameWidth => _spec.Frames[_currentFrame].Width;
        public int FrameHeight => _spec.Frames[_currentFrame].Height;

        public AnimatedSprite(string name, Texture2D spriteSheetTexture, int frameWidth, int frameHeight, int duration, bool isRepeating)
        {
            // try load spec
            // if it's not there build spec
            var spec = new AnimationSpec { SpriteSheet = name, Duration = duration, Repeating = isRepeating };

            int cols = spriteSheetTexture.Width / frameWidth;
            int rows = spriteSheetTexture.Height / frameHeight;
            spec.NumberOfFrames = cols * rows;
            spec.Frames = new Rectangle[spec.NumberOfFrames];

            int x = 0;
            int y = 0;
            for (int i = 0; i < spec.NumberOfFrames; i++)
            {
                spec.Frames[i] = new Rectangle(x, y, frameWidth, frameHeight);
                x += frameWidth;
                if (x >= spriteSheetTexture.Width)
                {
                    x = 0;
                    y += frameHeight;
                }
            }

            _spec = spec;
        }

        public void Update(float deltaTime)
        {
            if (IsTimeToChangeFrame(deltaTime))
            {
                if (_currentFrame < _spec.NumberOfFrames - 1)
                {
                    _currentFrame++;
                }
                else
                {
                    if (_spec.Repeating)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        IsFinished = true;
                    }
                }

                _timeElapsedSinceLastFrameChange = 0.0f;
            }
        }

        public Rectangle GetCurrentFrame()
        {
            return _spec.Frames[_currentFrame];
        }

        private bool IsTimeToChangeFrame(float deltaTime)
        {
            if (_timeElapsedSinceLastFrameChange < _spec.Duration)
            {
                _timeElapsedSinceLastFrameChange += deltaTime;

                return false;
            }

            return true;
        }
    }
}