using System;

namespace SpaceShooterUtilities
{
    public sealed class DeviceManager
    {
        private static readonly Lazy<DeviceManager> Lazy = new Lazy<DeviceManager>(() => new DeviceManager());

        public static DeviceManager Instance => Lazy.Value;

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        private DeviceManager()
        {
        }
    }
}