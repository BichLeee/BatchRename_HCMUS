using Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using MessageBox = System.Windows.Forms.MessageBox;
using Path = System.IO.Path;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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

      
        class Item
        {
            public string itemName { get; set; }
            public string newItemName { get; set; }
            public string path { get; set; }
            public string error { get; set; }

            public Item() { }

        }

        class Rule
        {
            public string ruleName { get; set; }
            public string ruleDescription { get; set; }
        }

        ObservableCollection<Item> _listFile= new ObservableCollection<Item>();
        ObservableCollection<Item> _listFolder = new ObservableCollection<Item>();

        ObservableCollection<Rule> _listRule = new ObservableCollection<Rule>();

        BindingList<string> itemType;

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _listRule = new ObservableCollection<Rule> {
                new Rule() { ruleName = "Rule 1", ruleDescription="Rule 1"},
                new Rule() { ruleName = "Rule 2", ruleDescription="Rule 2"}
            };

            
            rulesListBox.ItemsSource = _listRule;

            BindingList<string> itemType = new BindingList<string>()
            {
                "File","Folder"
            };

            ComboType.ItemsSource = itemType;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboType.SelectedItem == null)
                return;
            if(ComboType.SelectedItem.ToString() == "File")
                filesListBox.ItemsSource = _listFile;
            else if(ComboType.SelectedItem.ToString() == "Folder")
                filesListBox.ItemsSource = _listFolder;
        }

        private void listRules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Handle_Add(object sender, RoutedEventArgs e)
        {
            if (ComboType.SelectedItem == null)
            {
                MessageBox.Show("Please select type (files or folders)", "Error");
                return;
            }
            if (ComboType.SelectedItem.ToString() == "File")
            {
              
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.ShowDialog();


                string[] files = openFileDialog.FileNames;

                foreach(var file in files)
                {
                    string nameFile = Path.GetFileName(file);
                    string pathFile = Path.GetDirectoryName(file);
                    bool isExisted = false;

                    foreach (var f in _listFile)
                    {
                        if(nameFile == f.itemName && pathFile == f.path)
                        {
                            isExisted = true; break;
                        }
                    }
                    if (!isExisted)
                    {
                        _listFile.Add(new Item()
                        {
                            itemName = Path.GetFileName(file),
                            newItemName = "",
                            path = pathFile,
                            error = ""
                        });
                    }
                   

                }
            }else if(ComboType.SelectedItem.ToString() == "Folder")
            {
                var Folderdialog = new FolderBrowserDialog();
                var result = Folderdialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(Folderdialog.SelectedPath))
                {
                    string[] folders = Directory.GetDirectories(Folderdialog.SelectedPath);

                    foreach(var folder in folders)
                    {
                        string nameFolder= Path.GetFileName(folder);
                        string pathFolder = Path.GetDirectoryName(folder);

                        bool isExisted = false;

                        foreach (var f in _listFolder)
                        {
                            if (nameFolder == f.itemName && pathFolder == f.path)
                            {
                                isExisted = true; break;
                            }
                        }
                        if (!isExisted)
                        {
                            _listFolder.Add(new Item()
                            {
                                itemName = nameFolder,
                                newItemName = "",
                                path = pathFolder,
                                error = ""
                            });
                        }
                        MessageBox.Show("Files found: " + pathFolder, "Message");
                    }

                   
                }
            }
        }
    }
}
