using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace TP2
{
    public class Program
    {
        public class Key
        {
            public String apiKey;
            public Key(string key)
            {
                apiKey = key;
            }

            public AllWeather getWeatherData(string access)
            {
                string jsonString;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(access + apiKey));
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                request.Method = "GET";
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader streamRead = new StreamReader(stream, System.Text.Encoding.UTF8);
                    jsonString = streamRead.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<AllWeather>(jsonString);
            }
        }


        public static async Task Main(string[] args)
        {

            Key api = new Key("c797631c5eddfebc5da5b8451261e31c");

            //Weather at morocco
            AllWeather morocco = api.getWeatherData("https://api.openweathermap.org/data/2.5/weather?lat=32&lon=-5&APPID=");
            Console.WriteLine("The time at Morocco is " + morocco.weather[0].main + " (" + morocco.weather[0].description + ")");

            //Sun rise and set in Oslo UTC time
            AllWeather oslo = api.getWeatherData("https://api.openweathermap.org/data/2.5/weather?q=Oslo&APPID=");
            DateTimeOffset sunrise = DateTimeOffset.FromUnixTimeSeconds(oslo.sys.sunrise);
            DateTimeOffset set = DateTimeOffset.FromUnixTimeSeconds(oslo.sys.sunset);
            Console.WriteLine("Today in Oslo the sun rise time is : " + sunrise);
            Console.WriteLine("Today in Oslo the set time is : " + set);

            //Temperature in Jakarta in  Celsus
            AllWeather jakarta = api.getWeatherData("https://api.openweathermap.org/data/2.5/weather?q=Jakarta&APPID=");
            Console.WriteLine("The temperatue in Jakarta is : " + (int)Math.Round((jakarta.main.temp - 273.15)) + "°C");

            //more windy newyork/toyko/paris
            AllWeather newYork = api.getWeatherData("https://api.openweathermap.org/data/2.5/weather?q=New%20York&APPID=");
            AllWeather tokyo = api.getWeatherData("https://api.openweathermap.org/data/2.5/weather?q=Tokyo&APPID=");
            AllWeather paris = api.getWeatherData("https://api.openweathermap.org/data/2.5/weather?q=Paris&APPID=");
            AllWeather wind = tokyo;
            if (paris.wind.speed > wind.wind.speed)
            {
                wind = paris;
            }
            if (newYork.wind.speed > wind.wind.speed)
            {
                wind = newYork;
            }
            Console.WriteLine("The city where there is the most wind between these 3 countries (New York/Tokyo/Paris) is : " + wind.name);

            //Humidity pressure in Kiev, Moscow and Berlin
            AllWeather kiev = api.getWeatherData("https://api.openweathermap.org/data/2.5/weather?q=Kiev&APPID=");
            AllWeather moscow = api.getWeatherData("https://api.openweathermap.org/data/2.5/weather?q=Moscow&APPID=");
            AllWeather berlin = api.getWeatherData("https://api.openweathermap.org/data/2.5/weather?q=Berlin&APPID=");

            Console.WriteLine("The humidity in Kiev is : " + kiev.main.humidity + " % , and the pressure is : " + kiev.main.pressure);
            Console.WriteLine("The humidity in Moscow is : " + moscow.main.humidity + " % , and the pressure is : " + moscow.main.pressure);
            Console.WriteLine("The humidity in berlin is : " + berlin.main.humidity + " % , and the pressure is : " + berlin.main.pressure);
            

        }
    }
}
