using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.ObjectModel;

using CurrencyConverter.Data;
using CurrencyConverter.ViewModels.Commands;


namespace CurrencyConverter.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private readonly string url = @"https://www.cbr-xml-daily.ru/daily_json.js";

        //Было ли принятие данных
        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                _isLoaded = value;
            }
        }
        
        //Найдена ли волюта в строке поиска, если нет скрывает поля вывода
        private bool _isSearched;
        public bool IsSearched
        {
            get => _isSearched;
            set
            {
                _isSearched = value;
                OnPropertyChanged("IsSearched");
            }
        }

        //Цвет текста, показывает правильно ли ввели код валюты или страны
        private Brush _searchBorderBrush;
        public Brush SearchBorderBrush
        {
            get => _searchBorderBrush;
            set
            {
                _searchBorderBrush = value;
                OnPropertyChanged("SearchBorderBrush");
            }
        }

        //Коллекция с данными
        private ObservableCollection<ValuteType> _box;
        public ObservableCollection<ValuteType> Box { get => _box; set { _box = value; } }

        //Эквивалент валюты в BYN
        private double _eqwBYN;
        public double eqwBYN
        {
            get => _eqwBYN;
            set
            {
                _eqwBYN = value;
                OnPropertyChanged("eqwBYN");
            }
        }

        //Эквивалент валюты в USD
        private double _eqwUSD;
        public double eqwUSD
        {
            get => _eqwUSD;
            set
            {
                _eqwUSD = value;
                OnPropertyChanged("eqwUSD");
            }
        }

        //Первая выбранная валюта в ComboBox
        private ValuteType _firstSelectedValute;
        public ValuteType FirstSelectedValute
        {
            get => _firstSelectedValute;
            set
            {
                _firstSelectedValute = value;
                OnPropertyChanged("FirstSelectedValute");
            }
        }

        //Вторая выбранная валюта в ComboBox
        private ValuteType _secondSelectedValute;
        public ValuteType SecondSelectedValute
        {
            get => _secondSelectedValute;
            set
            {
                _secondSelectedValute = value;
                OnPropertyChanged("SecondSelectedValute");
            }
        }


        //Значение первой валюты, набранное пользователем
        private double _firstValueValute;
        public double FirstValueValute
        {
            get => _firstValueValute;
            set
            {
                _firstValueValute = value;
                OnPropertyChanged("FirstValueValute");
            }
        }

        //Значение второй валюты, набранное пользователем
        private double _secondValueValute;
        public double SecondValueValute
        {
            get => _secondValueValute;
            set
            {
                _secondValueValute = value;
                OnPropertyChanged("SecondValueValute");
            }
        }
        
        public MainViewModel()
        {
            _box = new ObservableCollection<ValuteType>();

            _searchBorderBrush = Brushes.Black;
            
            _loadCommand = new RelayCommand(x => LoadButtonCommand());

            _updateCommand = new RelayCommand(x => UpdateButtonCommand());

            _searchCommand = new RelayCommand(x => SearchButtonCommand());

            _convertCommand = new RelayCommand(x => FirstTextBoxConvertCommand());

            _secondConvertCommand = new RelayCommand(x => SecondTextBoxConvertCommand());
        }

        //Строка Поиска по коду или стране
        private string _searchCode;
        public string SearchCode
        {
            get => _searchCode;
            set
            {
                _searchCode = value;
                OnPropertyChanged("SearchCode");
            }
        }

        //Валюта найденная в результате поиска по коду или стране
        private ValuteType _selectedCurrency;
        public ValuteType SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged("SelectedCurrency");
            }
        }


        #region ICommands

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get => _loadCommand;
            set { _loadCommand = value; }
        }


        private ICommand _updateCommand;
        public ICommand UpdateCommand
        {
            get => _updateCommand;
            set { _updateCommand = value; }
        }


        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get => _searchCommand;
            set { _searchCommand = value; }
        }


        private ICommand _convertCommand;
        public ICommand ConvertCommand
        {
            get => _convertCommand;
            set { _convertCommand = value; }
        }

        private ICommand _secondConvertCommand;
        public ICommand SecondConvertCommand
        {
            get => _secondConvertCommand;
            set { _secondConvertCommand = value; }
        }
        #endregion


        #region ImplementationOfCommand
        //Выполнение команды поиска валюты, результат поиска и его эквивалент
        private void SearchButtonCommand()
        {
            try
            {
                SelectedCurrency = Box.Single(v =>
                (v.CharCode == _searchCode)
                || (v.NumCode == _searchCode));

                SearchBorderBrush = Brushes.Black;

                eqwBYN = Math.Round(SelectedCurrency.Value / Box.Single(v => v.CharCode == "BYN").Value, 4);

                eqwUSD = Math.Round(SelectedCurrency.Value / Box.Single(v => v.CharCode == "USD").Value, 4);

                IsSearched = true;
            }

            catch
            {
                SearchBorderBrush = Brushes.Red;
                IsSearched = false;
            }
        }

        //Выполнение команды загрузки данных(только один раз)
        private async void LoadButtonCommand()
        {
            var showCurr = await ShowCurrency.
                    _download_serialized_json_data(url);

    
            if (!_isLoaded)
            {
                foreach (var vl in showCurr.Valute.Values)
                {
                    _box.Add(vl);
                }
                _isLoaded = true;
            }
        }

        //Обновление данных
        private async void UpdateButtonCommand()
        {
            var showCurr = await ShowCurrency.
                    _download_serialized_json_data(url);

            if (_isLoaded)
            {
                _box.Clear();
                foreach (var vl in showCurr.Valute.Values)
                {
                    _box.Add(vl);
                }
            }
        }

        //Эквивалент первой валюты, ввод - в поле для первой валюты
        public void FirstTextBoxConvertCommand()
        {
            if(FirstSelectedValute != null
                && SecondSelectedValute != null)
            {
                SecondValueValute = Math.Round(FirstSelectedValute.Value/SecondSelectedValute.Value * SecondSelectedValute.Nominal * FirstValueValute,4);
            }
        }

        //Эквивалент второй валюты, ввод - в поле для второй валюты
        public void SecondTextBoxConvertCommand()
        {
            if (FirstSelectedValute != null
                && SecondSelectedValute != null)
            {
                FirstValueValute = Math.Round(SecondSelectedValute.Value / FirstSelectedValute.Value * FirstSelectedValute.Nominal * SecondValueValute, 4);
            }
        }


        //public void TextBoxConvertCommands(ValuteType first, ValuteType second)
        //{
        //    if (FirstSelectedValute != null
        //        && SecondSelectedValute != null)
        //    {
        //        SecondValueValute = Math.Round(first.Value / second.Value * second.Nominal * FirstValueValute, 4);
        //    }
        //}
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
