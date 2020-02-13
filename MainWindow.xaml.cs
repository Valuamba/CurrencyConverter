using System;
using System.Windows;

using CurrencyConverter.ViewModels;
using CurrencyConverter.ViewModels.Converters;


namespace CurrencyConverter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

        }

        //Допускает введение только десятичных цифр
        public void OnRestrictLettersEvent(object sender, System.Windows.Input.KeyEventArgs e )
        {
            e.Handled = !Char.IsDigit(KeyToChar.GetCharFromKey(e.Key));
        }

       
    }
}
