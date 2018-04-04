using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace MPLeaderProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                
                int randNumber = rand.Next(1000);
                int[] numbers = comm.Gather(randNumber, 0);
                Console.WriteLine("rank " + comm.Rank + " element " + randNumber);
                if (comm.Rank == 0)
                {
                    int i = 0;
                    int[] leader = new int[2];  //leader[0] represent rank of the process and leader[1] represent value produced by that process
                    foreach (var item in numbers)
                    {
                        if(leader[1] < item )
                        {
                            leader[0] = i;
                            leader[1] = item;
                        }
                        else if(leader[1] == item && leader[0] < i)
                        {
                            leader[0] = i;
                        }
                        //Console.WriteLine("Proces with rank " + i+ " produce element "+item);
                        i++;
                    }
                    Console.WriteLine("Process with rank " + leader[0] + " produce greatest element with value " + leader[1]);

                }


            };
        }
    }
}
