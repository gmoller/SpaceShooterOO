using GuiControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class GameOverState : IGameState
    {
        private readonly Label _lblGameOver;
        private readonly MenuButton _restartButton;

        public GameOverState(MenuButton restartButton)
        {
            var fontArial = AssetsManager.Instance.GetSpriteFont("arialHeading");
            string title = "GAME OVER";
            _lblGameOver = new Label(fontArial, VerticalAlignment.Middle, HorizontalAlignment.Center,
                new Vector2(DeviceManager.Instance.ScreenWidth * 0.5f, DeviceManager.Instance.ScreenHeight * 0.2f),
                title, Color.White);

            _restartButton = restartButton;
        }

        public void Enter()
        {
            //IsMouseVisible = true;
            _restartButton.IsActive = true;
        }

        public void Leave()
        {
            _restartButton.IsActive = false;
            //IsMouseVisible = false;
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
            _lblGameOver.Alpha = RandomGenerator.Instance.GetRandomFloat(0.5f, 1.0f);
            _lblGameOver.Draw(spriteBatch);

            _restartButton.Draw(spriteBatch);
        }
    }
}