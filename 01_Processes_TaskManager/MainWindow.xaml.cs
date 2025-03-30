using System.Diagnostics;
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
using System.Windows.Threading;

namespace _01_Processes_TaskManager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    DispatcherTimer timer = null;
    int second = 0;
    RadioButton radioButton = null;
    public MainWindow()
    {
        InitializeComponent();
        timer = new DispatcherTimer();
        //timer.Interval = new TimeSpan(0, 0, 20);
        timer.Tick += Timer_Tick;
        //timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        Refresh(sender, null);
    }

    private void Refresh(object sender, RoutedEventArgs e)
    {
        grid.ItemsSource = Process.GetProcesses();
    }

    private void RadioButton_Click(object sender, RoutedEventArgs e)
    {
        radioButton = sender as RadioButton;
        // is null check
        if (radioButton != null)
        {
            second = int.Parse(radioButton.Content.ToString());
            timer.Interval = new TimeSpan(0, 0, second);
            timer.Start();
        }
    }

    private void Kill(object sender, RoutedEventArgs e)
    {
        Process process = grid.SelectedItem as Process;
        if (process != null)
        {
            try
            {
                process.Kill();
                MessageBox.Show($"{process.ProcessName} ({process.Id}) is killed.");
            }
            catch (Exception)
            {
                MessageBox.Show($"{process.ProcessName} can't killed.");
            }
            
        }
        else
        {
            MessageBox.Show("No process selected.");
        }
    }

    private void StopRefresh(object sender, RoutedEventArgs e)
    {
        if (timer.IsEnabled)
            timer.Stop();

        if (radioButton != null)
            radioButton.IsChecked = false;
    }

    private void ShowDetail(object sender, RoutedEventArgs e)
    {
        Process process = grid.SelectedItem as Process;
        if (process != null)
        {
            try
            {
                string detail = $"Process Name : {process.ProcessName}\nID : {process.Id}\nStart Time : {process.StartTime}\nTotal Processor Time : {process.TotalProcessorTime}\nPriority : {process.PriorityClass}\nMemory used : {(double)(process.PagedMemorySize64 / 1024) / 1024} MB";
                MessageBox.Show(detail.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show($"Process Name : {process.ProcessName}\nID : {process.Id}\n");
            }
            
        }
        else
        {
            MessageBox.Show("No process selected.");
        }
    }

    private void Go(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(process.Text) && process.Text != "Enter name process:")
            {
                Process.Start(@$"{process.Text}");
            }
            else
            {
                MessageBox.Show("Enter process name.");
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Can't start process.");
        }
        
    }

    private void TextBoxGotFocus(object sender, RoutedEventArgs e)
    {
        (sender as TextBox).Text = "";
    }

    private void TextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (string.IsNullOrWhiteSpace(textBox.Text))
        {
            textBox.Text = "Enter name process:";
        }
    }
}