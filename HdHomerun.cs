using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace HdHomerunM3U
{
    /// <summary>
    /// Wrapper around calls to get information about and from Silicon Dust HSHomerun devices.
    /// </summary>
    public static class HdHomerun
    {
        /// <summary>
        /// Get and return the list of homerun devices found on the network.
        /// </summary>
        /// <returns>List of HdHomerunDevice objects</returns>
        public static List<HdHomerunDevice> GetHdHomerunDeviceList()
        {
            string json = string.Empty;

            try
            {
                Uri uri = new Uri("http://my.hdhomerun.com/discover");
                HttpClient http = new HttpClient();
                byte[] result = http.GetByteArrayAsync(uri).Result;

                json = Encoding.UTF8.GetString(result);
                return JsonConvert.DeserializeObject<List<HdHomerunDevice>>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception response from homerun device: " + e.Message);
                Console.WriteLine(json);
                throw;
            }
        }

        /// <summary>
        /// Gets information on a specific homerun device.
        /// </summary>
        /// <param name="device">Current device object</param>
        /// <returns>HdHomerunDiscovery object</returns>
        public static HdHomerunDiscovery GetHdHomerunDiscovery(this HdHomerunDevice device)
        {
            string json = string.Empty;

            try
            {
                Uri uri = new Uri(device.DiscoverURL);
                HttpClient http = new HttpClient();
                byte[] result = http.GetByteArrayAsync(uri).Result;
                json = Encoding.UTF8.GetString(result);
                return JsonConvert.DeserializeObject<HdHomerunDiscovery>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception response from homerun discovery: " + e.Message);
                Console.WriteLine(json);
                throw;
            }
        }

        /// <summary>
        /// Get and return the list of channels found on the specified homerun device.
        /// </summary>
        /// <param name="discovery">Current discovery object</param>
        /// <returns></returns>
        public static List<HdHomerunChannel> GetHdHomerunChannelList(this HdHomerunDiscovery discovery)
        {
            string json = string.Empty;

            try
            {
                //  Uri uri = new Uri("http://my.hdhomerun.com/api/guide.php?DeviceAuth=" + discovery.DeviceAuth);
                //string url = discovery.BaseURL.Substring(0, discovery.BaseURL.LastIndexOf(':'));
                //Uri uri = new Uri(url + "/lineup.json");
                Uri uri = new Uri(discovery.LineupURL);
                HttpClient http = new HttpClient();
                byte[] result = http.GetByteArrayAsync(uri).Result;
                json = Encoding.UTF8.GetString(result);
                //using (StreamWriter file = File.CreateText(@"C:\Stuff\HdHomerunM3U\channels.json"))
                //{
                //    file.Write(JsonPrettify(json));
                //}
                return JsonConvert.DeserializeObject<List<HdHomerunChannel>>(json);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception processing response from homerun get channel list: " + e.Message);
                Console.WriteLine(json);
                throw;
            }
        }

        /// <summary>
        /// Indents and adds line breaks etc to make it pretty for printing
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Newtonsoft.Json.Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }
    }

    public class HdHomerunDiscovery
    {
        public string BaseURL { get; set; }

        public string DeviceAuth { get; set; }

        public string DeviceID { get; set; }

        public string FirmwareName { get; set; }

        public string FirmwareVersion { get; set; }

        public string FriendlyName { get; set; }

        public string LineupURL { get; set; }

        public string ModelNumber { get; set; }

        public int TunerCount { get; set; }
    }

    public class HdHomerunDevice
    {
        public string BaseURL { get; set; }

        public string DeviceID { get; set; }

        public string DiscoverURL { get; set; }

        public string LineupURL { get; set; }

        public string LocalIP { get; set; }
    }

    public class HdHomerunChannel
    {
        public string GuideNumber { get; set; }

        public string GuideName { get; set; }
        public string HD { get; set; }
        public string Favorite { get; set; }

        public string URL { get; set; }
    }
}
