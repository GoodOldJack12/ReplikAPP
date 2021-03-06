using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using LyokoAPI.Events;

namespace ReplikAPP_Plugin
{
    public class FireBasePush
    {
        public static void SendMessage(string title, string body)
        {
            Task.Run(() =>
            {
                string serverKey = ServerKey.GetServerKey();

                try
                {
                    var result = "-1";
                    var webAddr = "https://fcm.googleapis.com/fcm/send";

                    var regId = Main.GetToken();

                    var httpWebRequest = (HttpWebRequest) WebRequest.Create(webAddr);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\"to\": \"" + regId + "\",\"notification\": {\"title\": \"" + title +
                                      "\",\"body\": \"" + body + "\"}}";
                        streamWriter.Write(json);
                        streamWriter.Flush();
                    }

                    var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                    using (var streamReader =
                        new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        result = streamReader.ReadToEnd();
                    }

                    return result;
                }
                catch (Exception e)
                {
                    LyokoLogger.Log("ReplikAPP", e.ToString());
                    return "SomethingWrong";
                }
            });
        }
    }
}