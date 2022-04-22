using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Miejsce gdzie przez poszczegolne klasy odwolujemy sie wybranych przez nas wartosci pozwalajac dzieki API na pobranie tych wartosci 
/// </summary>
namespace ToDoListWPF.Weather
{
    class WeatherInfo
    {
        public class coord
        {
            public double lon { get; set; }
            public double lat { get; set; }

        }

        public class weather
        {
            public string main { get; set; }
            public string descriptoin { get; set; }
            public string icon { get; set; }
        }

        public class main
        {
            public double temp { get; set; }
            public double pressure { get; set; }
        }

        
        public class root
        {
            public coord coord { get; set; }
            public List<weather> weather { get; set; }
            public main main { get; set; }
        }



    }
}
