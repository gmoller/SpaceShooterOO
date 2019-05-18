using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterLogic
{
    public abstract class Entity
    {
        protected Texture2D Texture { get; set; }
        protected bool IsRotatable { get; set; }
        public Vector2 Scale { get; set; } = new Vector2(1.5f, 1.5f);
        public Vector2 Position { get; set; } // center position of entity
        protected Vector2 SourceOrigin { get; set; } = Vector2.Zero;
        public Vector2 DestinationOrigin { get; set; } = Vector2.Zero;
        public PhysicsBody Body { get; }

        protected Entity()
        {
            Body = new PhysicsBody();
        }

        public void SetupBoundingBox(int width, int height)
        {
            Body.BoundingBox = new Rectangle(
                (int) (Position.X - (int)DestinationOrigin.X),
                (int) (Position.Y - (int)DestinationOrigin.Y),
                (int) (width * Scale.X),
                (int) (height * Scale.Y));
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Body != null)
            {
                Position = new Vector2(Position.X + Body.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y + Body.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
                Body.BoundingBox = new Rectangle(
                    (int) Position.X - (int)DestinationOrigin.X,
                    (int) Position.Y - (int)DestinationOrigin.Y,
                    Body.BoundingBox.Width,
                    Body.BoundingBox.Height);
            }
            else
            {
                Console.WriteLine("[BaseEntity] body not found, skipping position updates.");
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}