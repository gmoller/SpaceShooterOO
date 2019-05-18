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
            spriteBatch.DrawString(_font, $"Lives: {GameEntitiesManager.Instance.Lives}", new Vector2(0.0f, 50.0f), Color.White);
        }
    }
}