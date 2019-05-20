using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public class Projectile : Entity
    {
        public Projectile(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Scale = new Vector2(1.5f, 1.5f);
            SourceOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            DestinationOrigin = new Vector2(texture.Width * 0.5f * Scale.X, texture.Height * 0.5f * Scale.Y);
            Position = position;
            Body.Velocity = velocity;
            SetupBoundingBox(texture.Width, texture.Height);
        }
    }

    public class Projectiles
    {
        private readonly List<Projectile> _projectiles = new List<Projectile>();

        public void Add(Projectile projectile)
        {
            _projectiles.Add(projectile);
        }

        public void Update(GameTime gameTime)
        {
            Movement(gameTime);
            CollisionDetectionWithPlayer();
            CollisionDetectionWithEnemies();
        }

        private void Movement(GameTime gameTime)
        {
            for (int i = 0; i < _projectiles.Count; i++)
            {
                var projectile = _projectiles[i];
                projectile.Update(gameTime);
                if (projectile.Position.Y > DeviceManager.Instance.ScreenHeight || projectile.Position.Y < 0)
                {
                    _projectiles.Remove(projectile);
                }
            }
        }

        private void CollisionDetectionWithPlayer()
        {
            var player = GameEntitiesManager.Instance.Player;

            for (int i = 0; i < _projectiles.Count; i++)
            {
                Projectile projectile = _projectiles[i];
                if (player.IsAlive)
                {
                    if (player.Body.BoundingBox.Intersects(projectile.Body.BoundingBox))
                    {
                        // enemy projectile kills player
                        player.KillPlayer();
                        _projectiles.Remove(projectile);
                    }
                }
            }
        }

        private void CollisionDetectionWithEnemies()
        {
            for (int i = 0; i < _projectiles.Count; i++)
            {
                Projectile projectile = _projectiles[i];
                if (GameEntitiesManager.Instance.Enemies.CollisionDetectionWithProjectile(projectile))
                {
                    _projectiles.Remove(projectile);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }
    }
}