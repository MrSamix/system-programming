using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace _09_Exam;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public string DirectoryPath { get; set; }
    public string FindWord { get; set; }
    private int FilesCompleted = 0;
    public string SaveFilePath { get; set; }
    private List<string> items;
    public MainWindow()
    {
        InitializeComponent();
        items = new List<string>();
    }

    private async void Find(object sender, RoutedEventArgs e)
    {
        items.Clear();
        FilesCompleted = 0;
        DirectoryPath = tbpath.Text;

        if (string.IsNullOrEmpty(DirectoryPath))
        {
            MessageBox.Show("Path is empty. Please enter path to directory!", "Empty", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrEmpty(tbword.Text))
        {
            MessageBox.Show("Please enter a word!", "Empty", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        FindWord = tbword.Text;
        if (!Directory.Exists(DirectoryPath))
        {
            MessageBox.Show("Directory not founded!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var files = Directory.GetFiles(DirectoryPath, "*.*", SearchOption.AllDirectories);
        int countOfFiles = files.Length;
        if (countOfFiles == 0)
        {
            MessageBox.Show("No files founded!", "Empty Folder", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        pb.Maximum = countOfFiles;

        listbox.ItemsSource = null;
        listbox.ItemsSource = items;
        pb.Value = FilesCompleted;

        await Task.Run(() =>
        {
            Parallel.ForEach(files, file =>
            {
                FindWordInFile(file);
                Dispatcher.Invoke(() =>
                {
                    pb.Value = FilesCompleted;
                    listbox.Items.Refresh();
                });
            });
        });
        MessageBox.Show("Completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void OpenFolderBtn(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true; // only folder
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            tbpath.Text = dialog.FileName;
            
        }
    }

    private void FindWordInFile(string path)
    {
        Random rnd = new Random();
        int countOfWord = 0;
        var allwords = File.ReadAllText(path).Split(' ', StringSplitOptions.TrimEntries);
        foreach (var word in allwords)
        {
            if (word == FindWord)
            {
                countOfWord++;
            }
        }
        string res = $"Filename: {Path.GetFileName(path)}\nPath: {path}\nCount find word: {countOfWord}";
        Thread.Sleep(rnd.Next(100, 1500));
        items.Add(res);
        FilesCompleted++;
    }

    private void ChooseSaveDirectory(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog();
        dialog.DefaultExt = ".txt";
        dialog.Filter = "Text documents (.txt)|*.txt";
        if (dialog.ShowDialog() == true)
        {
            SaveFilePath = dialog.FileName;
            tbsavepath.Text = dialog.FileName;
        }
    }

    private void SaveResults(object sender, RoutedEventArgs e)
    {
        File.WriteAllText(SaveFilePath, string.Join('\n', items));
        MessageBox.Show("Saved!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}