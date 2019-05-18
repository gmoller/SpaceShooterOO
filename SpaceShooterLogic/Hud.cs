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
            _font = AssetsManager.Instance.GetSpriteFont("arialSmall");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, $"Score: {GameEntitiesManager.Instance.Score}", Vector2.Zero, Color.Red);
            spriteBatch.DrawString(_font, $"Score: {GameEntitiesManager.Instance.Score}", Vector2.One, Color.White);

            spriteBatch.DrawString(_font, $"Lives: {GameEntitiesManager.Instance.Lives}", new Vector2(0.0f, 50.0f), Color.Red);
            spriteBatch.DrawString(_font, $"Lives: {GameEntitiesManager.Instance.Lives}", new Vector2(1.0f, 51.0f), Color.White);
        }
    }
}