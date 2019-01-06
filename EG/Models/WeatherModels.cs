﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace EG.Models
{
    public class Weather
    {
        public object getWeather(string localidade)
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?q=barcelos&APPID=565094e4b43f6f2907e009dee8396c9a&units=metric";
            var client = new WebClient();
            var content = client.DownloadString(url);
            var serializer = new JavaScriptSerializer();
            var jsonContent = serializer.Deserialize<object>(content);
            return jsonContent;
        }
    }
}