using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SharedLibs
{
    public class ScoreHttpRequest
    {
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