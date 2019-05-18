using System;

namespace SpaceShooterLogic
{
    public sealed class GameEntitiesManager
    {
        private static readonly Lazy<GameEntitiesManager> Lazy = new Lazy<GameEntitiesManager>(() => new GameEntitiesManager());

        public static GameEntitiesManager Instance => Lazy.Value;

        public Player Player { get; set; }
        public Enemies.Enemies Enemies { get; set; }
        public Explosions Explosions { get; set; }
        public Hud Hud { get; set; }

        public int Score { get; set; }
        public int Lives { get; set; }

        private GameEntitiesManager()
        {
        }
    }
}