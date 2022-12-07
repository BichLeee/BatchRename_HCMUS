using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Item
    {
        public string itemName { get; set; }
        public string newItemName { get; set; }
        public string path { get; set; }
        public string error { get; set; }
        public Item Clone()
        {
            Item clone = new Item();
            clone.itemName = itemName;
            clone.newItemName = newItemName;   
            clone.path = path;
            clone.error = error;
            return clone;
        }

    }

}
