using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class GamePlayState : IGameState
    {
        private float _timeElapsedSinceDied; // in seconds
        private readonly int _restartDelay = 3; // in seconds

        private readonly bool _mustRecord;

        public GamePlayState(bool record = false)
        {
            _mustRecord = record;
        }

        public void Enter()
        {
            if (_mustRecord)
            {
                Recorder.Instance.StartRecording(1);
            }

            GameEntitiesManager.Instance.Player = new Player(AssetsManager.Instance.GetTexture("sprPlayer"), new Vector2(DeviceManager.Instance.ScreenWidth * 0.5f, DeviceManager.Instance.ScreenHeight * 0.5f));
            GameEntitiesManager.Instance.Enemies = new Enemies.Enemies();
            GameEntitiesManager.Instance.Explosions = new Explosions();
            GameEntitiesManager.Instance.Hud = new Hud();
            GameEntitiesManager.Instance.Score = 0;
            GameEntitiesManager.Instance.Lives = 3;
        }

        public void Leave()
        {
            GameEntitiesManager.Instance.Player = null;
            GameEntitiesManager.Instance.Enemies = null;
            GameEntitiesManager.Instance.Explosions = null;

            if (_mustRecord)
            {
                Recorder.Instance.StopRecording();
            }
        }

        public (bool changeGameState, IGameState newGameState) Update(GameTime gameTime)
        {
            GameEntitiesManager.Instance.Player.Update(gameTime);
            GameEntitiesManager.Instance.Enemies.Update(gameTime);
            GameEntitiesManager.Instance.Explosions.Update(gameTime);

            if (GameEntitiesManager.Instance.Player.IsDead)
            {
                // IDEA: fadeout period
                bool changeToGameOverState = CheckForChangeToGameOverState(gameTime);
                if (changeToGameOverState)
                {
                    return (true, new GameOverState(new MenuButton("sprBtnRestart", "sprBtnRestartDown", "sprBtnRestartHover", new Vector2(DeviceManager.Instance.ScreenWidth * 0.5f, DeviceManager.Instance.ScreenHeight * 0.5f))));
                }
            }

            return (false, null);
        }

        private bool CheckForChangeToGameOverState(GameTime gameTime)
        {
            if (_timeElapsedSinceDied < _restartDelay)
            {
                _timeElapsedSinceDied += (float)gameTime.ElapsedGameTime.TotalSeconds;

                return false;
            }

            _timeElapsedSinceDied = 0.0f;

            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameEntitiesManager.Instance.Player.Draw(spriteBatch);
            GameEntitiesManager.Instance.Enemies.Draw(spriteBatch);
            GameEntitiesManager.Instance.Explosions.Draw(spriteBatch);
            GameEntitiesManager.Instance.Hud.Draw(spriteBatch);
        }
    }
}