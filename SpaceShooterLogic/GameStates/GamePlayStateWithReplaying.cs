using SpaceShooterUtilities;

namespace SpaceShooterLogic.GameStates
{
    public class GamePlayStateWithReplaying : GamePlayState
    {
        public override void Enter()
        {
            base.Enter();

            Replayer.Instance.ReadInData();
            IPlayerController playerController = new PlayerControllerFromRecorder();
            GameEntitiesManager.Instance.Player.SetController(playerController);
        }
    }
}