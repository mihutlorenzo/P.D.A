using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace MPISumArray
{
    class Program
    {
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                if (comm.Rank == 0)
                {
                    Console.WriteLine("Give maximum number of elements from vector");
                    int maxElements = Convert.ToInt16(Console.ReadLine());
                    int[] vector = new int[maxElements];
                    for (int i = 0; i < maxElements; i++)
                    {
                        vector[i] = i;
                    }
                    int step = maxElements / comm.Size;
                    for (int i = 1; i < comm.Size; i++)
                    {
                        comm.Send(maxElements, i, 1);
                        int[] subvector = new int[step];

                        int initialValueForSubvector = (i - 1) * step;
                        //Console.WriteLine("Initial value " + initialValueForSubvector);
                        for (int j = 0; j < step; j++)
                        {
                            subvector[j] = vector[initialValueForSubvector + j];
                        }
                        comm.Send(subvector, i, 0);
                    }
                    int totalSum = 0;
                    for (int i = 1; i < comm.Size; i++)
                    {
                        totalSum += comm.Receive<int>(i, 2);
                    }
                    for (int i = (comm.Size - 1) * step; i < maxElements; i++)
                    {
                        totalSum += vector[i];
                        
                    }
                    Console.WriteLine("Total sum is " + totalSum);

                }
                else
                {
                    int vectorMax = comm.Receive<int>(0, 1);
                    int[] subvector = new int[vectorMax];
                    comm.Receive(0, 0, ref subvector);
                    int partialSum = AddElements(subvector, vectorMax);
                    comm.Send(partialSum, 0, 2);


                }

            }
        }
        public static int AddElements(int[] vector, int max)
        {
            int sum = 0;
            for (int i = 0; i < max; i++)
            {
                sum += vector[i];
            }
            return sum;
        }
    }
}
