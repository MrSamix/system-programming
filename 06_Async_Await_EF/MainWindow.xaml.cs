using _06_Async_Await_EF.EF;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Data.Common;
using System.Data;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Windows.Media.Animation;

namespace _06_Async_Await_EF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    Db_initilizer db;
    public MainWindow()
    {
        InitializeComponent();
        db = new Db_initilizer();
        foreach (var author in db.Authors)
        {
            cbauthor.Items.Add(author);
        }
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        var authorsel = cbauthor.SelectedItem;
        if (authorsel is null)
        {
            MessageBox.Show("Select a author!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        string[] author = authorsel.ToString().Split();
        string authorname = author[0];
        string authorsurname = author[1];
        //MessageBox.Show(author);

        string name = book.Text;
        if (!string.IsNullOrWhiteSpace(name))
        {
            if (name.Length < 3)
            {
                MessageBox.Show("Less 3 symbols in name book input box", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        var task = SearchBookAsync(authorname, authorsurname, name);
        await task;
        res.ItemsSource = task.Result;

    }


    private Task<List<Book>> SearchBookAsync(string authorname, string authorsurname, string bookname)
    {
        return Task.Run(() =>
        {
            if (!string.IsNullOrWhiteSpace(bookname))
            {
                var resultwithbook = db.Books.Include(a => a.Author)
                .Where(b => b.Author.Name == authorname && b.Author.Surname == authorsurname && b.Name.Contains(bookname))
                .ToList();
                Thread.Sleep(1000);
                return resultwithbook;
            }
            var result = db.Books.Include(a => a.Author)
                .Where(b => b.Author.Name == authorname && b.Author.Surname == authorsurname)
                .ToList();
            Thread.Sleep(1000);
            return result;
        });
    }

}