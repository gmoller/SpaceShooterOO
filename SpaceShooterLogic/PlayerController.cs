using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public enum PlayerAction
    {
        None,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        FireLaser
    }

    public interface IPlayerController
    {
        List<PlayerAction> GetActions();
    }

    public class PlayerController : IPlayerController
    {
        public List<PlayerAction> GetActions()
        {
            var keyState = Keyboard.GetState();

            List<PlayerAction> playerActions = new List<PlayerAction>();

            if (keyState.IsKeyDown(Keys.W))
            {
                playerActions.Add(PlayerAction.MoveUp);
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                playerActions.Add(PlayerAction.MoveDown);
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                playerActions.Add(PlayerAction.MoveLeft);
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                playerActions.Add(PlayerAction.MoveRight);
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                playerActions.Add(PlayerAction.FireLaser);
            }

            return playerActions;
        }
    }

    public class PlayerControllerWithRecorder : IPlayerController
    {
        public List<PlayerAction> GetActions()
        {
            var keyState = Keyboard.GetState();

            byte keysPressed = 0;

            List<PlayerAction> playerActions = new List<PlayerAction>();

            if (keyState.IsKeyDown(Keys.W))
            {
                keysPressed |= 1 << 0; // bit 0
                playerActions.Add(PlayerAction.MoveUp);
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                keysPressed |= 1 << 1; // bit 1
                playerActions.Add(PlayerAction.MoveDown);
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                keysPressed |= 1 << 2; // bit 2
                playerActions.Add(PlayerAction.MoveLeft);
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                keysPressed |= 1 << 3; // bit 3
                playerActions.Add(PlayerAction.MoveRight);
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                keysPressed |= 1 << 4; // bit 4
                playerActions.Add(PlayerAction.FireLaser);
            }

            Recorder.Instance.RecordData(keysPressed);

            return playerActions;
        }
    }

    public class PlayerControllerFromRecorder : IPlayerController
    {
        private IActionsGetter _actionsGetter = new ActionsGetter();

        public List<PlayerAction> GetActions()
        {
            List<PlayerAction> playerActions;
            try
            {
                playerActions = _actionsGetter.GetActions();
            }
            catch (Exception)
            {
                _actionsGetter = new ActionsGetterEndOfStream();
                playerActions = new List<PlayerAction> { PlayerAction.None };
            }

            return playerActions;
        }
    }

    public interface IActionsGetter
    {
        List<PlayerAction> GetActions();
    }

    public class ActionsGetter : IActionsGetter
    {
        public List<PlayerAction> GetActions()
        {
            byte keysPressed = Replayer.Instance.ReadData();
            if (keysPressed == 255)
            {
                // we've reached the end - stop reading
                throw new Exception("End of stream.");
            }

            List<PlayerAction> playerActions = new List<PlayerAction>();
            if ((keysPressed & (1 << 0)) != 0) playerActions.Add(PlayerAction.MoveUp);
            if ((keysPressed & (1 << 1)) != 0) playerActions.Add(PlayerAction.MoveDown);
            if ((keysPressed & (1 << 2)) != 0) playerActions.Add(PlayerAction.MoveLeft);
            if ((keysPressed & (1 << 3)) != 0) playerActions.Add(PlayerAction.MoveRight);
            if ((keysPressed & (1 << 4)) != 0) playerActions.Add(PlayerAction.FireLaser);

            return playerActions;
        }
    }

    public class ActionsGetterEndOfStream : IActionsGetter
    {
        public List<PlayerAction> GetActions()
        {
            return new List<PlayerAction> { PlayerAction.None };
        }
    }
}