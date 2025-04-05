using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _05_Async_Await;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(numb.Text))
        {
            MessageBox.Show("Enter a number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        try
        {
            int number = int.Parse(numb.Text);
            lbres.Items.Add(await CalcFactorialAsync(number));

        }
        catch (Exception)
        {
            MessageBox.Show("Is a not number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private int CalcFactorial(int numb)
    {
        if (numb == 1 || numb == 0)
        {
            return 1;
        }
        else
        {
            Thread.Sleep(100);
            return numb * CalcFactorial(numb - 1);
        }
    }

    private Task<int> CalcFactorialAsync(int numb)
    {
        return Task.Run(() => CalcFactorial(numb));
    }


}