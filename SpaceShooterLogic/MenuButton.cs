using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public class MenuButton
    {
        private readonly Vector2 _position;
        private readonly string _textureDefault;
        private readonly string _textureOnDown;
        private readonly string _textureOnHover;
        private Texture2D _currentTexture;
        private bool _isDown;
        private bool _isHovered;

        public Rectangle BoundingBox { get; }
        public bool IsActive { get; set; }
        public bool LastIsDown { get; set; }

        public MenuButton(string assetNameDefault, string assetNameOnDown, string assetNameOnHover, Vector2 centerPosition)
        {
            _textureDefault = assetNameDefault;
            _textureOnDown = assetNameOnDown;
            _textureOnHover = assetNameOnHover;
            _currentTexture = AssetsManager.Instance.GetTexture(assetNameDefault);
            _position = new Vector2(centerPosition.X - (int)(_currentTexture.Width * 0.5f), centerPosition.Y - (int)(_currentTexture.Height * 0.5f));

            BoundingBox = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                _currentTexture.Width,
                _currentTexture.Height);
        }

        public void SetDown(bool isDown)
        {
            if (!_isDown && isDown)
            {
                AssetsManager.Instance.GetSound("sndBtnDown").Play();
            }
            _isDown = isDown;
            ChangeTexture();
        }

        public void SetHovered(bool isHovered)
        {
            if (!_isHovered && !_isDown && isHovered)
            {
                AssetsManager.Instance.GetSound("sndBtnOver").Play();
            }
            _isHovered = isHovered;
            ChangeTexture();
        }

        private void ChangeTexture()
        {
            if (_isDown)
            {
                _currentTexture = AssetsManager.Instance.GetTexture(_textureOnDown);
            }
            else
            {
                _currentTexture = _isHovered ? AssetsManager.Instance.GetTexture(_textureOnHover) : AssetsManager.Instance.GetTexture(_textureDefault);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.Draw(_currentTexture, _position, Color.White);
            }
        }
    }
}