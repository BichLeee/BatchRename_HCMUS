using Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BatchRename
{
    public class LowercaseAndRemoveSpace : IRule
    {
        public string ruleName { get; set; }
        public string ruleDescription { get; set; }
        public List<string> Parameter { get; set; }
        public string Replace { get; set; }
        public List<int> counter { get; set; }

        public LowercaseAndRemoveSpace(string _rulename, string _ruleDescription, List<string> _parameter,
string _replace, List<int> _counter)
        {
            ruleName = _rulename;
            ruleDescription = _ruleDescription;
            Parameter = _parameter;
            Replace = _replace;
            counter = _counter;
        }
        public bool isEditable()
        {
            return false;
        }
        public LowercaseAndRemoveSpace()
        {
            ruleName = "Lower case and remove space";
            ruleDescription = "change file name to lower and remove all spaces";
        }

        public IRule Clone()
        {
            LowercaseAndRemoveSpace clone = new LowercaseAndRemoveSpace();
            clone.Parameter = Parameter;
            return clone;
        }


        public void Rename(ObservableCollection<Item> list, bool isFile)
        {
            string result;
            for (int i = 0; i < list.Count; i++)
            {
                string str = list[i].itemName;
                if (isFile)
                {
                    string[] strings = str.Split('.');
                    string fileName = "", extension = strings[^1];
                    if (strings.Length == 1)
                    {
                        fileName = str;
                        extension = "";
                    }
                    else
                    {
                        foreach (string s in strings)
                        {

                            if (Array.IndexOf(strings, s) == strings.Length - 1)
                            {
                                fileName = fileName.Remove(fileName.Length - 1);
                                break;
                            }
                            fileName = s + '.';
                        }
                    }
                    fileName = fileName.ToLower();
                    String[] par = fileName.Split(' ');
                    fileName = "";
                    for (int j = 0; j < par.Length; j++)
                    {
                        fileName = fileName + par[j];
                    }
                    result = fileName + '.' + extension;
                }
                else
                {
                    str = str.ToLower();
                    String[] par = str.Split(' ');
                    str = "";
                    for (int j = 0; j < par.Length; j++)
                    {
                        str = str + par[j];
                    }
                    result = str + '.' + str;

                    result = str;
                }

                list[i].newItemName = result;
            }
        }

        public bool? showUI()
        {
            return false;
        }
    }
}
