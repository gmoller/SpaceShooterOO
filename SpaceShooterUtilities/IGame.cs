using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterUtilities
{
    public interface IGame
    {
        void Initialize(GameWindow window, GraphicsDevice graphicsDevice);
        void LoadContent(ContentManager content, int width, int height);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}