using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;


namespace MPISearchElementInArray
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
                    Console.WriteLine("Give array length");
                    int length = Convert.ToInt16(Console.ReadLine());
                    int[] numbers = new int[length];
                    for (int i = 0; i < length; i++)
                    {
                        numbers[i] = i;
                        //Console.WriteLine("Number " + i);

                    }
                    Console.WriteLine("Give a number beetween 0 annd " + length);
                    int searchedNumbered = Convert.ToInt16(Console.ReadLine());

                    int step = length / comm.Size;
                    
                    for (int i = 1; i < comm.Size;i++)
                    {
                       
                        comm.Send(step, i, 1);
                        int[] subvector = new int[step];

                        int initialValueForSubvector = (i - 1) * step;
                        //Console.WriteLine("Initial value " + initialValueForSubvector);
                        for (int j = 0; j < step; j++)
                        {
                            subvector[j] = numbers[initialValueForSubvector + j];
                        }
                        comm.Send(subvector, i, 0);
                        comm.Send(searchedNumbered, i, 2);
                    }
                    int k = 0;
                    int limit = length - (comm.Size - 1) * step;
                    int[] vector = new int[limit];
                    for (int i = (comm.Size - 1) * step; i < length; i++)
                    {
                        vector[k] = numbers[i];
                        k++;
                    }
                    SearchElement(vector, (comm.Size - 1) * step, searchedNumbered);


                   


                }
                else
                {
                   
                    int vectorMax = comm.Receive<int>(0, 1);
                    int[] subvector = new int[vectorMax];
                    comm.Receive(0, 0, ref subvector);
                    int searchNumbered = comm.Receive<int>(0, 2);
                    SearchElement(subvector, vectorMax *(comm.Rank -1), searchNumbered);
                    
                }

            }
        }


        public static void SearchElement(int[] vector, int vectorRealPosition,int searchedNumber)
        {
            
            for (int i = 0; i < vector.Length; i++)
            {
                if(vector[i] == searchedNumber)
                {
                    Console.WriteLine("Number with value " + searchedNumber + " was found at position" + (vectorRealPosition+i));
                }
            }
           
        }
    }
}
