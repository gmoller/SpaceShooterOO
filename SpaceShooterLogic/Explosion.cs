using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterLogic
{
    public class Explosion : Entity
    {
        public AnimatedSprite Sprite { get; }

        public Explosion(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Sprite = new AnimatedSprite(texture.Width, 32, 32, 100) { Repeats = false };
            Scale = new Vector2(1.5f, 1.5f);
            SourceOrigin = new Vector2(Sprite.FrameWidth * 0.5f, Sprite.FrameHeight * 0.5f);
            DestinationOrigin = new Vector2(Sprite.FrameWidth * 0.5f * Scale.X, Sprite.FrameHeight * 0.5f * Scale.Y);
            Position = position;
            Body.Velocity = Vector2.Zero;
            SetupBoundingBox(Sprite.FrameWidth, Sprite.FrameHeight);
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var destRect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(Sprite.FrameWidth * Scale.X),
                (int)(Sprite.FrameHeight * Scale.Y));
            spriteBatch.Draw(Texture, destRect, Sprite.SourceRectangle, Color.White, 0.0f, SourceOrigin, SpriteEffects.None, 0.0f);
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