﻿using Fluent;
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
using System.Data;
using File = System.IO.File;

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

        ObservableCollection<Item> _listFile = new ObservableCollection<Item>();
        ObservableCollection<Item> _listFolder = new ObservableCollection<Item>();
        ObservableCollection<IRule> _listRule = new ObservableCollection<IRule>();
        ObservableCollection<IRule> _chosenRule = new ObservableCollection<IRule>();

        BindingList<string> itemType = new BindingList<string>()
            {
                "File","Folder"
            };




        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {


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
                            _listRule.Add(temp_rule);
                        }
                    }
                }
            }

            ComboType.ItemsSource = itemType;
            listRules.ItemsSource = _listRule;
            rulesListBox.ItemsSource = _chosenRule;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboType.SelectedItem == null)
                return;
            if (ComboType.SelectedItem.ToString() == "File")
                filesListBox.ItemsSource = _listFile;
            else if (ComboType.SelectedItem.ToString() == "Folder")
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

                foreach (var file in files)
                {
                    
                    string nameFile = Path.GetFileName(file);
                    string pathFile = Path.GetDirectoryName(file);
                    bool isExisted = false;

                    foreach (var f in _listFile)
                    {
                        if (nameFile == f.itemName && pathFile == f.path)
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
            }
            else if (ComboType.SelectedItem.ToString() == "Folder")
            {
                var Folderdialog = new FolderBrowserDialog();
                var result = Folderdialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(Folderdialog.SelectedPath))
                {
                    //string[] fol = Directory.GetDirectories(Folderdialog.SelectedPath) ;
                    //List<string> folders = new List<string>()
                    //{
                    //    Folderdialog.SelectedPath
                    //};
                    //foreach(string f in fol)
                    //    folders.Add(f);

                    //foreach (var folder in folders)
                    //{
                        string folder = Folderdialog.SelectedPath;
                        string nameFolder = Path.GetFileName(folder);
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
                    //}

                }
            }
        }

        private void Handle_Reset(object sender, RoutedEventArgs e)
        {
            _listFile.Clear();
            _listFolder.Clear();
        }

        private void MoveToTop(object sender, RoutedEventArgs e)
        {

            if (ComboType.SelectedItem == null)
            {
                return;
            }

            if (ComboType.SelectedItem.ToString() == "File")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToTop(_listFile, index);
            }
            else if (ComboType.SelectedItem.ToString() == "Folder")
            {
                int index = filesListBox.SelectedIndex;
                HandleMoveToTop(_listFolder, index);
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

        private void HandleMoveToTop(ObservableCollection<Item> list, int index)
        {
            if (index != -1)
            {
                Item temp = list[index];
                for (int i = index; i > 0; i--)
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
                for (int i = index; i < list.Count - 1; i++)
                {
                    list[i] = list[i + 1];
                }
                list[list.Count - 1] = temp;
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
            if (index != -1 && index != list.Count - 1)
            {
                Item temp = list[index];
                list[index] = list[index + 1];
                list[index + 1] = temp;
            }
        }



        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            if (listRules.SelectedItem == null)
                return;

            int index = listRules.SelectedIndex;

            var rule = _listRule[index].Clone();
            bool choseAdd = true;
            if (rule.isEditable())
            {
                if (rule.showUI() == false)
                    choseAdd = false;
            }
            if (choseAdd)
            {
                _chosenRule.Add(rule);
                rulesListBox.ItemsSource = null;
                rulesListBox.ItemsSource = _chosenRule;
            }

        }

        private void rulesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rulesListBox.SelectedItem == null)
                return;
            int index = rulesListBox.SelectedIndex;
            var rule = _chosenRule[index];
            txt_description.Text = rule.ruleDescription;
            if (rule.isEditable())
            {
                buttonEdit.Visibility = Visibility.Visible;
            }

        }

        private void buttonEditClick(object sender, RoutedEventArgs e)
        {
            if (rulesListBox.SelectedItem == null)
                return;
            int index = rulesListBox.SelectedIndex;
            var rule = _chosenRule[index].Clone();
            if (rule.showUI() == true)
            {
                _chosenRule[index] = rule;
                txt_description.Text = rule.ruleDescription;
            }

        }

        private void removeRule(object sender, RoutedEventArgs e)
        {
            if (rulesListBox.SelectedItem == null)
                return;
            int index = rulesListBox.SelectedIndex;

            _chosenRule.RemoveAt(index);
            txt_description.Text = "";

        }

        private void removeItem(object sender, RoutedEventArgs e)
        {
            if (ComboType.SelectedItem == null)
            {
                return;
            }
            if (ComboType.SelectedItem.ToString() == "File")
            {
                int index = filesListBox.SelectedIndex;
                _listFile.RemoveAt(index);
            }
            else if (ComboType.SelectedItem.ToString() == "Folder")
            {
                int index = filesListBox.SelectedIndex;
                _listFolder.RemoveAt(index);
            }
        }

        private void moveRuleToTop(object sender, MouseButtonEventArgs e)
        {
            int index = rulesListBox.SelectedIndex;
            if (index != -1)
            {
                IRule temp = _chosenRule[index];
                for (int i = index; i > 0; i--)
                {
                    _chosenRule[i] = _chosenRule[i - 1];
                }
                _chosenRule[0] = temp;
                rulesListBox.SelectedIndex = 0;
            }
        }

        private void moveRuleToBottom(object sender, MouseButtonEventArgs e)
        {
            int index = rulesListBox.SelectedIndex;
            if (index != -1)
            {
                IRule temp = _chosenRule[index];
                for (int i = index; i < _chosenRule.Count - 1; i++)
                {
                    _chosenRule[i] = _chosenRule[i + 1];
                }
                _chosenRule[_chosenRule.Count - 1] = temp;
                rulesListBox.SelectedIndex = _chosenRule.Count - 1;
            }
        }

        private void moveRuleToPrev(object sender, MouseButtonEventArgs e)
        {
            int index = rulesListBox.SelectedIndex;
            if (index != -1 && index != 0)
            {
                IRule temp = _chosenRule[index];
                _chosenRule[index] = _chosenRule[index - 1];
                _chosenRule[index - 1] = temp;
                rulesListBox.SelectedIndex = index - 1;
            }
        }

        private void moveRuleToNext(object sender, MouseButtonEventArgs e)
        {
            int index = rulesListBox.SelectedIndex;

            if (index != -1 && index != _chosenRule.Count - 1)
            {
                IRule temp = _chosenRule[index];
                _chosenRule[index] = _chosenRule[index + 1];
                _chosenRule[index + 1] = temp;

                rulesListBox.SelectedIndex = index + 1;
            }
        }

        private void Handle_Preview(object sender, RoutedEventArgs e)
        {
            bool isFile = true;
            if (ComboType.SelectedItem == null)
                return;

            ObservableCollection<Item> previewList = new ObservableCollection<Item>();
            if (ComboType.SelectedItem.ToString() == "File")
            {
                isFile = true;
                foreach (Item item in _listFile)
                    previewList.Add(item.Clone());
                addRuleToItem(previewList, isFile);
                for (int i = 0; i < previewList.Count; i++)
                {
                    previewList[i].itemName = _listFile[i].itemName;
                    _listFile[i] = previewList[i].Clone();
                }


            }

            else if (ComboType.SelectedItem.ToString() == "Folder")
            {
                isFile = false;
                foreach (Item item in _listFolder)
                    previewList.Add(item.Clone());
                addRuleToItem(previewList, isFile);
                for (int i = 0; i < previewList.Count; i++)
                {
                    previewList[i].itemName = _listFolder[i].itemName;
                    _listFolder[i] = previewList[i].Clone();
                }
            }

            filesListBox.ItemsSource = null;
            filesListBox.ItemsSource = previewList;
        }

        private void addRuleToItem(ObservableCollection<Item> list, bool isFile)
        {

            foreach (Item item in list)
                item.newItemName = item.itemName;
            foreach (IRule rule in _chosenRule)
            {
                rule.Rename(list, isFile);
                foreach (Item item in list)
                    item.itemName = item.newItemName;
            }
        }

        private void Click_Apply(object sender, RoutedEventArgs e)
        {
            if (ComboType.SelectedItem == null)
            {
                return;
            }
            Handle_Preview(sender, e);
            if (ComboType.SelectedItem.ToString() == "File")
            {
                foreach (Item file in _listFile)
                {
                    if (checkBoxOriginals.IsChecked == true)
                    {
                        File.Move(Path.Combine(file.path, file.itemName), Path.Combine(file.path, file.newItemName));
                        file.itemName = file.newItemName;
                    }
                    else if (checkBoxAnother.IsChecked == true)
                    {
                        File.Copy(Path.Combine(file.path, file.itemName), Path.Combine(checkBoxAnother.Header.ToString(), file.newItemName));
                    }
                }

            }
            else if (ComboType.SelectedItem.ToString() == "Folder")
            {
                foreach (Item folder in _listFolder)
                {
                    if (checkBoxOriginals.IsChecked == true)
                    {
                        Directory.Move(Path.Combine(folder.path, folder.itemName), Path.Combine(folder.path, folder.newItemName));
                        folder.itemName = folder.newItemName;
                    }
                    else if (checkBoxAnother.IsChecked == true)
                    {
                        CopyFilesRecursively(Path.Combine(folder.path, folder.itemName), Path.Combine(checkBoxAnother.Header.ToString(), folder.newItemName));
                    }
                }
            }
        }
        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        private void saveFileToOriginals(object sender, RoutedEventArgs e)
        {
            if (checkBoxOriginals.IsChecked == true)
                checkBoxAnother.IsChecked = false;
            else
                checkBoxAnother.IsChecked = true;
        }


        private void saveFileToAnother(object sender, RoutedEventArgs e)
        {
            if (checkBoxAnother.IsChecked == true)
            {
                checkBoxOriginals.IsChecked = false;

                var Folderdialog = new FolderBrowserDialog();
                var result = Folderdialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(Folderdialog.SelectedPath))
                {
                    string path = Folderdialog.SelectedPath;
                    checkBoxAnother.Header = path;
                }
            }
            else
                checkBoxOriginals.IsChecked = true;


            
        }

        private void clearRule(object sender, RoutedEventArgs e)
        {
            _chosenRule.Clear();
            txt_description.Text = "";
        }
    }
}
