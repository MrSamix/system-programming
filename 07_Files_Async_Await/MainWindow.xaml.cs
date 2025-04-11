using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _07_Files_Async_Await;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public string[] Path { get; set; }
    private List<Candidate> candidates;
    private List<Candidate> results;
    public MainWindow()
    {
        InitializeComponent();
        candidates = new List<Candidate>();
        results = new List<Candidate>();
    }

    private async void Find(object sender, RoutedEventArgs e)
    {
        Path = path.Text.Split('\n',StringSplitOptions.RemoveEmptyEntries);
        lbres.ItemsSource = null;
        
        if (Directory.Exists(path.Text))
        {
            Path = Directory.GetFiles(path.Text);
        }
        int selectedrb = ReturnRbChecked();

        if (selectedrb == -1)
        {
            MessageBox.Show("Please, check option", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (Path.Length == 0)
        {
            MessageBox.Show("Please, select or enter path to file/folder", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        candidates.Clear();
        LoadCV();

        // add async
        if (selectedrb == 0)
        {
            await FindMaxYearExperienceCandidatesAsync();
        }
        else if (selectedrb == 1)
        {
            await FindMinYearExperienceCandidatesAsync();
        }
        else if (selectedrb == 2)
        {
            await SortByCityAsync();
        }
        else if (selectedrb == 3)
        {
            await CandidatesWithMaxSalary();
        }
        else
        {
            await CandidatesWithMinSalary();
        }

    }

    private void OpenFile(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Multiselect = true;
        if (dialog.ShowDialog() == true)
        {
            path.Text = string.Join('\n', dialog.FileNames);
        }
    }

    private void OpenFolder(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog(); // з .NET 8 є вбудований клас OpenFolderDialog
        dialog.IsFolderPicker = true; // only folder
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            path.Text = dialog.FileName;
        }
    }

    public int ReturnRbChecked()
    {
        if (rbmaxyear.IsChecked == true)
        {
            return 0;
        }
        else if (rbminyear.IsChecked == true)
        {
            return 1;
        }
        else if (rbcity.IsChecked == true)
        {
            return 2;
        }
        else if (rbmaxsalary.IsChecked == true)
        {
            return 3;
        }
        else if (rbminsalary.IsChecked == true)
        {
            return 4;
        }
        else
        {
            return -1;
        }
    }


    public void SaveCV()
    {
        var candidate1 = new Candidate("Sasha", "Rivne", 30000, 5);
        var candidate2 = new Candidate("Oleg", "Kyiv", 15000, 10);
        var candidate3 = new Candidate("Sasha Kovalchuk", "Warshaw", 5000, 1);

        candidate1.SaveCandidate();
        candidate2.SaveCandidate();
        candidate3.SaveCandidate();
    }

    public void LoadCV()
    {
        foreach (var path in Path)
        {
            var candidate = new Candidate();
            candidate.LoadCandidate(path);
            candidates.Add(candidate);
        }
    }

    public Task FindMaxYearExperienceCandidatesAsync()
    {
        return Task.Run(() =>
        {
            var max = candidates.Where((c) => c.YearExperience == candidates.Max((c) => c.YearExperience));
            Thread.Sleep(1000);
            Dispatcher.Invoke(() => lbres.ItemsSource = max);
        });
    }

    public Task FindMinYearExperienceCandidatesAsync()
    {
        return Task.Run(() =>
        {
            var min = candidates.Where((c) => c.YearExperience == candidates.Min((c) => c.YearExperience));
            Thread.Sleep(1000);
            Dispatcher.Invoke(() => lbres.ItemsSource = min);
        });
    }

    public Task SortByCityAsync()
    {
        return Task.Run(() =>
        {
            List<string> res = new List<string>();
            var cities = candidates.Select((c) => c.City).Distinct();
            foreach (var city in cities)
            {
                var query = candidates.Where(c => c.City == city).ToList();
                res.Add(city);
                for (int i = 0; i < query.Count(); i++)
                {
                    res.Add("\t" + query[i].ToString());
                }
            }
            Thread.Sleep(1000);
            Dispatcher.Invoke(() => lbres.ItemsSource = res);
        });
    }
    public Task CandidatesWithMaxSalary()
    {
        return Task.Run(() =>
        {
            var max = candidates.Where((c) => c.Salary == candidates.Max((c) => c.Salary));
            Thread.Sleep(1000);
            Dispatcher.Invoke(() => lbres.ItemsSource = max);
        });
    }

    public Task CandidatesWithMinSalary()
    {
        return Task.Run(() =>
        {
            var min = candidates.Where((c) => c.Salary == candidates.Min((c) => c.Salary));
            Thread.Sleep(1000);
            Dispatcher.Invoke(() => lbres.ItemsSource = min);
        });
    }
}