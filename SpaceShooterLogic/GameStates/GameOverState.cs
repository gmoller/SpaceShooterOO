using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class GameOverState : IGameState
    {
        private readonly MenuButton _restartButton;

        public GameOverState(MenuButton restartButton)
        {
            _restartButton = restartButton;
        }

        public void Enter()
        {
            _restartButton.IsActive = true;
        }

        public void Leave()
        {
            _restartButton.IsActive = false;
        }

        public (bool changeGameState, IGameState newGameState) Update(GameTime gameTime)
        {
            if (_restartButton.IsActive)
            {
                MouseState mouseState = Mouse.GetState();
                if (_restartButton.BoundingBox.Contains(mouseState.Position))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        _restartButton.SetDown(true);
                        _restartButton.SetHovered(false);
                    }
                    else
                    {
                        _restartButton.SetDown(false);
                        _restartButton.SetHovered(true);
                    }
                    if (mouseState.LeftButton == ButtonState.Released && _restartButton.LastIsDown)
                    {
                        return (true, new GamePlayState());
                    }
                }
                else
                {
                    _restartButton.SetDown(false);
                    _restartButton.SetHovered(false);
                }
                _restartButton.LastIsDown = mouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                _restartButton.IsActive = true;
            }

            return (false, null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string title = "GAME OVER";
            var fontArial = AssetsManager.Instance.GetSpriteFont("arialHeading");
            spriteBatch.DrawString(fontArial, title, new Vector2(DeviceManager.Instance.ScreenWidth * 0.5f - (fontArial.MeasureString(title).X * 0.5f), DeviceManager.Instance.ScreenHeight * 0.2f), Color.White);
            _restartButton.Draw(spriteBatch);
        }
    }
}