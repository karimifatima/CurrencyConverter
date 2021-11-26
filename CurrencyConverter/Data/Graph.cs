using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Data
{
    public class Graph
    {
        Dictionary<int, Node> NodesIdIndex = new Dictionary<int, Node>();
        Dictionary<string, Node> NodesTitleIndex = new Dictionary<string, Node>();

        public void AddEdge(string source, string destination, double convRate)
        {
            Node sourceNode = FindOrCreateNode(source);
            Node destNode = FindOrCreateNode(destination);

            sourceNode.AddRelation(destNode, convRate);
            destNode.AddRelation(sourceNode, 1 / convRate);
        }

        public List<Node> FindPath(string start, string end)
        {
            var sourceNode = FindNode(start);
            var destNode = FindNode(end);

            if (sourceNode == null || destNode == null)
            {
                return null;
            }

            var sourceId = sourceNode.Id;
            var destId = destNode.Id;

            var Path = BFS(sourceId, destId);
            if (Path.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("Given source and destination are not connected");
                return null;
            }

            System.Diagnostics.Debug.WriteLine("Shortest path length is: " + Path.Count);

            System.Diagnostics.Debug.WriteLine("Path is ::");

            foreach (var item in Path)
            {
                System.Diagnostics.Debug.WriteLine(item.Title + " ");
            }

            return Path;
        }

        public List<Node> BFS(int start, int target)
        {
            int[] predecessors = new int[NodesIdIndex.Count];
            int[] distances = new int[NodesIdIndex.Count];

            List<int> queue = new List<int>();
            bool[] visited = new bool[NodesIdIndex.Count];

            for (int i = 0; i < NodesIdIndex.Count; i++)
            {
                visited[i] = false;
                distances[i] = int.MaxValue;
                predecessors[i] = -1;
            }

            visited[start] = true;
            distances[start] = 0;
            queue.Add(start);

            // bfs Algorithm
            while (queue.Count != 0)
            {
                int u = queue[0];
                queue.RemoveAt(0);

                foreach (var item in NodesIdIndex[u].Relations)
                {
                    var destId = item.Key;
                    if (visited[destId] == false)
                    {
                        visited[destId] = true;
                        distances[destId] = distances[u] + 1;
                        predecessors[destId] = u;
                        queue.Add(destId);

                        if (destId == target)
                            return CreateNodePath(target, predecessors);
                    }
                }
            }
            return new List<Node>();
        }

        List<Node> CreateNodePath(int targetId, int[] predecessors)
        {
            List<Node> path = new List<Node>();
            int crawl = targetId;
            path.Add(NodesIdIndex[crawl]);

            while (predecessors[crawl] != -1)
            {
                path.Add(NodesIdIndex[predecessors[crawl]]);
                crawl = predecessors[crawl];
            }
            path.Reverse();
            return path;
        }

        Node FindOrCreateNode(string Title)
        {
            Node result = FindNode(Title);
            if (result == null)
            {
                result = new Node { Id = NodesTitleIndex.Count, Title = Title };
                NodesIdIndex.Add(result.Id, result);
                NodesTitleIndex.Add(result.Title, result);
            }
            return result;
        }

        Node FindNode(string Title)
        {
            if (NodesTitleIndex.ContainsKey(Title))
            {
                return NodesTitleIndex[Title];
            }
            else
                return null;
        }

    }
}
