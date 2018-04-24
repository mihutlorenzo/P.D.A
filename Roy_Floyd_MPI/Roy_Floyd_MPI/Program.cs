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
                                { 0,   2,  INF, 10,  INF },
                                { 2,   0,    3,INF,  INF },
                                { INF, 3, 0,    1,    8 },
                                { 10, INF, 1, 0 ,INF},
                                { INF, INF, 8, INF ,0}
                            };
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                for(int k = 0; k< 5;k++)
                {
                    for(int j=0;j<5;j++)
                    {
                        if (graph[comm.Rank, k] != INF && graph[k, j] != INF)
                        {
                            if (graph[comm.Rank, k] + graph[k, j] < graph[comm.Rank, j])
                            {
                                graph[comm.Rank, j] = graph[comm.Rank, k] + graph[k, j];
                            }
                        }
                    }
                    comm.Barrier();
                }

            }
            Print(ref graph, 5);
            Console.ReadLine();
        }

        public static void Print(ref int[,] graph,int verticesCount)
        {
            Console.WriteLine("Shortest distances between every pair of vertices:");
            for (int i=0; i<verticesCount;i++)
            {
                for(int j=0;j<verticesCount;j++)
                {
                    if (graph[i, j] == INF)
                        Console.Write("INF".PadLeft(7));
                    else
                        Console.Write(graph[i, j].ToString().PadLeft(7));
                }
                Console.WriteLine();
            }
        }

        public static void FloydWarshall(ref int[,] graph,int verticesCount)
        {
           for(int k =0; k<verticesCount;k++)
            {
                for(int i=0;i<verticesCount;i++)
                {
                    for(int j=0; j< verticesCount;j++)
                    {
                        if (graph[i, k] != INF && graph[k, j] != INF)
                        {
                            if (graph[i , k] + graph[k, j] < graph[i, j])
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
