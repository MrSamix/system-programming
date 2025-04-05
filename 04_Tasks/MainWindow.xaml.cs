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

namespace _04_Tasks;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        tbpath.Visibility = Visibility.Hidden;
        path.Visibility = Visibility.Hidden;
        res.Visibility = Visibility.Hidden;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        string txt = text.Text;
        if (string.IsNullOrWhiteSpace(txt))
        {
            MessageBox.Show("Textbox is empty.");
            return;
        }
        if (rbtn.IsChecked == false && rbtn2.IsChecked == false)
        {
            MessageBox.Show("Check a option!");
            return;
        }
        if (rbtn.IsChecked == true && string.IsNullOrWhiteSpace(path.Text))
        {
            MessageBox.Show("Enter a path to file!");
            return;
        }

        Task<int> task_sentence = Task.Run(() => Count.CountOfSentences(txt));
        Task<int> task_symbol = Task.Run(() => Count.CountOfSymbols(txt));
        Task<int> task_word = Task.Run(() => Count.CountOfWords(txt));
        Task<int> task_question = Task.Run(() => Count.CountOfQuestions(txt));
        Task<int> task_exclamatory_sentence = Task.Run(() => Count.CountOfExclamatorySentences(txt));

        Task.WaitAll(task_sentence, task_symbol, task_word, task_question, task_exclamatory_sentence);

        int sentences = task_sentence.Result;
        int symbols = task_symbol.Result;
        int words = task_word.Result;
        int questions = task_question.Result;
        int exclamatory_sentences = task_exclamatory_sentence.Result;

        string res_str = $"Sentences: {sentences}\nSymbols: {symbols}\nWords: {words}\nQuestions: {questions}\nExclamatory sentences: {exclamatory_sentences}";

        if (rbtn.IsChecked == true)
        {
            if (Path.Exists(path.Text))
            {
                var messageBoxResult = MessageBox.Show("This file is already created. Overwrite?", "Overwrite?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    MessageBox.Show("Please, enter a another path.");
                    return;
                }
            }
            File.WriteAllText(path.Text, res_str);
            MessageBox.Show("Result is written into a file!");
        }
        else
        {
            res.Visibility = Visibility.Visible;
            resdetail.Text = res_str;
        }


    }

    private void rbtn_Checked(object sender, RoutedEventArgs e)
    {
        tbpath.Visibility = Visibility.Visible;
        path.Visibility = Visibility.Visible;
    }

    private void rbtn_Unchecked(object sender, RoutedEventArgs e)
    {
        tbpath.Visibility = Visibility.Hidden;
        path.Visibility = Visibility.Hidden;
    }
}


public static class Count
{
    public static int CountOfSentences(string text)
    {
        return text.Count((char s) => s == '.' || s == '!' || s == '?');
    }
    public static int CountOfSymbols(string text)
    {
        return text.Count();
    }
    public static int CountOfWords(string text)
    {
        var words = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }
    public static int CountOfQuestions(string text)
    {
        return text.Count((char s) => s == '?');
    }
    public static int CountOfExclamatorySentences(string text)
    {
        return text.Count((char s) => s == '!');
    }
}