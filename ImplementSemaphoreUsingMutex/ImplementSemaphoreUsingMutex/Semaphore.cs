using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImplementSemaphoreUsingMutex
{
    class Semaphore
    {
        private volatile int permits;
        private Mutex permitsMutex;
        private Mutex waitForPermit;

        public Semaphore(int nrOfPermits = 1)
        {
            permits = nrOfPermits - 1;
            permitsMutex = new Mutex();
            waitForPermit = new Mutex();
        }


        public void Aquire()
        {
            permitsMutex.WaitOne();
            while(permits <= 0)
            {
                permitsMutex.ReleaseMutex();
                Thread.Sleep(100);
                permitsMutex.WaitOne();
            }
            permits--;
            permitsMutex.ReleaseMutex();
        }

        public void Release()
        {
            permitsMutex.WaitOne();
            permits++;
            permitsMutex.ReleaseMutex();
        }
    }

    
}
