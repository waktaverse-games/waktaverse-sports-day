using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Mono.Cecil;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace SharedLibs
{
    public class ServerScore
    {
        public string GameTitle { get; set; }
        public string GameType { get; set; }
        public string UserName { get; set; }
        public int UserScore { get; set; }
    }
    
    public class ScoreHttpRequest
    {
        private const string BaseUrl = "http://localhost:3000";
        
        private static HttpClient _sharedClient = new()
        {
            BaseAddress = new Uri(BaseUrl),
        };
        
        public static async void SendScore(ServerScore score)
        {
            _sharedClient.BaseAddress = new Uri(BaseUrl);
            _sharedClient.DefaultRequestHeaders.Accept.Clear();
            _sharedClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = JsonConvert.SerializeObject(score, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
            Debug.Log("{\"game_title\":\"" + score.GameTitle + "\",\"game_type\":\"" + score.GameType + "\",\"user_name\":\"" + score.UserName + "\",\"user_score\":" + score.UserScore + "}");;
            Debug.Log(content);
            var response = await _sharedClient.PostAsync("/", new StringContent(content));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var res = await response.Content.ReadAsStringAsync();
                Debug.Log(res);
            }

            // var request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/score");
            // request.Method = "POST";
            // request.ContentType = "application/json";
            // request.Accept = "application/json";
            //
            // using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            // {
            //     string json = JsonConvert.SerializeObject(score);
            //
            //     streamWriter.Write(json);
            //     streamWriter.Flush();
            //     streamWriter.Close();
            // }
            //
            // var httpResponse = (HttpWebResponse)request.GetResponse();
            // using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            // {
            //     var result = streamReader.ReadToEnd();
            // }
        }
        
        public static async void PostScore(string gameTitle, string gameType, string userName, int score)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.BeginGetRequestStream((ar) =>
            {
                var requestStream = request.EndGetRequestStream(ar);
                var writer = new StreamWriter(requestStream);
                writer.Write("{\"game_title\":\"" + gameTitle + "\",\"game_type\":\"" + gameType + "\",\"user_name\":\"" + userName + "\",\"user_score\":" + score + "}");
                writer.Flush();
                requestStream.Close();
                request.BeginGetResponse((ar2) =>
                {
                    var response = (HttpWebResponse)request.EndGetResponse(ar2);
                    var responseStream = response.GetResponseStream();
                    var reader = new StreamReader(responseStream);
                    var responseString = reader.ReadToEnd();
                    responseStream.Close();
                    response.Close();
                }, null);
            }, null);
        }
        
        public static async Task<string> GetScores(string gameTitle, string gameType, int count)
        {
            string result = "";
            var request = (HttpWebRequest)WebRequest.Create($"http://localhost:3000/?title={gameTitle}&type={gameType}&count={count}");
            request.Method = "GET";
            var res = request.BeginGetResponse((ar) =>
            {
                var response = (HttpWebResponse)request.EndGetResponse(ar);
                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);
                var responseString = reader.ReadToEnd();
                result = responseString;
                responseStream.Close();
                response.Close();
            }, null);
            while (res.IsCompleted == false)
            {
                await Task.Delay(100);
            }
            return result;
        }
    }
}