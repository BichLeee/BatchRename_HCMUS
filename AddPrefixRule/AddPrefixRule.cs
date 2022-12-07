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
    public class AddPrefixRule : Window, IRule
    {

        private Canvas canvas = new Canvas();
        private Label label = new Label();
        private Button addBtn = new Button();
        private Button cancelBtn = new Button();
        private TextBox editTxtBox = new TextBox();

        public string ruleName { get; set; }
        public string ruleDescription { get; set; }
        public List<string> Parameter { get; set; }
        public string Replace { get; set; }
        public List<int> counter { get; set; }

        public bool isEditable()
        {
          return true;
        }
        public AddPrefixRule(string _rulename,string _ruleDescription, List<string> _parameter,
            string _replace, List<int> _counter)
        {
            ruleName = _rulename;
            ruleDescription = _ruleDescription;
            Parameter = _parameter;
            Replace = _replace;
            counter = _counter;
        }
        public AddPrefixRule()
        {
            Parameter = new List<string>();
            Parameter.Add("");
            ruleName = "Add Prefix Rule";
            ruleDescription = "Add " + Parameter[0] + " into prefix filename.";
            counter = new List<int>();
            counter.Add(0);


            this.Title = "Add Prefix Rule";
            this.Width = 420;
            this.Height = 240;
            this.ResizeMode = ResizeMode.NoResize;


            label.Content = "Input characters you want to add as prefix";
            label.Margin = new Thickness(20, 15, 0, 0);
            label.FontSize = 15;

            editTxtBox.Width = 360;
            editTxtBox.Height = 80;
            editTxtBox.TextWrapping = TextWrapping.WrapWithOverflow;
            editTxtBox.Margin = new Thickness(20, 55, 0, 5);
            editTxtBox.Text = Parameter[0];



            addBtn.Content = "Add";
            addBtn.Name = "add";
            addBtn.IsDefault = true;
            addBtn.Width = 170;
            addBtn.Height = 40;
            addBtn.Margin = new Thickness(20, 145, 0, 0);
            addBtn.FontSize = 15;
            addBtn.Click += this.handleAdd;


            cancelBtn.IsCancel = true;
            cancelBtn.Content = "Cancel";
            cancelBtn.Width = 170;
            cancelBtn.Height = 40;
            cancelBtn.Margin = new Thickness(210, 145, 0, 0);
            cancelBtn.FontSize = 15;
            cancelBtn.Click += this.handleCancel;

            canvas.Children.Add(label);
            canvas.Children.Add(editTxtBox);
            canvas.Children.Add(addBtn);
            canvas.Children.Add(cancelBtn);

            this.AddChild(canvas);
          
        }

        public void handleAdd(object sender, RoutedEventArgs e)
        {
            Parameter[0] = editTxtBox.Text.ToString();
            ruleDescription = "Add " + Parameter[0] + " into prefix filename.";
            DialogResult = true;
        }
        public void handleCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public bool? showUI()
        {
             return this.ShowDialog();           
        }
        public void Rename(ObservableCollection<Item> list, bool isFile)
        {

            for (int i =0; i < list.Count; i++)
            {
                var builder = new StringBuilder();
                builder.Append(Parameter[0]);
                builder.Append(list[i].itemName);

                string result = builder.ToString();
                list[i].newItemName = result;
            }

        }
        public IRule Clone()
        {
            AddPrefixRule clone = new AddPrefixRule();
            clone.Parameter = Parameter;
            return clone;
        }

    }
}
