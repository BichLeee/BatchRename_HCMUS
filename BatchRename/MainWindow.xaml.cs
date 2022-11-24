using Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        class File
        {
            public string fileName { get; set; }
            public string newFileName { get; set; }
            public string path { get; set; }
            public string error { get; set; }

            public File() { }

        }

        class Rule
        {
            public string ruleName { get; set; }
            public string ruleDescription { get; set; }
        }

        ObservableCollection<File> _listFile= new ObservableCollection<File>();
        ObservableCollection<Rule> _listRule = new ObservableCollection<Rule>();

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _listFile = new ObservableCollection<File> { 
                new File() { fileName = "abc", newFileName="104abc", path="C:/xxx...", error="No error"},
                new File() { fileName = "abc", newFileName="104abc", path="C:/xxx...", error="No error"},
                new File() { fileName = "abc", newFileName="104abc", path="C:/xxx...", error="No error"},
                new File() { fileName = "abc", newFileName="104abc", path="C:/xxx...", error="No error"},
                new File() { fileName = "abc", newFileName="104abc", path="C:/xxx...", error="No error"},
                new File() { fileName = "abc", newFileName="104abc", path="C:/xxx...", error="No error"}

            };

            _listRule = new ObservableCollection<Rule> {
                new Rule() { ruleName = "Rule 1", ruleDescription="Rule 1"},
                new Rule() { ruleName = "Rule 2", ruleDescription="Rule 2"}
            };

            filesListBox.ItemsSource = _listFile;
            rulesListBox.ItemsSource = _listRule;
        }


    }
}
