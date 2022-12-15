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
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Linq;

namespace BatchRename
{
    public class RemoveAllSpaceFromBeginAndEnd : IRule
    {
        public string ruleName { get; set; }
        public string ruleDescription { get; set; }
        public List<string> Parameter { get; set; }
        public string Replace { get; set; }
        public List<int> counter { get; set; }

        public RemoveAllSpaceFromBeginAndEnd(string _rulename, string _ruleDescription, List<string> _parameter,
string _replace, List<int> _counter)
        {
            ruleName = _rulename;
            ruleDescription = _ruleDescription;
            Parameter = _parameter;
            Replace = _replace;
            counter = _counter;
        }

        public RemoveAllSpaceFromBeginAndEnd()
        {
            Parameter = new List<string>();
            Parameter.Add("");
            ruleName = "Remove all space begin and end";
            ruleDescription = "Remove all space from the beginning and the ending of the filename";
            counter = new List<int>();
            counter.Add(0);

        }


        public void handleAdd(object sender, RoutedEventArgs e)
        {
            ruleDescription = "Remove all space from the beginning and the ending of the filename";

        }
        public void handleCancel(object sender, RoutedEventArgs e)
        {
        }

        public IRule Clone()
        {
            RemoveAllSpaceFromBeginAndEnd clone = new RemoveAllSpaceFromBeginAndEnd();
            clone.Parameter = Parameter;
            return clone;
        }

        public bool isEditable()
        {
            return false;
        }

        public void Rename(ObservableCollection<Item> list, bool isFile)
        {
            for (int i = 0; i < list.Count; i++)
            {
                String oldName = list[i].itemName;
                string[] parts = oldName.Split('.');

                for (int j = 0; j < parts.Length; j++)
                {
                    parts[j] = parts[j].Trim();
                }
                list[i].newItemName = string.Join(".", parts);
            }
        }

        public bool? showUI()
        {
            return true;
        }
    }
}
