using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterLogic.GameStates;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public class SpaceShooterGame
    {
        private ScrollingBackground _scrollingBackground;

        private IGameState _gameState;

        public void LoadContent(ContentManager content, int width, int height)
        {
            AssetsManager.Instance.ContentManager = content;
            DeviceManager.Instance.ScreenWidth = width;
            DeviceManager.Instance.ScreenHeight = height;

            // Load textures
            AssetsManager.Instance.AddTextures("sprBg0", "sprBg1", "sprBtnPlay", "sprBtnPlayDown", "sprBtnPlayHover", "sprBtnRestart", "sprBtnRestartDown", "sprBtnRestartHover", "sprPlayer", "sprLaserPlayer", "sprLaserEnemy0", "sprExplosion", "sprEnemy0", "sprEnemy1", "sprEnemy2");
            // Load sounds
            AssetsManager.Instance.AddSounds("sndBtnDown", "sndBtnOver", "sndLaser", "sndExplode0", "sndExplode1");
            // Load sprite fonts
            AssetsManager.Instance.AddSpriteFonts("arialHeading");

            _scrollingBackground = new ScrollingBackground(new List<string> { "sprBg0", "sprBg1" });

            float x = DeviceManager.Instance.ScreenWidth * 0.5f;
            float y = DeviceManager.Instance.ScreenHeight * 0.5f;

            var button1 = new MenuButton(
                "sprBtnPlay",
                "sprBtnPlayDown",
                "sprBtnPlayHover",
                new Vector2(x, y));

            y += 50;
            var button2 = new MenuButton(
                "sprBtnPlay",
                "sprBtnPlayDown",
                "sprBtnPlayHover",
                new Vector2(x, y));

            y += 50;
            var button3 = new MenuButton(
                "sprBtnPlay",
                "sprBtnPlayDown",
                "sprBtnPlayHover",
                new Vector2(x, y));

            _gameState = new MainMenuState(
                button1);
            _gameState.Enter();
        }

        public void Update(GameTime gameTime)
        {
            _scrollingBackground.Update(gameTime);

            (bool changeGameState, IGameState newGameState) returnGameState = _gameState.Update(gameTime);
            if (returnGameState.changeGameState) ChangeGameState(returnGameState.newGameState);
        }

        private void ChangeGameState(IGameState newGameState)
        {
            _gameState.Leave();
            _gameState = newGameState;
            _gameState.Enter();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap);

            _scrollingBackground.Draw(spriteBatch);
            _gameState.Draw(spriteBatch);
            
            spriteBatch.End();
        }
    }
}