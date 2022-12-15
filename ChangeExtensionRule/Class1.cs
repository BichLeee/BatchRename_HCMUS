using System;
using System.Windows.Controls;
using System.Windows;
using Interface;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;

namespace BatchRename
{
    public class ChangeExtensionRule : Window, IRule
    {
        public string ruleName { get; set; }

        public string ruleDescription { get; set; }
        public List<string> Parameter { get; set; }
        public string Replace { get; set; }
        public List<int> counter { get; set; }

        public ChangeExtensionRule(string _rulename, string _ruleDescription, List<string> _parameter,
            string _replace, List<int> _counter)
        {
            ruleName = _rulename;
            ruleDescription = _ruleDescription;
            Parameter = _parameter;
            Replace = _replace;
            counter = _counter;
        }

        public ChangeExtensionRule()
        {
            Parameter = new List<string>();
            Parameter.Add("");
            ruleName = "Change Extension Rule";
            ruleDescription = "Add " + Parameter[0] + " into prefix filename.";
            counter = new List<int>();
            counter.Add(0);
        }

        public IRule Clone()
        {
            ChangeExtensionRule clone = new ChangeExtensionRule();
            clone.Parameter = Parameter;
            return clone;
        }

        public bool isEditable()
        {
            return true;
        }

        public void Rename(ObservableCollection<Item> list, bool isFile)
        {
            throw new NotImplementedException();
        }

        public bool? showUI()
        {
            throw new NotImplementedException();
        }
    }
}
