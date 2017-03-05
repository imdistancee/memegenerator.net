using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Meme_Generator.Helpers
{
    public class Create
    {
        public Image createMeme(Tuple<string, string, string> item, string topText, string bottomText)
        {
            string urlName = item.Item1;
            string generatorId = item.Item2;
            string imageId = item.Item3;

            topText = topText.Replace(" ", "+");
            bottomText = bottomText.Replace(" ", "+");

            string post = $"languageCode=en&generatorID={generatorId}&urlName={urlName}&imageID={imageId}&text0={topText}&text1={bottomText}&uploadToImgur=false";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://memegenerator.net/Xhr/Instance_Create");
            request.Method = "POST";
            request.Host = "memegenerator.net";
            request.KeepAlive = true;
            request.Accept = "*/*";
            request.Headers.Add("Origin", "https://memegenerator.net");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "https://memegenerator.net/create";
            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
            request.Headers.Add("Cookie", "ASP.NET_SessionId=yo0gjyq1ca4isdc2hi1ooeb1; GaExperimentTickets=[]; languageCode=en; __unam=6cb5566-15a9b91fbb4-122a2d63-58");

            byte[] data = Encoding.ASCII.GetBytes(post);
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string html = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                html = reader.ReadToEnd();
                reader.Close();
            }

            dynamic json = JsonConvert.DeserializeObject(html);

            string instance = json.Data.InstanceID.ToString();
            using (WebClient client = new WebClient())
            {
                string input = client.DownloadString($"https://memegenerator.net/instance/{instance}");
                string pattern = $"src=\"(.*?)/{instance}.jpg\"";

                string imageUrl = Regex.Matches(input, pattern)[0].Groups[1].Value + $"/{instance}.jpg";
                byte[] imageData = client.DownloadData(imageUrl);
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(stream);
                    return image;
                }
            }
            
        }
    }
}
