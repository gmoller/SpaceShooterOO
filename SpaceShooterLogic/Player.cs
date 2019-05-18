using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public class Player : Entity
    {
        private const float MOVE_SPEED = 240.0f; // pixels per second
        private const int PLAYER_LASER_COOLDOWN = 200; // in milliseconds
        private const float PLAYER_LASER_VELOCITY = 600.0f; // in pixels per second

        private IPlayerController _playerController;

        private float _timeElapsedSinceLastPlayerShot; // in milliseconds
        private readonly List<Projectile> _projectiles;

        private readonly AnimatedSprite _sprite;

        public bool IsDead { get; private set; }
        public bool IsAlive => !IsDead;

        public Player(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            _sprite = new AnimatedSprite(texture.Width, 16, 16, 160);
            Scale = new Vector2(1.5f, 1.5f);
            SourceOrigin = new Vector2(_sprite.FrameWidth * 0.5f, _sprite.FrameHeight * 0.5f);
            DestinationOrigin = new Vector2(_sprite.FrameWidth * 0.5f * Scale.X, _sprite.FrameHeight * 0.5f * Scale.Y);
            Position = position;
            Body.Velocity = Vector2.Zero;
            SetupBoundingBox(_sprite.FrameWidth, _sprite.FrameHeight);

            _projectiles = new List<Projectile>();
            _timeElapsedSinceLastPlayerShot = PLAYER_LASER_COOLDOWN;
        }

        public override void Update(GameTime gameTime)
        {
            MoveProjectiles(gameTime);
            ProjectileCollisionDetectionAndResolution();

            if (IsDead) return;

            Body.Velocity = new Vector2(0, 0);

            HandleInput(gameTime);
            _sprite.Update(gameTime);

            base.Update(gameTime);

            // do not allow our player off the screen
            float x = Body.BoundingBox.Width / 2.0f;
            float y = Body.BoundingBox.Height / 2.0f;
            Position = new Vector2(
                MathHelper.Clamp(Position.X, x, DeviceManager.Instance.ScreenWidth - x),
                MathHelper.Clamp(Position.Y, y, DeviceManager.Instance.ScreenHeight - y));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var playerLaser in _projectiles)
            {
                playerLaser.Draw(spriteBatch);
            }

            if (IsDead) return;

            var destRect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(_sprite.FrameWidth * Scale.X),
                (int)(_sprite.FrameHeight * Scale.Y));
            spriteBatch.Draw(Texture, destRect, _sprite.SourceRectangle, Color.White, 0.0f, SourceOrigin, SpriteEffects.None, 0.0f);

            //spriteBatch.DrawRectangle(Body.BoundingBox, Color.Red, 1.0f);
        }

        public void SetController(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public void KillPlayer()
        {
            int i = RandomGenerator.Instance.GetRandomInt(0, 1);
            SoundEffect sndExplode = AssetsManager.Instance.GetSound($"sndExplode{i}");
            sndExplode.Play();
            var explosionPosition = new Vector2(Position.X, Position.Y);
            Explosion explosion = new Explosion(AssetsManager.Instance.GetTexture("sprExplosion"), explosionPosition);
            GameEntitiesManager.Instance.Explosions.Add(explosion);
            IsDead = true;
        }

        private void MoveProjectiles(GameTime gameTime)
        {
            for (int i = 0; i < _projectiles.Count; i++)
            {
                _projectiles[i].Update(gameTime);
                if (_projectiles[i].Position.Y < 0)
                {
                    _projectiles.Remove(_projectiles[i]);
                }
            }
        }

        private void ProjectileCollisionDetectionAndResolution()
        {
            for (int i = 0; i < _projectiles.Count; i++)
            {
                if (GameEntitiesManager.Instance.Enemies.CollisionDetectionWithProjectile(_projectiles[i]))
                {
                    _projectiles.Remove(_projectiles[i]);
                }
            }
        }

        private void HandleInput(GameTime gameTime)
        {
            List<PlayerAction> playerActions = _playerController.GetActions();

            foreach (PlayerAction playerAction in playerActions)
            {
                switch (playerAction)
                {
                    case PlayerAction.None:
                        // do nothing
                        break;
                    case PlayerAction.MoveUp:
                        Body.Velocity = new Vector2(Body.Velocity.X, -MOVE_SPEED);
                        break;
                    case PlayerAction.MoveDown:
                        Body.Velocity = new Vector2(Body.Velocity.X, MOVE_SPEED);
                        break;
                    case PlayerAction.MoveLeft:
                        Body.Velocity = new Vector2(-MOVE_SPEED, Body.Velocity.Y);
                        break;
                    case PlayerAction.MoveRight:
                        Body.Velocity = new Vector2(MOVE_SPEED, Body.Velocity.Y);
                        break;
                    case PlayerAction.FireLaser:
                        if (!PlayerLaserOnCooldown(gameTime))
                        {
                            ShootLaser();
                            StartPlayerLaserCooldown();
                        }

                        break;
                }
            }
        }

        private bool PlayerLaserOnCooldown(GameTime gameTime)
        {
            if (_timeElapsedSinceLastPlayerShot < PLAYER_LASER_COOLDOWN)
            {
                _timeElapsedSinceLastPlayerShot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                return true;
            }

            return false;
        }

        private void StartPlayerLaserCooldown()
        {
            _timeElapsedSinceLastPlayerShot = 0.0f;
        }

        private void ShootLaser()
        {
            AssetsManager.Instance.GetSound("sndLaser").Play();
            var laserPosition = new Vector2(Position.X, Position.Y);
            var projectile = new Projectile(AssetsManager.Instance.GetTexture("sprLaserPlayer"), laserPosition, new Vector2(0, -PLAYER_LASER_VELOCITY));
            _projectiles.Add(projectile);
            _timeElapsedSinceLastPlayerShot = 0.0f;
        }
    }
}