using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.Enemies
{
    public abstract class Enemy : Entity
    {
        protected AnimatedSprite Sprite;
        public float Angle { get; set; }

        protected readonly List<Projectile> Projectiles;

        protected Enemy(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Sprite = new AnimatedSprite(texture.Width, 16, 16, 160);
            Scale = new Vector2(RandomGenerator.Instance.GetRandomFloat(1.0f, 2.0f));
            SourceOrigin = new Vector2(Sprite.FrameWidth * 0.5f, Sprite.FrameHeight * 0.5f);
            DestinationOrigin = new Vector2(Sprite.FrameWidth * 0.5f * Scale.X, Sprite.FrameHeight * 0.5f * Scale.Y);
            Position = position;
            Body.Velocity = velocity;
            SetupBoundingBox(Sprite.FrameWidth, Sprite.FrameHeight);

            Projectiles = new List<Projectile>();
        }

        public override void Update(GameTime gameTime)
        {
            MoveProjectiles(gameTime);
            ProjectileCollisionDetectionAndResolution();

            Sprite.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var destRect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(Sprite.FrameWidth * Scale.X),
                (int)(Sprite.FrameHeight * Scale.Y));

            spriteBatch.Draw(Texture, destRect, Sprite.SourceRectangle, Color.White, IsRotatable ? MathHelper.ToRadians(Angle) : 0.0f, SourceOrigin, SpriteEffects.None, 0);

            //spriteBatch.DrawRectangle(Body.BoundingBox, Color.Yellow, 1.0f);

            foreach (Projectile projectile in Projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }

        public abstract int Score { get; }

        public abstract void UseSpecialPower();

        private void MoveProjectiles(GameTime gameTime)
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Update(gameTime);
                if (Projectiles[i].Position.Y > DeviceManager.Instance.ScreenHeight)
                {
                    Projectiles.Remove(Projectiles[i]);
                }
            }
        }

        private void ProjectileCollisionDetectionAndResolution()
        {
            var player = GameEntitiesManager.Instance.Player;

            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectile projectile = Projectiles[i];
                if (player.IsAlive)
                {
                    if (player.Body.BoundingBox.Intersects(projectile.Body.BoundingBox))
                    {
                        // enemy projectile kills player
                        player.KillPlayer();
                        Projectiles.Remove(projectile);
                    }
                }
            }
        }
    }

    public class Enemies
    {
        private const int SPAWN_ENEMY_COOLDOWN = 1000; // in milliseconds (3600)

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
                        // enemy kills player
                        player.KillPlayer();
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
                    _enemies.Remove(enemy);

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
            var explosion = new Explosion(AssetsManager.Instance.GetTexture("sprExplosion"), new Vector2(enemy.Position.X, enemy.Position.Y)) { Scale = enemy.Scale };
            GameEntitiesManager.Instance.Explosions.Add(explosion);
            GameEntitiesManager.Instance.Score += enemy.Score;
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
            Vector2 spawnPos = new Vector2(RandomGenerator.Instance.GetRandomFloat(0, DeviceManager.Instance.ScreenWidth), -20.0f); // -128.0f
            float velocity = RandomGenerator.Instance.GetRandomFloat(60.0f, 180.0f);
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