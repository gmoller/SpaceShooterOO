using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public class ScrollingBackgroundLayer : Entity
    {
        public int Depth { get; }
        public int PositionIndex { get; }
        public Vector2 InitialPosition { get; }

        public ScrollingBackgroundLayer(Texture2D texture, int depth, int positionIndex, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Depth = depth;
            PositionIndex = positionIndex;
            Position = position;
            InitialPosition = position;
            Body.Velocity = velocity;
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    var destinationRectangle = DeviceManager.Instance.Viewport;
        //    spriteBatch.Draw(Texture, destinationRectangle, Color.White);
        //}
    }
}