using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GuiControls;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class MainMenuState : IGameState
    {
        private readonly Label _lblTitle;
        private readonly Button _btnPlay;

        private bool _startGame;

        public MainMenuState()
        {
            var fontArial = AssetsManager.Instance.GetSpriteFont("arialHeading");
            var fontLed = AssetsManager.Instance.GetDynamicSpriteFont("The Led Display St");

            string title = "SPACE SHOOTER";
            _lblTitle = new Label(fontLed, VerticalAlignment.Middle, HorizontalAlignment.Center,
                new Vector2(DeviceManager.Instance.ScreenWidth * 0.5f, DeviceManager.Instance.ScreenHeight * 0.2f),
                title, Color.Red) { TextShadow = true, TextShadowOffset = new Vector2(5.0f, 5.0f) };

            _btnPlay = new Button(fontArial, VerticalAlignment.Middle, HorizontalAlignment.Center,
                DeviceManager.Instance.ScreenDimensions * 0.5f, new Vector2(128.0f, 32.0f), string.Empty, Color.White, 1.0f, 1.0f, "sprBtnPlay",
                "sndBtn");
            _btnPlay.OnClick += btnPlay_Click;
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
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.F1)) // record
            {
                return (true, new GamePlayStateWithRecording());
            }

            if (keyboardState.IsKeyDown(Keys.F2)) // replay
            {
                return (true, new GamePlayStateWithReplaying());
            }

            _btnPlay.Update(gameTime);

            if (_startGame)
            {
                return (true, new GamePlayState());
            }

            return (false, null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _lblTitle.Draw(spriteBatch);

            _btnPlay.Draw(spriteBatch);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            _startGame = true;
        }
    }
}