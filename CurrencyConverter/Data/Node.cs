using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Data
{
    public class Node
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Dictionary<int, Relation> Relations { get; set; } = new Dictionary<int, Relation>();

        public void AddRelation(Node destination, double convRate)
        {
            Relations.Add(destination.Id, new Relation { Destination = destination, ConvertionRate = convRate });
        }
    }

    public class Relation
    {
        public Node Destination { get; set; }
        public double ConvertionRate { get; set; }
    }
}
