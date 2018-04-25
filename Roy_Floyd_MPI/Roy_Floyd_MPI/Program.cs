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
                

                for (int k = 0; k < comm.Size; k++)
                {
                    for (int i = 0; i < comm.Size; i++)
                    {
                        
                        if (graph[i, k] != INF && graph[k, comm.Rank] != INF)
                        {
                            if (graph[i, k] + graph[k, comm.Rank] < graph[i, comm.Rank])
                            {
                                graph[i, comm.Rank] = graph[i, k] + graph[k, comm.Rank];
                               
                                Console.WriteLine("noua valoare a nodului cu x {0} si y {1} la pasul {3} este {2}", i, comm.Rank, graph[i, comm.Rank], k);
                                
                            }
                        }
                        
                         int[] lineData= comm.Gather(graph[i, comm.Rank], 0);
                        
                        comm.Barrier();
                        if (comm.Rank == 0)
                        {
                            Console.Write("Pe linia cu numarul {0} din pasul {1} avem elementele :", i, k);
                            for (int j = 1; j < comm.Size; j++)
                            {
                                Console.Write("{0}   ",lineData[j]);
                                graph[i, j] = lineData[j];
                                comm.Broadcast(ref graph[i, j], 0);

                            }
                            Console.WriteLine();
                        }
                        comm.Barrier();
                        


                    }
                }
                if(comm.Rank ==0)
                    Print(graph,comm.Size);

            }
        }
    

        public static void Print(int[,] graph, int verticesCount)
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
            Print(graph, verticesCount);

        }
    }

}
