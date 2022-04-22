using Newtonsoft.Json;
using System;
using System.Windows;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ToDoListWPF.Weather;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ToDoList.Database;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;
using System.Globalization;

namespace ToDoListWPF
{
    /// <summary>
    /// Logika interakcji dla klasy WorkTAsksPage.xaml
    /// </summary>
    /// 

    public class MainViewModel
    {
        public HashSet<DateTime> Dates { get; } = new HashSet<DateTime>();

        public MainViewModel()
        {
            // highlight today and tomorrow
            this.Dates.Add(DateTime.Today);
            this.Dates.Add(DateTime.Today.AddDays(5));
        }
    }

    public class LookupConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)values[0];
            var dates = values[1] as HashSet<DateTime>;
            return dates.Contains(date);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }

    public partial class WorkTAsksPage : Page
    {

        /// <summary>
        /// Inicjalizacja strony i bazy danych do aplikacji 
        /// </summary>
        public WorkTAsksPage()
        {
            InitializeComponent();

            DataContext = new WorkTasksPageViewModel();

            TaskListDBEntities db = new TaskListDBEntities();


        }

        string APIKey = "d5e7c9c9291f1474db8e2d26f5282781";

        /// <summary>
        /// Funkcja zwracające wybrane wartosci pogodowe
        /// </summary>
        private void loadSunInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            getWeather();
        }

        /// <summary>
        /// Funkcja pobierająca przez API wybrane wartosci pogodowe i pozwalające sprawdzic je w dowolnej lokalizacji jesli taak istnieje 
        /// </summary>
        void getWeather()
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", CityToCHeck.Text, APIKey);
                try
                {
                    var json = web.DownloadString(url);
                    WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);
                    picIcon.Source = new BitmapImage(new Uri("https://openweathermap.org/img/w/" + Info.weather[0].icon + ".png"));

                    temperatureText.Text = Info.main.temp.ToString();

                    preasureText.Text = Info.main.pressure.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid name");
                }

            }
        }

    }
}
