using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.Enemies
{
    public class GunShip : Enemy
    {
        private const int LASER_VELOCITY = 300; // in pixels per second

        private float _timeElapsedSinceLastShot;
        private readonly int _durationInMillisecondsBetweenEachShot = 1000;

        public bool CanShoot { get; private set; }

        public GunShip(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
        {
        }

        public override int Score => 20;

        public override void Update(GameTime gameTime)
        {
            if (!CanShoot)
            {
                if (_timeElapsedSinceLastShot < _durationInMillisecondsBetweenEachShot)
                {
                    _timeElapsedSinceLastShot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    CanShoot = true;
                }
            }
            Sprite.Update(gameTime);

            base.Update(gameTime);
        }

        public override void UseSpecialPower()
        {
            if (CanShoot)
            {
                var projectile = new Projectile(AssetsManager.Instance.GetTexture("sprLaserEnemy0"), new Vector2(Position.X, Position.Y), new Vector2(0.0f, LASER_VELOCITY));
                Projectiles.Add(projectile);
                ResetCanShoot();
            }
        }

        private void ResetCanShoot()
        {
            CanShoot = false;
            _timeElapsedSinceLastShot = 0;
        }
    }
}