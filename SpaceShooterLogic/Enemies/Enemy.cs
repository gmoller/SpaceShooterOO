using System.Collections.Generic;
using AnimationLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.Enemies
{
    public abstract class Enemy : Entity
    {
        protected AnimatedSprite Sprite;
        public float Angle { get; set; }

        protected Enemy(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            AnimationSpec animationSpec = AssetsManager.Instance.GetAnimations(texture.Name);
            Sprite = new AnimatedSprite(animationSpec);
            Scale = new Vector2(RandomGenerator.Instance.GetRandomFloat(1.0f, 3.0f));
            SourceOrigin = new Vector2(Sprite.FrameWidth * 0.5f, Sprite.FrameHeight * 0.5f);
            DestinationOrigin = new Vector2(Sprite.FrameWidth * 0.5f * Scale.X, Sprite.FrameHeight * 0.5f * Scale.Y);
            Position = position;
            Body.Velocity = velocity;
            SetupBoundingBox(Sprite.FrameWidth, Sprite.FrameHeight);
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var destRect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(Sprite.FrameWidth * Scale.X),
                (int)(Sprite.FrameHeight * Scale.Y));

            spriteBatch.Draw(Texture, destRect, Sprite.GetCurrentFrame(), Color.White, IsRotatable ? MathHelper.ToRadians(Angle) : 0.0f, SourceOrigin, SpriteEffects.None, 0);

            //spriteBatch.DrawRectangle(Body.BoundingBox, Color.Yellow, 1.0f);
        }

        public abstract int Score { get; }

        public abstract void UseSpecialPower();
    }

    public class Enemies
    {
        private const int SPAWN_ENEMY_COOLDOWN = 1000; // in milliseconds (3600)
        private const float MIN_ENEMY_VELOCITY = 60.0f;
        private const float MAX_ENEMY_VELOCITY = 180.0f;

        private readonly List<Enemy> _enemies = new List<Enemy>();

        private float _timeElapsedSinceLastEnemySpawned; // in milliseconds

        public void Add(Enemy enemy)
        {
            _enemies.Add(enemy);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                Enemy enemy = _enemies[i];
                enemy.Update(gameTime);
                if (enemy.Position.Y > DeviceManager.Instance.ScreenHeight)
                {
                    _enemies.Remove(enemy);
                }
            }

            var player = GameEntitiesManager.Instance.Player;
            if (player.IsAlive)
            {
                foreach (Enemy enemy in _enemies)
                {
                    if (player.Body.BoundingBox.Intersects(enemy.Body.BoundingBox))
                    {
                        // enemy and player collide
                        player.KillPlayer();
                        KillEnemy(enemy);
                        return;
                    }

                    enemy.UseSpecialPower();
                }
            }

            SpawnEnemies(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        public bool CollisionDetectionWithProjectile(Projectile projectile)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                Enemy enemy = _enemies[i];
                if (projectile.Body.BoundingBox.Intersects(enemy.Body.BoundingBox))
                {
                    // player projectile kills enemy
                    KillEnemy(enemy);

                    return true;
                }
            }

            return false;
        }

        private void KillEnemy(Enemy enemy)
        {
            int idx = RandomGenerator.Instance.GetRandomInt(0, 1);
            var sndExplode = AssetsManager.Instance.GetSound($"sndExplode{idx}");
            sndExplode.Play();

            Texture2D texture = AssetsManager.Instance.GetTexture("Explosion10");
            AnimationSpec animationSpec = AssetsManager.Instance.GetAnimations("Explosion10");
            var explosionPosition = new Vector2(enemy.Position.X, enemy.Position.Y);
            var enemySize = new Vector2(enemy.Body.BoundingBox.Width, enemy.Body.BoundingBox.Height);
            var explosion = new Explosion(texture, animationSpec, explosionPosition, enemySize); // { Scale = enemy.Scale };
            GameEntitiesManager.Instance.Explosions.Add(explosion);
            GameEntitiesManager.Instance.Score += enemy.Score;
            _enemies.Remove(enemy);
        }

        private void SpawnEnemies(GameTime gameTime)
        {
            if (!EnemySpawnerOnCooldown(gameTime))
            {
                SpawnEnemy();
                StartEnemySpawnerCooldown();
            }
        }

        private bool EnemySpawnerOnCooldown(GameTime gameTime)
        {
            if (_timeElapsedSinceLastEnemySpawned < SPAWN_ENEMY_COOLDOWN)
            {
                _timeElapsedSinceLastEnemySpawned += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                return true;
            }

            return false;
        }

        private void StartEnemySpawnerCooldown()
        {
            _timeElapsedSinceLastEnemySpawned = 0.0f;
        }

        private void SpawnEnemy()
        {
            Enemy enemy;
            int choice = RandomGenerator.Instance.GetRandomInt(1, 10);
            Vector2 spawnPos = new Vector2(RandomGenerator.Instance.GetRandomFloat(0, DeviceManager.Instance.ScreenWidth), -20.0f);
            float velocity = RandomGenerator.Instance.GetRandomFloat(MIN_ENEMY_VELOCITY, MAX_ENEMY_VELOCITY);
            if (choice <= 3)
            {
                enemy = new GunShip(AssetsManager.Instance.GetTexture("sprEnemy0"), spawnPos, new Vector2(0, velocity));
            }
            else if (choice >= 5)
            {
                enemy = new ChaserShip(AssetsManager.Instance.GetTexture("sprEnemy1"), spawnPos, new Vector2(0, velocity));
            }
            else
            {
                enemy = new CarrierShip(AssetsManager.Instance.GetTexture("sprEnemy2"), spawnPos, new Vector2(0, velocity));
            }
            GameEntitiesManager.Instance.Enemies.Add(enemy);
        }
    }
}