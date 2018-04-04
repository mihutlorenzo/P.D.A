using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Program
    {
        private static ArrayList buffer = new ArrayList(5);
        private const int NUMBERPRODUCERS = 3;
        private const int NUMBERCONSUMERS = 2;
        private static Producer[] producers = new Producer[NUMBERPRODUCERS];
        private static Consumer[] consumers = new Consumer[NUMBERCONSUMERS];
        private static Thread[] threadProducers = new Thread[NUMBERPRODUCERS];
        private static Thread[] threadConsuemrs = new Thread[NUMBERCONSUMERS];

        static void Main(string[] args)
        {
            for(int i = 0; i<NUMBERPRODUCERS;i++)
            {
                producers[i] = new Producer(buffer, i);
            }

            for(int i = 0;i<NUMBERCONSUMERS;i++)
            {
                consumers[i] = new Consumer(buffer, i);
            }

            for(int i = 0; i < NUMBERPRODUCERS; i++)
            {
                threadProducers[i] = new Thread(producers[i].Produce);
                threadProducers[i].Start();
            }

            for (int i = 0; i < NUMBERCONSUMERS; i++)
            {
                threadConsuemrs[i] = new Thread(consumers[i].Consume);
                threadConsuemrs[i].Start(); 
            }

            for (int i = 0; i < NUMBERPRODUCERS; i++)
            {
                threadProducers[i].Join();
            }

            for (int i = 0; i < NUMBERCONSUMERS; i++)
            {
                threadConsuemrs[i].Join();
            }

            Console.ReadLine();
        }
    }
}
