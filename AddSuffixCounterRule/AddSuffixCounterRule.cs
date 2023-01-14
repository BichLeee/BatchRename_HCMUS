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

namespace BatchRename
{
    public class AddSuffixCounterRule : Window, IRule
    {
        private Canvas canvas = new Canvas();
        private Label labelStartValue = new Label();
        private Label labelSteps = new Label();
        private Label labelNumDigits = new Label();

        private TextBox edtStartValue = new TextBox();
        private TextBox edtSteps = new TextBox();
        private TextBox edtNumDigits = new TextBox();


        private Button addBtn = new Button();
        private Button cancelBtn = new Button();

        public string ruleName { get; set; }
        public string ruleDescription { get; set; }
        public List<string> Parameter { get; set; }
        public string Replace { get; set; }
        public List<int> counter { get; set; }

        public AddSuffixCounterRule(string _rulename, string _ruleDescription, List<string> _parameter,
    string _replace, List<int> _counter)
        {
            ruleName = _rulename;
            ruleDescription = _ruleDescription;
            Parameter = _parameter;
            Replace = _replace;
            counter = _counter;
        }

        public AddSuffixCounterRule()
        {
            Parameter = new List<string>();
            Parameter.Add("0");
            Parameter.Add("1");
            Parameter.Add("1");
            ruleName = "Add counter to the end";
            ruleDescription = $"Add counter to the end.\n" +
                                $"Start: {Parameter[0]}\n" +
                                $"Steps: {Parameter[1]}\n" +
                                $"Number of Digits: {Parameter[2]}\n"
                                ;
            counter = new List<int>();
            counter.Add(0);

            this.Title = "Add Counter To End Rule";
            this.Width = 420;
            this.Height = 300;
            this.ResizeMode = ResizeMode.NoResize;


            labelStartValue.Content = "Start Value: ";
            labelStartValue.Margin = new Thickness(20, 15, 0, 0);
            labelStartValue.FontSize = 15;

            labelSteps.Content = "Steps: ";
            labelSteps.Margin = new Thickness(20, 60, 0, 0);
            labelSteps.FontSize = 15;

            labelNumDigits.Content = "Number of digits: ";
            labelNumDigits.Margin = new Thickness(20, 105, 0, 0);
            labelNumDigits.FontSize = 15;

            edtStartValue.Width = 120;
            edtStartValue.Height = 40;
            edtStartValue.TextWrapping = TextWrapping.WrapWithOverflow;
            edtStartValue.Margin = new Thickness(160, 20, 0, 0);
            edtStartValue.Text = Parameter[0];
            edtStartValue.PreviewTextInput += this.NumberValidationTextBox;
            edtStartValue.VerticalContentAlignment = VerticalAlignment.Center;

            edtSteps.Width = 120;
            edtSteps.Height = 40;
            edtSteps.TextWrapping = TextWrapping.WrapWithOverflow;
            edtSteps.Margin = new Thickness(160, 65, 0, 0);
            edtSteps.Text = Parameter[1].ToString();
            edtSteps.PreviewTextInput += this.NumberValidationTextBox;
            edtSteps.VerticalContentAlignment = VerticalAlignment.Center;

            edtNumDigits.Width = 120;
            edtNumDigits.Height = 40;
            edtNumDigits.TextWrapping = TextWrapping.WrapWithOverflow;
            edtNumDigits.Margin = new Thickness(160, 110, 0, 0);
            edtNumDigits.Text = Parameter[2].ToString();
            edtNumDigits.PreviewTextInput += this.NumberValidationTextBox;
            edtNumDigits.VerticalContentAlignment = VerticalAlignment.Center;


            addBtn.Content = "Add";
            addBtn.Name = "add";
            addBtn.IsDefault = true;
            addBtn.Width = 170;
            addBtn.Height = 40;
            addBtn.Margin = new Thickness(20, 160, 0, 0);
            addBtn.FontSize = 15;
            addBtn.Click += this.handleAdd;


            cancelBtn.IsCancel = true;
            cancelBtn.Content = "Cancel";
            cancelBtn.Width = 170;
            cancelBtn.Height = 40;
            cancelBtn.Margin = new Thickness(210, 160, 0, 0);
            cancelBtn.FontSize = 15;
            cancelBtn.Click += this.handleCancel;

            canvas.Children.Add(labelStartValue);
            canvas.Children.Add(labelSteps);
            canvas.Children.Add(labelNumDigits);

            canvas.Children.Add(edtStartValue);
            canvas.Children.Add(edtSteps);
            canvas.Children.Add(edtNumDigits);

            canvas.Children.Add(addBtn);
            canvas.Children.Add(cancelBtn);

            this.AddChild(canvas);
        }

        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void handleAdd(object sender, RoutedEventArgs e)
        {
            Parameter[0] = edtStartValue.Text.Replace(" ", "").ToString();
            Parameter[1] = edtSteps.Text.Replace(" ", "").ToString();
            Parameter[2] = edtNumDigits.Text.Replace(" ", "").ToString();


            ruleDescription = $"Add counter to the end.\n" +
                                $"Start: {Parameter[0]}\n" +
                                $"Steps: {Parameter[1]}\n" +
                                $"Number of Digits: {Parameter[2]}\n";
            DialogResult = true;
        }
        public void handleCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public IRule Clone()
        {
            AddSuffixCounterRule clone = new AddSuffixCounterRule();
            clone.Parameter = Parameter;
            return clone;
        }

        public bool isEditable()
        {
            return true;
        }

        public void Rename(ObservableCollection<Item> list, bool isFile)
        {
            int count = Int32.Parse(Parameter[0]);
            for (int i = 0; i < list.Count; i++)
            {
                var builder = new StringBuilder();
                String oldName = list[i].itemName;
                int index = oldName.LastIndexOf('.');

                if (index < 0) index = oldName.Length;
                builder.Append(oldName.Substring(0, index));
                builder.Append(count.ToString("D" + Parameter[2]));
                builder.Append(oldName.Substring(index));

                string result = builder.ToString();
                list[i].newItemName = result;

                count += Int32.Parse(Parameter[1]);
            }
        }

        public bool? showUI()
        {
            return this.ShowDialog();
        }
    }
}
