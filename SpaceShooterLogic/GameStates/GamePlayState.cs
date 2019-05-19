using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class GamePlayState : IGameState
    {
        private float _timeElapsedSinceDied; // in seconds
        private readonly int _restartDelay = 3; // in seconds

        public virtual void Enter()
        {
            ResetLevel();
            SetController();
            GameEntitiesManager.Instance.Score = 0;
            GameEntitiesManager.Instance.Lives = 1; // 3
        }

        public virtual void Leave()
        {
            GameEntitiesManager.Instance.Player = null;
            GameEntitiesManager.Instance.Enemies = null;
            GameEntitiesManager.Instance.Explosions = null;
        }

        protected virtual void SetController()
        {
            IPlayerController playerController = new PlayerController();
            GameEntitiesManager.Instance.Player.SetController(playerController);
        }

        public (bool changeGameState, IGameState newGameState) Update(GameTime gameTime)
        {
            GameEntitiesManager.Instance.Player.Update(gameTime);
            GameEntitiesManager.Instance.Enemies.Update(gameTime);
            GameEntitiesManager.Instance.Explosions.Update(gameTime);

            if (GameEntitiesManager.Instance.Player.IsDead)
            {
                bool changeToGameOverState = CheckForChangeToGameOverState(gameTime);
                if (changeToGameOverState)
                {
                    return (true, new GameOverState());
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
            if (GameEntitiesManager.Instance.Lives > 1)
            {
                ResetLevel();
                GameEntitiesManager.Instance.Lives--;
                SetController();
                return false;
            }

            // out of lives, game over!
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameEntitiesManager.Instance.Player.Draw(spriteBatch);
            GameEntitiesManager.Instance.Enemies.Draw(spriteBatch);
            GameEntitiesManager.Instance.Explosions.Draw(spriteBatch);
            GameEntitiesManager.Instance.Hud.Draw(spriteBatch);
        }

        private void ResetLevel()
        {
            GameEntitiesManager.Instance.Player = new Player(AssetsManager.Instance.GetTexture("sprPlayer"), DeviceManager.Instance.ScreenDimensions * 0.5f);
            GameEntitiesManager.Instance.Enemies = new Enemies.Enemies();
            GameEntitiesManager.Instance.Explosions = new Explosions();
            GameEntitiesManager.Instance.Hud = new Hud();
        }
    }
}