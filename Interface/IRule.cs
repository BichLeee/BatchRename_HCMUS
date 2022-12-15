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
        public string ruleName { get;  } // tên
        public string ruleDescription { get; set; } // mô tả 
        public List<string> Parameter { get; set; } // tập hợp các biến cần đưa vào để thực hiện rule
        public string Replace { get; set; } // dùng cho rule thay thế 
        public List<int> counter { get; set; } // dùng cho rule counter
        public bool isEditable(); // có thể edit hay k
        public void Rename(ObservableCollection<Item> list,bool isFile); // thực hiện rename, isFile là coi là File 
                                                                        // hay folder, rename phân ra 2 trường hợp
        public bool? showUI(); // show giao diện thêm 

        public IRule Clone(); // copy rule
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
