using System;
using System.Collections.Generic;
using System.IO;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public sealed class Recorder
    {
        private static readonly Lazy<Recorder> Lazy = new Lazy<Recorder>(() => new Recorder());

        public static Recorder Instance => Lazy.Value;

        private List<byte> _recordingData;

        public void StartRecording(byte seed)
        {
            RandomGenerator.Instance.SetNewRandom(seed);
            _recordingData = new List<byte> {seed};
        }

        public void RecordData(byte b)
        {
            _recordingData?.Add(b);
        }

        public void StopRecording()
        {
            // write to file
            using (var bw = new BinaryWriter(new FileStream("mydata.dat", FileMode.Create)))
            {
                foreach (byte b in _recordingData)
                {
                    bw.Write(b);
                }
                bw.Write(255); // marks eof

                bw.Close();
            }
        }
    }
}