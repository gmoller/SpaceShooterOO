using Microsoft.Xna.Framework;

namespace SpaceShooterLogic
{
    public class PhysicsBody
    {
        public Vector2 Velocity { get; set; } = new Vector2(0, 0);
        public Rectangle BoundingBox { get; set; }
    }
}