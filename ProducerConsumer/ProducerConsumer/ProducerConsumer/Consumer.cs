using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Consumer
    {
        private ArrayList buffer;
        private int consumerId;

        public Consumer(ArrayList buffer, int i)
        {
            this.buffer = buffer;
            this.consumerId = i;
        }

        public void Consume()
        {
            int i = 0;
            while (i < 10)
            {
                int element = 0;
                if (buffer.Count >= 0)
                {
                    lock (buffer)
                    {
                        int position = buffer.Count;
                        element = (int)buffer[position];
                        buffer.RemoveAt(position);
                    }
                    Console.WriteLine("Consumer with id {0} consume element with number {1} and value {2}", consumerId, buffer.Count - 1, element);
                    Thread.Sleep(600);
                }
            }
        }
    }
}
