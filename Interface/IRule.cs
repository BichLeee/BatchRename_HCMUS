using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IRule
    {
        public string ruleName { get; }
        public string ruleDescription { get;  }
        public bool isEditable();
        public void Rename(ObservableCollection<Item> list);
        public bool? showUI();
    }
}
