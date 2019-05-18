using System;
using Microsoft.Xna.Framework;

namespace SpaceShooter
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (Game game = new Game1())
            {
                game.Run();
            }
        }
    }
}