using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooterUtilities;

namespace GuiControls
{
    public class Button : Control
    {
        private Texture2D _texture;
        private readonly string _defaultTexture;
        private readonly string _mouseOverTexture;
        private readonly string _mouseClickedTexture;

        private readonly string _mouseOverSound;
        private readonly string _mouseClickedSound;

        private readonly Label _label;

        private ControlState _controlState;
        private float _clickedCountdown;

        public event EventHandler OnClick;

        public Button(SpriteFont font, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, Vector2 position, Vector2 size, string text, Color textColor, float scale, float alpha, string texture, string soundEffect) :
            base(verticalAlignment, horizontalAlignment, position)
        {
            _defaultTexture = texture;
            _mouseOverTexture = $"{texture}Hover";
            _mouseClickedTexture = $"{texture}Down";
            _mouseOverSound = $"{soundEffect}Over";
            _mouseClickedSound = $"{soundEffect}Down";

            _texture = AssetsManager.Instance.GetTexture(texture);

            Size = size;
            Scale = scale;
            Alpha = alpha;

            _label = new Label(font, VerticalAlignment.Middle, HorizontalAlignment.Center, new Vector2(Area.X + Area.Width / 2.0f, Area.Y + Area.Height / 2.0f), text, textColor, scale);
        }

        public void Update(GameTime gameTime)
        {
            if (_clickedCountdown > 0.0f)
            {
                _clickedCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_clickedCountdown <= 0.0f)
                {
                    ClickComplete(new EventArgs());
                }
            }
            else
            {
                MouseState mouseState = Mouse.GetState();
                if (Area.Contains(mouseState.Position))
                {
                    if (_controlState == ControlState.None)
                    {
                        _controlState = ControlState.MouseOver;
                        _texture = AssetsManager.Instance.GetTexture(_mouseOverTexture);
                        AssetsManager.Instance.GetSound(_mouseOverSound).Play();
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed && _controlState == ControlState.MouseOver)
                    {
                        Click(new EventArgs());
                    }
                }
                else
                {
                    _controlState = ControlState.None;
                    _texture = AssetsManager.Instance.GetTexture(_defaultTexture);
                }
            }

            float scale = _controlState == ControlState.MouseOver ? 1.1f : 1.0f;
            _label.Scale = scale;
            _label.Alpha = Alpha;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White * Alpha;

            spriteBatch.Draw(_texture, Area, color);
            _label.Draw(spriteBatch);
        }

        private void Click(EventArgs e)
        {
            _controlState = ControlState.Clicked;
            _texture = AssetsManager.Instance.GetTexture(_mouseClickedTexture);
            AssetsManager.Instance.GetSound(_mouseClickedSound).Play();

            _clickedCountdown = 0.2f; // in seconds
            Size.X -= 5;
            Size.Y -= 5;
            OnClick?.Invoke(this, e);
        }

        private void ClickComplete(EventArgs e)
        {
            _clickedCountdown = 0.0f;
            Size.X += 5;
            Size.Y += 5;
            //OnClick?.Invoke(this, e);
            _controlState = ControlState.MouseOver;
        }
    }

    public enum ControlState
    {
        None,
        MouseOver,
        Clicked
    }
}