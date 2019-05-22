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

        public AnimatedSprite(AnimationSpec spec)
        {
            _spec = spec;
        }

        //public AnimatedSprite(string name, Texture2D spriteSheetTexture, int frameWidth, int frameHeight, int duration, bool isRepeating)
        //{
        //    // TODO: try load spec, if it's not there build spec
        //    _spec = AnimationSpecCreator.Create(name, spriteSheetTexture, frameWidth, frameHeight, duration, isRepeating);
        //}

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