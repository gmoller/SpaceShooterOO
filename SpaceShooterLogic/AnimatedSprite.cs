using Microsoft.Xna.Framework;

namespace SpaceShooterLogic
{
    public class AnimatedSprite
    {
        private float _timeElapsedSinceLastFrameChange; // in milliseconds
        private readonly int _durationInMillisecondsOfEachFrame;
        private readonly int _numberOfHorizontalFrames;
        private readonly int _numberOfVerticalFrames;
        private readonly int _numberOfFrames;
        private int _currentFrame;

        public int FrameWidth { get; }
        public int FrameHeight { get; }
        public Rectangle SourceRectangle { get; private set; }
        public bool IsRepeating { get; set; } = true;
        public bool IsFinished { get; private set; }

        public AnimatedSprite(int textureWidth, int textureHeight, int frameWidth, int frameHeight, int duration)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            _durationInMillisecondsOfEachFrame = duration;

            _numberOfHorizontalFrames = textureWidth / FrameWidth;
            _numberOfVerticalFrames = textureHeight / FrameHeight;

            _numberOfFrames = _numberOfHorizontalFrames * _numberOfVerticalFrames;

            SourceRectangle = new Rectangle(0, 0, FrameWidth, FrameHeight);
        }

        public void Update(GameTime gameTime)
        {
            if (IsTimeToChangeFrame(gameTime))
            {
                if (_currentFrame < _numberOfFrames - 1)
                {
                    _currentFrame++;
                }
                else
                {
                    if (IsRepeating)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        IsFinished = true;
                    }
                }

                int x = _currentFrame % _numberOfHorizontalFrames;
                int y = _currentFrame / _numberOfHorizontalFrames;
                SourceRectangle = new Rectangle(
                    x * FrameWidth,
                    y * FrameHeight,
                    FrameWidth,
                    FrameHeight);

                _timeElapsedSinceLastFrameChange = 0.0f;
            }
        }

        private bool IsTimeToChangeFrame(GameTime gameTime)
        {
            if (_timeElapsedSinceLastFrameChange < _durationInMillisecondsOfEachFrame)
            {
                _timeElapsedSinceLastFrameChange += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                return false;
            }

            return true;
        }
    }
}