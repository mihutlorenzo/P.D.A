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
            //int[,] graph = {
            //                    { 0  , 2  , INF, 10 , INF },
            //                    { 2  , 0  , 3  , INF, INF },
            //                    { INF, 3  , 0  , 1  , 8   },
            //                    { 10 , INF, 1  , 0  , INF },
            //                    { INF, INF, 8  , INF, 0   }
            //                };


            //FloydWarshall(ref graph, 5);
            using (new MPI.Environment(ref args))
            {

                Intracommunicator comm = Communicator.world;
                int[,] graph = new int[5,5];

                if (comm.Rank == 0)
                {
                    graph[0, 0] = 0;
                    graph[0, 1] = 2;
                    graph[0, 2] = INF;
                    graph[0, 3] = 10;
                    graph[0, 4] = INF;
                    graph[1, 0] = 2;
                    graph[1, 1] = 0;
                    graph[1, 2] = 3;
                    graph[1, 3] = INF;
                    graph[1, 4] = INF;
                    graph[2, 0] = INF;
                    graph[2, 1] = 3;
                    graph[2, 2] = 0;
                    graph[2, 3] = 1;
                    graph[2, 4] = 8;
                    graph[3, 0] = 10;
                    graph[3, 1] = INF;
                    graph[3, 2] = 1;
                    graph[3, 3] = 0;
                    graph[3, 4] = INF;
                    graph[4, 0] = INF;
                    graph[4, 1] = INF;
                    graph[4, 2] = 8;
                    graph[4, 3] = INF;
                    graph[4, 4] = 0;
                }

                //for (int k = 0; k < 5; k++)
                //    {
                //    for (int j = 0; j < 5; j++)
                //    {
                //        if (graph[comm.Rank, k] != INF && graph[k, j] != INF)
                //        {
                //            if (graph[comm.Rank, k] + graph[k, j] < graph[comm.Rank, j])
                //            {
                //                graph[comm.Rank, j] = graph[comm.Rank, k] + graph[k, j];
                //            }
                //        }
                //    }
                //    for (int i = 0; i < 5; i++)
                //    {
                //        comm.Broadcast(ref graph[comm.Rank, i], comm.Rank);
                //    }
                //    comm.Barrier();
                //    if (comm.Rank == 0)
                //    {
                //        Console.WriteLine("Pasul {0}", k);
                //        Print(ref graph, 5);
                //    }
                //}
                //if (comm.Rank == 0)
                //{
                //    Print(ref graph, 5);
                //}

                for (int i = 0; i < 5; i++)
                {
                    comm.Broadcast(ref graph[comm.Rank, i], comm.Rank);
                }

                Console.WriteLine("Procesul cu rankul {0}", comm.Rank);
                Print(ref graph, 5);

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
