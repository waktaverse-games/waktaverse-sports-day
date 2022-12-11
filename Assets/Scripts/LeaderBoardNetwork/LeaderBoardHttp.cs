using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace LeaderBoardNetwork
{
    public class LeaderBoardData
    {
        [JsonConstructor]
        public LeaderBoardData(string title, string type, string id, string name, int score)
        {
            GameTitle = title;
            GameType = type;
            UserId = id;
            UserName = name;
            UserScore = score;
        }

        public LeaderBoardData(string title, string type, string name, int score) : this(title, type, "", name, score)
        {
        }

        public LeaderBoardData(string title, string type, int score) : this(title, type, "", "", score)
        {
        }

        [JsonProperty("game_title")] public string GameTitle { get; set; }
        [JsonProperty("game_type")] public string GameType { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("user_name")] public string UserName { get; set; }
        [JsonProperty("user_score")] public int UserScore { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [Serializable]
    public class RankData
    {
        [JsonConstructor]
        public RankData(string name, int score, int rank)
        {
            userName = name;
            userScore = score;
            ranking = rank;
        }

        [JsonProperty("user_name")] public string userName;
        [JsonProperty("user_score")] public int userScore;
        [JsonProperty("rank")] public int ranking;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public static class LeaderBoardHttp
    {
        private static readonly HttpClient SharedClient = new()
        {
            BaseAddress = new Uri("http://localhost:3000"),
            DefaultRequestHeaders = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") } },
            Timeout = TimeSpan.FromSeconds(20)
        };

        public static async Task<LeaderBoardData[]> GetBoardDataArray(string title, string type, int count)
        {
            var queryStr = BuildQueries(nameof(title), title, nameof(type), type,
                nameof(count), count);
            LeaderBoardData[] result = null;
            try
            {
                var response = await SharedClient.GetStringAsync("/?" + queryStr);
                result = DeserializeBoardData(response);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError(e.Message);
            }
            return result;
        }

        public static async Task<string> PostBoardData(LeaderBoardData data)
        {
            var serializedData = SerializeBoardData(data);
            var content = new StringContent(serializedData, Encoding.UTF8, "application/json");
            string result = null;
            try
            {
                var response = await SharedClient.PostAsync("/", content);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError(e.Message);
            }
            return result;
        }

        public static async Task<RankData[]> GetRankDataArray(string title, string type, int count)
        {
            var queryStr = BuildQueries(nameof(title), title, nameof(type), type,
                nameof(count), count);
            RankData[] result = null;
            try
            {
                var response = await SharedClient.GetStringAsync("/rank/?" + queryStr);
                result = DeserializeRankData(response);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError(e.Message);
            }
            return result;
        }

        public static async Task<RankData> GetUserRankData(string title, string type, string user)
        {
            var queryStr = BuildQueries(nameof(title), title, nameof(type), type);
            RankData result = null;
            try
            {
                var response = await SharedClient.GetStringAsync("/" + user + "/?" + queryStr);
                result = JObject.Parse(response).GetValue("rank")?.ToObject<RankData>();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError(e.Message);
            }
            return result;
        }

        private static string SerializeBoardData(LeaderBoardData data)
        {
            var trimData = data.TrimDBData();
            var str = JsonConvert.SerializeObject(trimData);
            return str;
        }

        private static LeaderBoardData[] DeserializeBoardData(string data)
        {
            var scoresTok = JObject.Parse(data).GetValue("scores");
            if (scoresTok is not { HasValues: true }) return null;
            var scoresJArr = (JArray)scoresTok;
            if (scoresJArr.Count == 0) return null;
            var scoresArr = new LeaderBoardData[scoresJArr.Count];
            for (var i = 0; i < scoresJArr.Count; i++)
            {
                scoresArr[i] = scoresJArr[i].ToObject<LeaderBoardData>();
            }
            return scoresArr;
        }

        private static RankData[] DeserializeRankData(string data)
        {
            var ranksTok = JObject.Parse(data).GetValue("ranks");
            if (ranksTok is not { HasValues: true }) return null;
            var ranksJArr = (JArray)ranksTok;
            if (ranksJArr.Count == 0) return null;
            var ranksArr = new RankData[ranksJArr.Count];
            for (var i = 0; i < ranksJArr.Count; i++)
            {
                ranksArr[i] = ranksJArr[i].ToObject<RankData>();
            }
            return ranksArr;
        }

        private static string BuildQueries(params object[] queries)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            for (var i = 0; i < queries.Length; i += 2) query[queries[i].ToString()] = queries[i + 1].ToString();
            var queryStr = query.ToString();
            return queryStr;
        }

        private static LeaderBoardData TrimDBData(this LeaderBoardData data)
        {
            var fieldsData = new List<FieldInfo>(data.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            foreach (var info in fieldsData)
            {
                if (info.FieldType == typeof(string))
                {
                    info.SetValue(data, TrimDBString(info.GetValue(data)?.ToString()));
                }
            }
            return data;
        }
        
        private static string TrimDBString(string str)
        {
            var trimChars = new[] {' ', '\t', '\n', '\r'};
            var replaceChars = new[]
            {
                "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "=", "+", "[", "]", "{", "}", "\\", "|", ";",
                ":", "'", "\"", ",", "<", ".", ">", "/", "?", "`", "~"
            };
            var sb = new StringBuilder(str.Trim(trimChars));
            sb.Replace(' ', '_');
            foreach (var repChar in replaceChars)
            {
                sb.Replace(repChar, "");
            }

            return sb.ToString().ToLower();
        }
    }
}