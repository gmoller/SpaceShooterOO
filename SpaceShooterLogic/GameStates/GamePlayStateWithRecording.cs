﻿using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class GamePlayStateWithRecording : GamePlayState
    {
        public override void Enter()
        {
            base.Enter();

            Recorder.Instance.StartRecording(1);
        }

        protected override void SetController()
        {
            IPlayerController playerController = new PlayerControllerWithRecorder();
            GameEntitiesManager.Instance.Player.SetController(playerController);
        }

        public override void Leave()
        {
            base.Leave();

            Recorder.Instance.StopRecording();
        }
    }
}