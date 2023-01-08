using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPlan
{
    ///
    /// Модифицированный алгоритм Дейкстры
    /// 
    public class DModif
    {
        private const double X_MAX = double.MinValue;
        private int MaximumDistance(double[] distance, bool[] shortestPathTreeSet, int verticesCount)
        {
            double min = X_MAX;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (shortestPathTreeSet[v] == false && distance[v] >= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        public double[] Dijkstra(double[,] graph, int source, int verticesCount, int[] prev)
        {
            double[] distance = new double[verticesCount];
            bool[] shortestPathTreeSet = new bool[verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                distance[i] = X_MAX;
                shortestPathTreeSet[i] = false;
                prev[i] = source;
            }
            distance[source] = double.MaxValue; // 0;

            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MaximumDistance(distance, shortestPathTreeSet, verticesCount);
                shortestPathTreeSet[u] = true;

                for (int v = 0; v < verticesCount; ++v)
                    if (!shortestPathTreeSet[v]
                        && Convert.ToBoolean((graph[u, v]) > 0)
                        && (distance[u] != X_MAX)
                        && (Math.Min( distance[u],  graph[u, v]) > distance[v]))
                    {
                        distance[v] = Math.Min(distance[u], graph[u, v]);
                        prev[v] = u;
                    }
            }
            return distance;
        }
    }
}
