using System;
using System.Collections.Generic;

namespace SpaceShooterUtilities
{
    public sealed class BenchmarkMetrics
    {
        private static readonly Lazy<BenchmarkMetrics> Lazy = new Lazy<BenchmarkMetrics>(() => new BenchmarkMetrics());

        public static BenchmarkMetrics Instance => Lazy.Value;

        public Dictionary<string, Metric> Metrics { get; }

        private BenchmarkMetrics()
        {
            Metrics = new Dictionary<string, Metric>();
        }
    }

    public struct Metric
    {
        public double _elapsedTime;
        public int _frames;

        public Metric(double elapsedTime, int frames)
        {
            _elapsedTime = elapsedTime;
            _frames = frames;
        }
    }
}