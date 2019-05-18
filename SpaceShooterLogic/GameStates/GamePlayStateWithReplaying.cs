using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class GamePlayStateWithReplaying : GamePlayState
    {
        public override void Enter()
        {
            base.Enter();

            Replayer.Instance.ReadInData();
        }

        protected override void SetController()
        {
            IPlayerController playerController = new PlayerControllerFromRecorder();
            GameEntitiesManager.Instance.Player.SetController(playerController);
        }
    }
}