using Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace BatchRename
{
    public class PascalCase : IRule
    {
        public string ruleName { get; set; }
        public string ruleDescription { get; set; }
        public List<string> Parameter { get; set; }
        public string Replace { get; set; }
        public List<int> counter { get; set; }

        public PascalCase(string _rulename, string _ruleDescription, List<string> _parameter,
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
        public PascalCase()
        {
            ruleName = "PascalCase";
            ruleDescription = "Convert filename to PascalCase";
        }

        public IRule Clone()
        {
            PascalCase clone = new PascalCase();
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
                    fileName = fileName.Trim();
                    String[] par = fileName.Split(' ');
                    fileName = "";
                    for (int j = 0; j < par.Length; j++)
                    {
                        if (String.Equals(par[j], ""))
                        {
                        }
                        else
                        {
                            if (par[j][0] <= 'z' && par[j][0] >= 'a')
                            {
                                fileName = fileName + Convert.ToChar(par[j][0] - ('a' - 'A'));
                                fileName = fileName + par[j].Remove(0, 1);
                            }
                            else
                            {
                                fileName = fileName + par[j];
                            }
                        }
                    }
                    result = fileName + '.' + extension;
                }
                else
                {
                    str = str.ToLower();
                    str = str.Trim();
                    String[] par = str.Split(' ');
                    str = "";
                    for (int j = 0; j < par.Length; j++)
                    {
                        if (String.Equals(par[j], ""))
                        {

                        }
                        else
                        {
                            if (par[j][0] <= 'z' && par[j][0] >= 'a')
                            {
                                str = str + Convert.ToChar(par[j][0] - ('a' - 'A'));
                                str = str + par[j].Remove(0, 1);
                            }
                            else
                            {
                                str = str + par[j];
                            }
                        }
                    }

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
