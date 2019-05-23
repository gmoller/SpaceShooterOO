using System.Collections.Generic;
using AnimationLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterLogic
{
    public class Explosion : Entity
    {
        public AnimatedSprite Sprite { get; }

        public Explosion(Texture2D texture, AnimationSpec animationSpec, Vector2 position, Vector2 size)
        {
            Texture = texture;
            Sprite = new AnimatedSprite(animationSpec);
            Scale = new Vector2(size.X / Sprite.FrameWidth, size.Y / Sprite.FrameHeight);

            SourceOrigin = new Vector2(Sprite.FrameWidth * 0.5f, Sprite.FrameHeight * 0.5f);
            DestinationOrigin = new Vector2(Sprite.FrameWidth * 0.5f * Scale.X, Sprite.FrameHeight * 0.5f * Scale.Y);
            Position = position;
            Body.Velocity = Vector2.Zero;
            SetupBoundingBox(Sprite.FrameWidth, Sprite.FrameHeight);
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var destRect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(Sprite.FrameWidth * Scale.X),
                (int)(Sprite.FrameHeight * Scale.Y));

             spriteBatch.Draw(Texture, destRect, Sprite.GetCurrentFrame(), Color.White, 0.0f, SourceOrigin, SpriteEffects.None, 0.0f);

             //spriteBatch.DrawRectangle(Body.BoundingBox, Color.Yellow, 1.0f);
        }
    }

    public class Explosions
    {
        private readonly List<Explosion> _explosions = new List<Explosion>();

        public void Add(Explosion explosion)
        {
            _explosions.Add(explosion);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _explosions.Count; i++)
            {
                var explosion = _explosions[i];
                explosion.Update(gameTime);
                if (explosion.Sprite.IsFinished)
                {
                    _explosions.Remove(explosion);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Explosion explosion in _explosions)
            {
                explosion.Draw(spriteBatch);
            }
        }
    }
}