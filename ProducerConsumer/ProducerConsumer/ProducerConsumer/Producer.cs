using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Producer
    {
        private ArrayList buffer;
        private Random rand;
        private int producerId;

        public Producer(ArrayList buffer, int i)
        {
            this.buffer = buffer;
            this.producerId = i;
        }

        public void Produce()
        {
            int i = 0;
            while (i < 10)
            {

                if (buffer.Count <= buffer.Capacity)
                {
                    int element = rand.Next(1, 100);
                    lock (buffer)
                    {
                        buffer.Insert(buffer.Count, element);
                    }
                    Console.WriteLine("Producer with id {0} produce element with number {1} and value {2}", producerId, buffer.Count, element);
                    Thread.Sleep(300);

                }
                i++;
            }

        }
    }
}
