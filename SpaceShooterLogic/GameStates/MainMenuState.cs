using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class MainMenuState : IGameState
    {
        private readonly List<MenuButton> _menuItems;

        public MainMenuState(params MenuButton[] menuItems)
        {
            _menuItems = new List<MenuButton>();

            foreach (MenuButton item in menuItems)
            {
                _menuItems.Add(item);
            }
        }

        public void Enter()
        {
            foreach (MenuButton item in _menuItems)
            {
                item.IsActive = true;
            }
        }

        public void Leave()
        {
            foreach (MenuButton item in _menuItems)
            {
                item.IsActive = false;
            }
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

            MouseState mouseState = Mouse.GetState();

            foreach (MenuButton item in _menuItems)
            {
                if (item.BoundingBox.Contains(mouseState.Position))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        item.SetDown(true);
                        item.SetHovered(false);
                    }
                    else
                    {
                        item.SetDown(false);
                        item.SetHovered(true);
                    }

                    if (mouseState.LeftButton == ButtonState.Released && item.LastIsDown)
                    {
                        return (true, new GamePlayState());
                    }
                }
                else
                {
                    item.SetDown(false);
                    item.SetHovered(false);
                }

                item.LastIsDown = mouseState.LeftButton == ButtonState.Pressed;
            }

            return (false, null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var fontArial = AssetsManager.Instance.GetSpriteFont("arialHeading");
            string title = "SPACE SHOOTER";
            spriteBatch.DrawString(fontArial, title, new Vector2(DeviceManager.Instance.ScreenWidth * 0.5f - fontArial.MeasureString(title).X * 0.5f, DeviceManager.Instance.ScreenHeight * 0.2f), Color.White);

            foreach (MenuButton item in _menuItems)
            {
                item.Draw(spriteBatch);
            }
        }
    }
}