using System.Collections.Generic;
using System.Linq;

namespace IlegraChallange
{
    public class Sale
    {
        public string Id { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public string Salesman { get; set; }
        public decimal Total => Items.Sum(i => i.Price);
    }
}