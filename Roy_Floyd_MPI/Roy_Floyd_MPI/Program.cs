using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace Roy_Floyd_MPI
{
    class Program
    {
        private const int INF = 99999;

        static void Main(string[] args)
        {
            int[,] graph = {
                                { 0  , 2  , INF, 10 , INF },
                                { 2  , 0  , 3  , INF, INF },
                                { INF, 3  , 0  , 1  , 8   },
                                { 10 , INF, 1  , 0  , INF },
                                { INF, INF, 8  , INF, 0   }
                            };


            //FloydWarshall(ref graph, 5);
            using (new MPI.Environment(ref args))
            {

                Intracommunicator comm = Communicator.world;
                int[] columnData = new int[comm.Size];

                for (int k = 0; k < 5; k++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (graph[j, k] != INF && graph[k, comm.Rank] != INF)
                        {
                            if (graph[j, k] + graph[k, comm.Rank] < graph[j ,comm.Rank])
                            {
                                graph[j, comm.Rank] = graph[j, k] + graph[k, comm.Rank];
                                
                                comm.Broadcast(ref graph[j, comm.Rank], comm.Rank);
                            }
                        }
                        comm.Barrier();
                    }
                    



                    //for (int i = 0; i < 5; i++)
                    //{
                    //    for (int index = 0; index < 5; index++)
                    //    {
                    //        columnData[index] = graph[i,index];
                    //        Console.Write("line number {1} data {0}", graph[i, index],i);
                    //    }
                    //    Console.WriteLine();
                    //    int[] results = comm.Alltoall(columnData);
                    //    for(int index =0; index<5;index++)
                    //    {
                    //        Console.Write("line number {1} data {0}", results[index],i);
                    //        graph[i, index] = results[index];
                    //    }

                    //}



                    //for (int i = 0; i < 5; i++)
                    //{
                    //    int[] data = comm.Gather(graph[comm.Rank, i], 0);
                    //    if (comm.Rank == 0)
                    //    {
                    //        for (int z = 0; z < 5; z++)
                    //        {
                    //            graph[z, i] = data[z];

                    //        }
                    //    }
                    //}

                    //if (comm.Rank == 0)
                    //{
                    //    for (int z = 0; z < 5; z++)
                    //        for (int y = 0; y < 5; y++)
                    //        {
                    //            comm.Broadcast(ref graph[z, y], 0);
                    //        }
                    //    Console.WriteLine("Pasul {0}", k);
                    //    Print(ref graph, 5);
                    //}
                }
                if (comm.Rank == 0)
                {
                   Print(ref graph, 5);
                }

                

            }


        }

        public static void Print(ref int[,] graph, int verticesCount)
        {
            Console.WriteLine("Shortest distances between every pair of vertices:");
            for (int i = 0; i < verticesCount; i++)
            {
                for (int j = 0; j < verticesCount; j++)
                {
                    if (graph[i, j] == INF)
                        Console.Write("INF".PadLeft(7));
                    else
                        Console.Write(graph[i, j].ToString().PadLeft(7));
                }
                Console.WriteLine();
            }
        }

        public static void FloydWarshall(ref int[,] graph, int verticesCount)
        {
            for (int k = 0; k < verticesCount; k++)
            {
                for (int i = 0; i < verticesCount; i++)
                {
                    for (int j = 0; j < verticesCount; j++)
                    {
                        if (graph[i, k] != INF && graph[k, j] != INF)
                        {
                            if (graph[i, k] + graph[k, j] < graph[i, j])
                            {
                                graph[i, j] = graph[i, k] + graph[k, j];
                            }
                        }
                    }
                }
            }
            Print(ref graph, verticesCount);

        }
    }
}
