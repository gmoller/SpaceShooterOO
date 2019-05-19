using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceShooterUtilities
{
    public class Replayer
    {
        private static readonly Lazy<Replayer> Lazy = new Lazy<Replayer>(() => new Replayer());

        public static Replayer Instance => Lazy.Value;

        private Queue<byte> _queue;

        private Replayer()
        {
        }

        public void ReadInData()
        {
            _queue = new Queue<byte>();
            using (var br = new BinaryReader(new FileStream("mydata.dat", FileMode.Open)))
            {
                byte byteRead;
                do
                {
                    byteRead = br.ReadByte();
                    _queue.Enqueue(byteRead);

                } while (byteRead != 255);

                br.Close();
            }

            int seed = _queue.Dequeue();
            RandomGenerator.Instance.SetNewRandom(seed);
        }

        public byte ReadData()
        {
            return _queue.Dequeue();
        }
    }
}