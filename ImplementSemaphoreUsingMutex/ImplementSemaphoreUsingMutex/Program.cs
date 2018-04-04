using ProducerConsumerUsingSemaphores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImplementSemaphoreUsingMutex
{
    class Program
    {
        const int BUFFERSIZE = 5;
        static PCBuffer buffer = new PCBuffer(BUFFERSIZE);
        const int NUMBEROFPRODUCERS = 2;
        const int NUMBEROFCONSUMERS = 2;

        static void Main(string[] args)
        {

            Thread[] producers = new Thread[NUMBEROFPRODUCERS];
            Thread[] consumers = new Thread[NUMBEROFCONSUMERS];

            for (int i = 0; i < NUMBEROFPRODUCERS; i++)
            {
                producers[i] = new Thread(new Producer(buffer, i).Produce);
                producers[i].Start();
            }

            for (int i = 0; i < NUMBEROFCONSUMERS; i++)
            {
                consumers[i] = new Thread(new Consumer(buffer, i).Consume);
                consumers[i].Start();
            }

            for (int i = 0; i < NUMBEROFPRODUCERS; i++)
            {

                producers[i].Join();
            }

            for (int i = 0; i < NUMBEROFCONSUMERS; i++)
            {

                consumers[i].Join();
            }
            Console.ReadLine();

        }
    }
}
