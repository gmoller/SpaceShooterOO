using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterUtilities
{
    public class ResolutionIndependentRenderer
    {
        private readonly Game _game;
        private Viewport _viewport;
        private Vector2 _ratio;
        private Vector2 _virtualMousePosition;
        private static Matrix _scaleMatrix;
        private bool _dirtyMatrix = true;

        public int VirtualHeight { get; set; }
        public int VirtualWidth { get; set; }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        public ResolutionIndependentRenderer(Game game)
        {
            _game = game;
            _virtualMousePosition = Vector2.Zero;

            VirtualWidth = 1366;
            VirtualHeight = 768;

            ScreenWidth = 1024;
            ScreenHeight = 768;
        }

        public void Initialize()
        {
            SetupVirtualScreenViewport();

            _ratio = new Vector2((float)_viewport.Width / VirtualWidth, (float)_viewport.Height / VirtualHeight);

            _dirtyMatrix = true;
        }

        public void BeginDraw()
        {
            // Start by resetting viewport to (0,0,1,1)
            SetupFullViewport();

            // Clear full viewport
            _game.GraphicsDevice.Clear(Color.Orange);

            // Calculate proper viewport according to aspect ratio
            SetupVirtualScreenViewport();
            // and clear that
            // This way we are going to have bars if aspect ratio requires it and
            // the clear color on the rest
        }

        private void SetupFullViewport()
        {
            var vp = new Viewport { X = 0, Y = 0, Width = ScreenWidth, Height = ScreenHeight };
            _game.GraphicsDevice.Viewport = vp;
            _dirtyMatrix = true;
        }

        private void SetupVirtualScreenViewport()
        {
            var targetAspectRatio = VirtualWidth / (float)VirtualHeight;

            // figure out the largest area that fits in this resolution at the desired aspect ratio
            var width = ScreenWidth;
            var height = (int)(width / targetAspectRatio + 0.5f);

            if (height > ScreenHeight)
            {
                height = ScreenHeight;
                // PillarBox
                width = (int)(height * targetAspectRatio + 0.5f);
            }

            // set up the new viewport centered in the back buffer
            _viewport = new Viewport
            {
                X = (ScreenWidth / 2) - (width / 2),
                Y = (ScreenHeight / 2) - (height / 2),
                Width = width,
                Height = height
            };

            _game.GraphicsDevice.Viewport = _viewport;
        }

        public Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition)
        {
            var realX = screenPosition.X - _viewport.X;
            var realY = screenPosition.Y - _viewport.Y;

            _virtualMousePosition.X = realX / _ratio.X;
            _virtualMousePosition.Y = realY / _ratio.Y;

            return _virtualMousePosition;
        }

        public Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix)
                RecreateScaleMatrix();

            return _scaleMatrix;
        }

        private void RecreateScaleMatrix()
        {
            Matrix.CreateScale((float)ScreenWidth / VirtualWidth, (float)ScreenWidth / VirtualWidth, 1f, out _scaleMatrix);
            _dirtyMatrix = false;
        }
    }
}