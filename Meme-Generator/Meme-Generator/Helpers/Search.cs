using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;

namespace Meme_Generator.Helpers
{
    public class Search
    {
        public List<Tuple<Image, string, string, string>> getImagesFromSearch(string searchFor = "")
        {
            List<Tuple<Image, string, string, string>> list = new List<Tuple<Image, string, string, string>>();

            string requestUrl = string.Empty;
            if (searchFor == string.Empty)
            {
                requestUrl = "https://memegenerator.net/Proxy/Api?q=&pageIndex=0&pageSize=25&method=Generators_Search";
            }
            else
            {
                requestUrl = $"https://memegenerator.net/Proxy/Api?q={searchFor.Replace(" ", "+")}&pageIndex=0&pageSize=&method=Generators_Search";
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "GET";
            request.Host = "memegenerator.net";
            request.KeepAlive = true;
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Referer = "https://memegenerator.net/create";
            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
            request.Headers.Add("Cookie", "ASP.NET_SessionId=yo0gjyq1ca4isdc2hi1ooeb1; GaExperimentTickets=[]; languageCode=null; __unam=6cb5566-15a9b91fbb4-122a2d63-40");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string html = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                html = reader.ReadToEnd();
                reader.Close();
            }

            dynamic json = JsonConvert.DeserializeObject(html);
            for (int i = 0; i < json.result.Count; i++)
            {
                string urlName = json.result[i].urlName.ToString();
                string generatorId = json.result[i].generatorID.ToString();
                string imageId = json.result[i].imageID.ToString();
                string imageUrl = json.result[i].imageUrl.ToString();

                using (WebClient client = new WebClient())
                {
                    byte[] data = client.DownloadData(imageUrl);
                    using (MemoryStream stream = new MemoryStream(data))
                    {
                        Image image = Image.FromStream(stream);
                        list.Add(new Tuple<Image, string, string, string>(image, urlName, generatorId, imageId));
                    }
                }
            }

            return list;
        }
    }
}
