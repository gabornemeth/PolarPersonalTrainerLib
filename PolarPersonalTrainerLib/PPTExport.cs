using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PolarPersonalTrainerLib
{
    /// <summary>
    /// Exercise exporter
    /// </summary>
    public class PPTExport
    {
        private const string BaseUrl = "https://www.polarpersonaltrainer.com";
        private String _userName;
        private String _password;
        private PPTUser _userData;
        private CookieContainer _jar = new CookieContainer();

        public PPTExport(String userName, String password)
        {
            this._password = password;
            this._userName = userName;
        }

        private String GetTrainingSessions(HtmlNode calItems)
        {
            int itemCount = 0;
            var keyValues = new Dictionary<string, string>();

            keyValues.Add(".action", "export");
            keyValues.Add(".filename", "export.xml");

            foreach (HtmlNode row in calItems.Descendants("tr") ?? Enumerable.Empty<HtmlNode>())
            {
                if (row.GetAttributeValue("class", "").Equals("listHeadRow"))
                    continue;

                foreach (HtmlNode cell in row.Descendants("td") ?? Enumerable.Empty<HtmlNode>())
                {
                    // Check if the training Type is OptimizedExcercise (training data which has a sport assigned)
                    HtmlNode itemType = cell.Elements("input").FirstOrDefault(element => element.GetAttributeValue("name", "") == "calendarItemTypes");

                    if (itemType == null)
                        continue;

                    if (!itemType.GetAttributeValue("value", "").Equals("OptimizedExercise"))
                        continue;

                    HtmlNode itemValue = cell.Elements("input").FirstOrDefault(element => element.GetAttributeValue("name", "") == "calendarItem");

                    if (itemValue == null)
                        continue;

                    keyValues.Add("items." + itemCount + ".item", itemValue.GetAttributeValue("value", ""));
                    keyValues.Add("items." + itemCount++ + ".itemType", "OptimizedExercise");
                }
            }

            if (keyValues.Count <= 2)
                return null;

            var strPost = new StringBuilder();

            foreach (string key in keyValues.Keys)
            {
                strPost.AppendFormat("{0}={1}&", key, WebUtility.UrlEncode(keyValues[key]));
            }
            if (strPost.Length > 0)
                strPost.Remove(strPost.Length - 1, 1); // remove the last '&'

            return strPost.ToString();
        }

        public async Task<PPTUser> GetUser()
        {
            if (_userData != null)
            {
                return _userData;
            }

            var httpClient = await CreateHttpClient();
            var url = BaseUrl + "/user/settings/index.ftl";
            var response = await httpClient.GetAsync(url);
            var responseStr = await response.Content.ReadAsStringAsync();
            HtmlNode.ElementsFlags.Remove("option");
            var doc = new HtmlDocument();
            doc.LoadHtml(responseStr);

            var user = new PPTUser();
            // name of the user
            var nodeFirstName = doc.GetElementbyId("user.name.firstName");
            user.FirstName = nodeFirstName.GetAttributeValue("value", "");

            var nodeLastName = doc.GetElementbyId("user.name.lastName");
            user.LastName = nodeLastName.GetAttributeValue("value", "");

            var nodeNickName = doc.GetElementbyId("user.nickName");
            user.Nickname = nodeNickName.GetAttributeValue("value", "");

            // measurement units
            var nodeUnits = doc.DocumentNode.Descendants("input").FirstOrDefault(node =>
                node.GetAttributeValue("name", "") == "data.units" && node.GetAttributeValue("checked", "false") == "true");
            if (nodeUnits.GetAttributeValue("value", "METRIC") == "METRIC")
                user.Units = Units.Metric;
            else
                user.Units = Units.Imperial;

            // retrieve date format
            var nodeDateFormat = doc.DocumentNode.Descendants("select").FirstOrDefault(
                node => node.HasAttributes && node.Attributes.FirstOrDefault(attr => attr.Value == "data.dateFormat") != null);
            var selectedDateFormatNode = nodeDateFormat.Descendants().First(node => node.HasAttributes && node.Attributes.Contains("selected"));
            var dateFormat = selectedDateFormatNode.Attributes.First(attr => attr.Name == "value").Value;

            var nodeDateSeparator = doc.DocumentNode.Descendants("select").FirstOrDefault(
                node => node.HasAttributes && node.Attributes.FirstOrDefault(attr => attr.Value == "data.dateSeparator") != null);
            var selectedDateSeparatorNode = nodeDateSeparator.Descendants().First(node => node.HasAttributes && node.Attributes.Contains("selected"));
            var separator = selectedDateSeparatorNode.Attributes.First(attr => attr.Name == "value").Value;
            user.DateFormat = dateFormat.Replace(" ", separator);

            _userData = user;
            return user;
        }

        private async Task<HttpClient> CreateHttpClient(bool login = true)
        {
            // Add handler to keep cookies so the connection is persistent and login is avoided if allready logged in.
            HttpClientHandler handler = new HttpClientHandler()
            {
                CookieContainer = this._jar
            };
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            if (login)
            {
                // Attempt login
                var url = BaseUrl + "/index.ftl";
                var strPost = string.Format("email={0}&password={1}&.action=login&tz=0", _userName, _password);

                var encoding = Encoding.UTF8;
                var postData = encoding.GetBytes(strPost);
                var response = await client.PostAsync(url, new StringContent(strPost, encoding, "application/x-www-form-urlencoded"));

                var responseStr = await response.Content.ReadAsStringAsync();
                //var responseStr = postRequest(url, strPost);

                if (responseStr == null || responseStr.Length == 0)
                    throw new PPTException("Failed to login to PolarPersonalTrainer.com");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(responseStr);

                if (doc.GetElementbyId("ico-logout") == null)
                    throw new PPTException("Unable to login to PolarPersonalTrainer.com using the provided credentials");
            }

            return client;
        }

        public async Task<IEnumerable<PPTExercise>> GetExercises(DateTime startDate, DateTime endDate)
        {
            // Attempt login
            var client = await CreateHttpClient();

            // retrieve user settings
            var settings = await GetUser();

            // Attempt to get the list of training sessions for the requested dates
            var url = string.Format("{0}/user/calendar/inc/listview.ftl?startDate={1}&endDate={2}", BaseUrl,
                startDate.ToString(settings.DateFormat), endDate.ToString(settings.DateFormat));
            var response = await client.GetAsync(url);
            var responseStr = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(responseStr);

            var calItems = doc.GetElementbyId("calItems");

            if (calItems == null)
                throw new PPTException("No diary items found in the selected timeframe");

            var strPost = GetTrainingSessions(calItems);

            if (strPost == null)
                throw new PPTException("No complete training sessions found");

            // Attempt to export the XML file for the excercises found above
            url = BaseUrl + "/user/calendar/index.jxml";
            response = await client.PostAsync(url, new StringContent(strPost, Encoding.UTF8, "application/x-www-form-urlencoded"));
            var responseStream = await response.Content.ReadAsStreamAsync();
            var element = XElement.Load(responseStream);
            var reader = element.CreateReader();
            reader.MoveToContent();

            return PPTExtract.ConvertXmlToExercises(element);
        }

        public async Task<GpxExercise> GetGpsData(PPTExercise exercise)
        {
            // retrieve user settings
            var user = await GetUser();

            // Attempt to get the list of training sessions for the requested dates
            var client = await CreateHttpClient();
            var url = string.Format("{0}/user/calendar/inc/listview.ftl?startDate={1}&endDate={2}", BaseUrl,
                exercise.StartTime.ToString(user.DateFormat), exercise.StartTime.ToString(user.DateFormat));
            var response = await client.GetAsync(url);
            var responseStr = await response.Content.ReadAsStringAsync();

            var gpxId = System.Text.RegularExpressions.Regex.Match(responseStr, @"analyze\.ftl\?id=(\d+)").Groups[1].Value;

            if (gpxId == null)
                throw new PPTException("No diary items found in the selected timeframe");

            var keyValues = new Dictionary<string, string>
            {
                {".action", "gpx"},
                {"items.0.item", gpxId},
                {"items.0.itemType", "OptimizedExercise"}
            };

            string postStr = String.Join("&", keyValues.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)));

            // Attempt to export the XML file for the excercises found above
            url = BaseUrl + "/user/calendar/index.gpx";
            response = await client.PostAsync(url, new StringContent(postStr, Encoding.UTF8, "application/x-www-form-urlencoded"));

            // Test if GPS data does exist
            if (response.RequestMessage.RequestUri.Query.Contains("nothing_to_export"))
                throw new PPTException("GPS data for PPT exercise: " + gpxId + " does not exist");

            var responseStream = await response.Content.ReadAsStreamAsync();
            var element = XElement.Load(responseStream);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(GpxExercise));
            var ex = (GpxExercise)serializer.Deserialize(element.CreateReader());
            return ex;
        }
    }
}
