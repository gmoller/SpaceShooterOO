using Microsoft.Xna.Framework;

namespace SpaceShooterLogic
{
    public class AnimatedSprite
    {
        private float _timeElapsedSinceLastFrameChange; // in milliseconds
        private readonly int _durationInMillisecondsOfEachFrame;
        private readonly int _numberOfFrames;
        private int _currentFrame;

        public int FrameWidth { get; }
        public int FrameHeight { get; }
        public Rectangle SourceRectangle { get; private set; }
        public bool Repeats { get; set; } = true;
        public bool IsFinished { get; private set; }

        public AnimatedSprite(int textureWidth, int frameWidth, int frameHeight, int duration)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            _durationInMillisecondsOfEachFrame = duration;
            _numberOfFrames = textureWidth / FrameWidth;
            SourceRectangle = new Rectangle(0, 0, FrameWidth, FrameHeight);
        }

        public void Update(GameTime gameTime)
        {
            if (_timeElapsedSinceLastFrameChange < _durationInMillisecondsOfEachFrame)
            {
                _timeElapsedSinceLastFrameChange += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                if (_currentFrame < _numberOfFrames - 1)
                {
                    _currentFrame++;
                }
                else
                {
                    if (Repeats)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        IsFinished = true;
                    }
                }

                SourceRectangle = new Rectangle(
                    _currentFrame * FrameWidth,
                    0,
                    FrameWidth,
                    FrameHeight);

                _timeElapsedSinceLastFrameChange = 0.0f;
            }
        }
    }
}