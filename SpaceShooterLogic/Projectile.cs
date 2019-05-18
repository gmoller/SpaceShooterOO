using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterLogic
{
    public class Projectile : Entity
    {
        public Projectile(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Scale = new Vector2(1.5f, 1.5f);
            SourceOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            DestinationOrigin = new Vector2(texture.Width * 0.5f * Scale.X, texture.Height * 0.5f * Scale.Y);
            Position = position;
            Body.Velocity = velocity;
            SetupBoundingBox(texture.Width, texture.Height);
        }
    }
}