using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterLogic.Enemies
{
    public class ChaserShip : Enemy
    {
        private const float RANGE_TO_START_CHASING = 320.0f;
        private const float CHASING_MOVE_SPEED = 180.0f; // pixels per second
        private const float ROTATION_SPEED = 60.0f;

        public enum ChasingState
        {
            MoveDown,
            Chase
        }

        private ChasingState _state = ChasingState.MoveDown;

        public ChaserShip(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
        {
            Angle = 0;
        }

        public override int Score => 10;

        public override void UseSpecialPower()
        {
            Player player = GameEntitiesManager.Instance.Player;

            if (Vector2.Distance(Position, player.Position + player.DestinationOrigin) < RANGE_TO_START_CHASING)
            {
                _state = ChasingState.Chase;
                IsRotatable = true;
            }
            if (_state == ChasingState.Chase)
            {
                Vector2 direction = (player.Position + player.DestinationOrigin) - Position;
                direction.Normalize();
                Body.Velocity = direction * CHASING_MOVE_SPEED;
                if (Position.X + DestinationOrigin.X < player.Position.X + player.DestinationOrigin.X)
                {
                    Angle -= ROTATION_SPEED;
                }
                else
                {
                    Angle += ROTATION_SPEED;
                }
            }
        }
    }
}