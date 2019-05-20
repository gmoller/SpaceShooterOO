using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class GameOverState : IGameState
    {
        private readonly Label _lblGameOver;
        private readonly Button _btnRestart;
        private readonly MetricsDisplay _metricsDisplay;

        private bool _startGame;

        public GameOverState()
        {
            var fontArial = AssetsManager.Instance.GetSpriteFont("arialHeading");

            string title = "GAME OVER";
            _lblGameOver = new Label(fontArial, VerticalAlignment.Middle, HorizontalAlignment.Center,
                new Vector2(DeviceManager.Instance.ScreenWidth * 0.5f, DeviceManager.Instance.ScreenHeight * 0.2f),
                title, Color.White);

            _btnRestart = new Button(fontArial, VerticalAlignment.Middle, HorizontalAlignment.Center,
                DeviceManager.Instance.ScreenDimensions * 0.5f, new Vector2(128.0f, 32.0f), string.Empty, Color.White, 1.0f, 1.0f, "sprBtnRestart",
                "sndBtn");
            _btnRestart.OnClick += btnRestart_Click;

            _metricsDisplay = new MetricsDisplay();
        }

        public void Enter()
        {
            DeviceManager.Instance.IsMouseVisible = true;
        }

        public void Leave()
        {
            DeviceManager.Instance.IsMouseVisible = false;
        }

        public (bool changeGameState, IGameState newGameState) Update(GameTime gameTime)
        {
            _btnRestart.Update(gameTime);

            _metricsDisplay.Update(gameTime);

            if (_startGame)
            {
                return (true, new GamePlayState());
            }

            return (false, null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _lblGameOver.Alpha = RandomGenerator.Instance.GetRandomFloat(0.5f, 1.0f);
            _lblGameOver.Draw(spriteBatch);

            _btnRestart.Draw(spriteBatch);

            _metricsDisplay.Draw(spriteBatch);
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            _startGame = true;
        }
    }
}