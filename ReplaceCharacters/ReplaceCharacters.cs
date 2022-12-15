using Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BatchRename
{
    public class ReplaceCharacters : Window, IRule
    {
        private Canvas canvas = new Canvas();
        private Label label1 = new Label();
        private Label label2 = new Label();
        private Button addBtn = new Button();
        private Button cancelBtn = new Button();
        private TextBox editTxtBox = new TextBox();
        private TextBox replaceTxtBox = new TextBox();

        public string ruleName { get; set; }
        public string ruleDescription { get; set; }
        public List<string> Parameter { get; set; }
        public string Replace { get; set; }
        public List<int> counter { get; set; }
        public bool isEditable()
        {
            return true;
        }

        public ReplaceCharacters(string _rulename, string _ruleDescription, List<string> _parameter,
   string _replace, List<int> _counter)
        {
            ruleName = _rulename;
            ruleDescription = _ruleDescription;
            Parameter = _parameter;
            Replace = _replace;
            counter = _counter;
        }

        public ReplaceCharacters()
        {
            Parameter = new List<string>();
            Parameter.Add("");
            Parameter.Add("");
            ruleName = "Replace characters";
            ruleDescription = "Replace " + Parameter[0] + " characters into " + Parameter[1];
            counter = new List<int>();
            counter.Add(0);

            this.Title = "Replace characters";
            this.Width = 420;
            this.Height = 360;
            this.ResizeMode = ResizeMode.NoResize;


            label1.Content = "Input characters you want to be replaced ex: ab - +";
            label1.Margin = new Thickness(20, 15, 0, 0);
            label1.FontSize = 15;

            editTxtBox.Width = 360;
            editTxtBox.Height = 80;
            editTxtBox.TextWrapping = TextWrapping.WrapWithOverflow;
            editTxtBox.Margin = new Thickness(20, 55, 0, 5);
            editTxtBox.Text = Parameter[0];


            label2.Content = "Input characters you want to replaced ";
            label2.Margin = new Thickness(20, 145, 0, 0);
            label2.FontSize = 15;

            replaceTxtBox.Width = 360;
            replaceTxtBox.Height = 80;
            replaceTxtBox.TextWrapping = TextWrapping.WrapWithOverflow;
            replaceTxtBox.Margin = new Thickness(20, 180, 0, 5);
            replaceTxtBox.Text = Parameter[1];

            addBtn.Content = "Add";
            addBtn.Name = "add";
            addBtn.IsDefault = true;
            addBtn.Width = 170;
            addBtn.Height = 40;
            addBtn.Margin = new Thickness(20, 270, 0, 0);
            addBtn.FontSize = 15;
            addBtn.Click += this.handleReplace;


            cancelBtn.IsCancel = true;
            cancelBtn.Content = "Cancel";
            cancelBtn.Width = 170;
            cancelBtn.Height = 40;
            cancelBtn.Margin = new Thickness(210, 270, 0, 0);
            cancelBtn.FontSize = 15;
            cancelBtn.Click += this.handleCancel;

            canvas.Children.Add(label1);
            canvas.Children.Add(editTxtBox);
            canvas.Children.Add(label2);
            canvas.Children.Add(replaceTxtBox);
            canvas.Children.Add(addBtn);
            canvas.Children.Add(cancelBtn);

            this.AddChild(canvas);

        }

        public IRule Clone()
        {
            ReplaceCharacters clone = new ReplaceCharacters();
            clone.Parameter = Parameter;
            return clone;
        }

        public void Rename(ObservableCollection<Item> list, bool isFile)
        {
            string result;
            String[] par = Parameter[0].Split(' ');
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
                    for (int j = 0; j < par.Length; j++)
                    {
                        fileName = fileName.Replace(par[j], Parameter[1]);
                    }
                    
                    result = fileName + '.' + extension;
                }
                else
                {
                    for (int j = 0; j < par.Length; j++)
                    {
                        str = str.Replace(par[j], Parameter[1]);
                    }

                    result = str;
                }

                list[i].newItemName = result;
            }
        }

        public bool? showUI()
        {
            return this.ShowDialog();
        }

        public void handleReplace(object sender, RoutedEventArgs e)
        {
            Parameter[0] = editTxtBox.Text.ToString();
            Parameter[1] = replaceTxtBox.Text.ToString();

            ruleDescription = "Replace " + Parameter[0] + " characters into " + Parameter[1];
            DialogResult = true;
        }
        public void handleCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }


    }
}
