using System;
using Microsoft.Xna.Framework;

namespace SpaceShooterUtilities
{
    public sealed class DeviceManager
    {
        private static readonly Lazy<DeviceManager> Lazy = new Lazy<DeviceManager>(() => new DeviceManager());

        public static DeviceManager Instance => Lazy.Value;

        public Rectangle Viewport { get; set; }

        public Vector2 ScreenDimensions => new Vector2(Viewport.Width, Viewport.Height);
        public int ScreenWidth => Viewport.Width;
        public int ScreenHeight => Viewport.Height;

        private DeviceManager()
        {
        }
    }
}