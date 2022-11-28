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
using Interface;
using System.Reflection;

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

        ObservableCollection<Item> _listFile= new ObservableCollection<Item>();
        ObservableCollection<Item> _listFolder = new ObservableCollection<Item>();

        ObservableCollection<IRule> _listRule = new ObservableCollection<IRule>();

        BindingList<string> itemType;

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _listRule = new ObservableCollection<IRule> {

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
                    }
                                      
                }
            }
        }

        private void Handle_Reset(object sender, RoutedEventArgs e)
        {
            _listFile.Clear();
            _listFolder.Clear();
        }

        private void MoveToTop(object sender, RoutedEventArgs e) { 

            if(ComboType.SelectedItem == null)
            {
                return;
            }
        
            if (ComboType.SelectedItem.ToString() == "File")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToTop(_listFile, index);
            }else if(ComboType.SelectedItem.ToString() == "Folder")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToTop(_listFolder, index);
            }
            else
            {

            }
        }

        private void MoveToBottom(object sender, RoutedEventArgs e)
        {
            if (ComboType.SelectedItem == null)
            {
                return;
            }
            if (ComboType.SelectedItem.ToString() == "File")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToBottom(_listFile, index);
            }
            else if (ComboType.SelectedItem.ToString() == "Folder")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToBottom(_listFolder, index);
            }
        }

        private void MoveToPrev(object sender, RoutedEventArgs e)
        {
            if (ComboType.SelectedItem == null)
            {
                return;
            }
            if (ComboType.SelectedItem.ToString() == "File")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToPrev(_listFile, index);
            }
            else if (ComboType.SelectedItem.ToString() == "Folder")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToPrev(_listFolder, index);
            }
        }

        private void MoveToNext(object sender, RoutedEventArgs e)
        {
            if (ComboType.SelectedItem == null)
            {
                return;
            }
            if (ComboType.SelectedItem.ToString() == "File")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToNext(_listFile, index);
            }
            else if (ComboType.SelectedItem.ToString() == "Folder")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToNext(_listFolder, index);
            }
        }

        private void HandleMoveToTop(ObservableCollection<Item> list , int index)
        {
            if(index != -1)
            {
                Item temp = list[index];
                for(int i=index; i>0; i--)
                {
                    list[i] = list[i - 1];
                }
                list[0] = temp;
            }
        }
        private void HandleMoveToBottom(ObservableCollection<Item> list, int index)
        {
            if (index != -1)
            {
                Item temp = list[index];
                for (int i = index; i < list.Count -1; i++)
                {
                    list[i] = list[i + 1];
                }
                list[list.Count -1] = temp;
            }
        }
        private void HandleMoveToPrev(ObservableCollection<Item> list, int index)
        {
            if (index != -1 && index != 0)
            {
                Item temp = list[index];
                list[index] = list[index - 1];
                list[index - 1] = temp;
            }
        }
        private void HandleMoveToNext(ObservableCollection<Item> list, int index)
        {
            if (index != -1 && index != list.Count -1)
            {
                Item temp = list[index];
                list[index] = list[index + 1];
                list[index + 1] = temp;
            }
        }

        private void Click_Apply(object sender, RoutedEventArgs e)
        {

            List<IRule> rules = new List<IRule>();
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var dlls = new DirectoryInfo(exeFolder).GetFiles("dllRules/*.dll");


            foreach (var dll in dlls)
            {
                var assembly = Assembly.LoadFile(dll.FullName);

                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass)
                    {
                        if (typeof(IRule).IsAssignableFrom(type))
                        {
                            var temp_rule = Activator.CreateInstance(type) as IRule;
                            rules.Add(temp_rule);
                        }
                    }
                }
            }


            var add = rules[0];
            add.showUI();   
            add.Rename(_listFile);

            filesListBox.ItemsSource = null;
            filesListBox.ItemsSource = _listFile;
        }
    }
}
