using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IRule
    {
        public string ruleName { get;  }
        public string ruleDescription { get; set; }
        public List<string> Parameter { get; set; }
        public string Replace { get; set; }
        public List<int> counter { get; set; }
        public bool isEditable();
        public void Rename(ObservableCollection<Item> list,bool isFile);
        public bool? showUI();

        public IRule Clone();
    }
    public class RuleFormat
    {
        public string ruleName { get; set; }
        public string ruleDescription { get; set; }
        public List<string> Parameter { get; set; }
        public string Replace { get; set; }
        public List<int> counter { get; set; }
    }
    public class ProJect
    {
        public ObservableCollection<Item> listFiles { get; set; }
        public ObservableCollection<Item> listForder { get; set; }
        public List<RuleFormat> rules { get; set; }
    }
}
