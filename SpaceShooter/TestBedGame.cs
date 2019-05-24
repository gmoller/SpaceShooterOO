using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooterUtilities;
using SpaceShooterUtilities.ViewportAdapters;

namespace SpaceShooter
{
    public class TestBedGame : IGame
    {
        private GameWindow _window;
        private GraphicsDevice _graphicsDevice;
        private ViewportAdapter _viewportAdapter;
        private OrthographicCamera _camera;
        private SpriteFont _font;

        public void Initialize(GameWindow window, GraphicsDevice graphicsDevice)
        {
            _window = window;
            _graphicsDevice = graphicsDevice;
        }

        public void LoadContent(ContentManager content, int width, int height)
        {
            _viewportAdapter = new ScalingViewportAdapter(_graphicsDevice, 800, 1200);
            //_viewportAdapter = new BoxingViewportAdapter(_window, _graphicsDevice, 800, 1200);
            _viewportAdapter.Reset();
            _camera = new OrthographicCamera(_viewportAdapter);

            ContentLoader.LoadContent(_graphicsDevice);
            AssetsManager.Instance.ContentManager = content;
            AssetsManager.Instance.AddSpriteFonts("arialHeading", "arialSmall", "arialTiny");
            _font = AssetsManager.Instance.GetSpriteFont("arialHeading");
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D1))
            {
                _viewportAdapter = new ScalingViewportAdapter(_graphicsDevice, 800, 1200);
                _viewportAdapter.Reset();
            }
            if (keyboardState.IsKeyDown(Keys.D2))
            {
                _viewportAdapter = new ScalingViewportAdapter(_graphicsDevice, 1600, 2400);
                _viewportAdapter.Reset();
            }

            _camera.Move(new Vector2(0.01f, 0.0f));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: _camera.GetViewMatrix()); // _viewportAdapter.GetScaleMatrix()
            spriteBatch.FillRectangle(new Rectangle(2, 2, 796, 1196), Color.LightBlue);
            spriteBatch.DrawCircle(new Vector2(400, 600), 400, 50, Color.Red);
            spriteBatch.DrawString(_font, $"Resolution: {_viewportAdapter.ViewportWidth}x{_viewportAdapter.ViewportHeight}", Vector2.Zero, Color.White);
            spriteBatch.DrawString(_font, $"Virtual Resolution: {_viewportAdapter.VirtualWidth}x{_viewportAdapter.VirtualHeight}", new Vector2(0.0f, 50.0f), Color.White);
            spriteBatch.End();
        }
    }
}