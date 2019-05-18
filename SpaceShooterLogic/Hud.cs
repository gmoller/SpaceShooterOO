using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public class Hud
    {
        private readonly SpriteFont _font;

        public Hud()
        {
            _font = AssetsManager.Instance.GetSpriteFont("arialHeading");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, $"Score: {GameEntitiesManager.Instance.Score}", Vector2.Zero, Color.White);
        }
    }
}