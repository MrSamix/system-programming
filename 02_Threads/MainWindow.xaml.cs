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

namespace _02_Threads;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    Thread thread;
    Thread thread2;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonPrimeNumbers(object sender, RoutedEventArgs e)
    {
        lb1.Items.Clear();
        int from = 2;
        int to = int.MaxValue;

        try
        {
            if (!string.IsNullOrWhiteSpace(tbfrom.Text))
            {
                from = int.Parse(tbfrom.Text);
            }
            if (!string.IsNullOrWhiteSpace(tbto.Text))
            {
                to = int.Parse(tbto.Text);
                if (to < from)
                {
                    throw new ArgumentException($"To number({to}) < min number({from})");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        thread = new Thread(() =>
        {
            Application.Current.Dispatcher.Invoke(() => StopButtonPrime.IsEnabled = true);
            try
            {
                for (int i = from; i < to; i++)
                {
                    bool PrimeNumber = true;
                    for (int j = 2; j < i; j++)
                    {
                        if (i % j == 0)
                        {
                            PrimeNumber = false;
                            break;
                        }
                    }
                    if (PrimeNumber)
                    {
                        Application.Current.Dispatcher.Invoke(() => lb1.Items.Add(i));
                        Thread.Sleep(100);
                    }
                }
                Application.Current.Dispatcher.Invoke(() => StopButtonPrime.IsEnabled = false);
            }
            catch (System.Threading.ThreadInterruptedException)
            {
                Application.Current.Dispatcher.Invoke(() => StopButtonPrime.IsEnabled = false);
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show("Stopped!"));
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    StopButtonPrime.IsEnabled = false;
                });
            }
        });
        thread.Start();
    }

    private void ButtonFibonachiNumbers(object sender, RoutedEventArgs e)
    {
        lb2.Items.Clear();
        int count = int.MaxValue;
        thread2 = new Thread(() =>
        {
            Application.Current.Dispatcher.Invoke(() => StopButtonFibonachi.IsEnabled = true);
            try
            {
                long a = 0;
                long b = 1;
                Application.Current.Dispatcher.Invoke(() => lb2.Items.Add(a));
                Application.Current.Dispatcher.Invoke(() => lb2.Items.Add(b));

                for (int i = 2; i < count; i++)
                {
                    long temp = a + b;
                    a = b;
                    b = temp;
                    Application.Current.Dispatcher.Invoke(() => lb2.Items.Add(b));
                    Thread.Sleep(100);
                }
                Application.Current.Dispatcher.Invoke(() => StopButtonFibonachi.IsEnabled = false);
            }
            catch (System.Threading.ThreadInterruptedException)
            {
                Application.Current.Dispatcher.Invoke(() => StopButtonFibonachi.IsEnabled = false);
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show("Stopped!"));
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    StopButtonFibonachi.IsEnabled = false;
                });
            }
        });
        thread2.Start();
    }

    private void StopButtonPrimeNumbers(object sender, RoutedEventArgs e)
    {
        thread.Interrupt();        
    }

    private void StopButtonFibonachiNumbers(object sender, RoutedEventArgs e)
    {
        thread2.Interrupt();
    }
}