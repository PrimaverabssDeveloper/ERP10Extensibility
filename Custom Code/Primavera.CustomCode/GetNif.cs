using Newtonsoft.Json;
using Primavera.CustomNifService.RootEntity;
using System;
using System.IO;
using System.Net;

namespace Primavera.CustomNifService
{
    internal static class GetNIF
    {
        // Go to www.nif.pt and register your account to get a key.
        const string ApiKey = "<-- YOUR KEY -->";

        public static EntitySupport GetFromNIFPT(string nif)
        {
            string baseUrl = "http://www.nif.pt/?json=1&q={0}&key={1}";

            {
                baseUrl = String.Format(baseUrl, nif, ApiKey);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())

                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonString = reader.ReadToEnd();
                    EntitySupport responseData = JsonConvert.DeserializeObject<EntitySupport>(jsonString);

                    return responseData;
                }
            }
        }
    }
}
