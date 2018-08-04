using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    public class Luis
    {
        public static async Task<string> MakeRequest(string uttrance)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // This app ID is for a public sample app that reco gnizes requests to turn on and turn off lights
            var luisAppId = "4acfac1e-f1cb-4fcf-a477-781479420e2f";
            var subscriptionKey = "42fc3ce3a4e94df682de90cbd99658bc";

            // The request header contains your subscription key
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // The "q" parameter contains the utterance to send to LUIS
            queryString["q"] = uttrance;

            // These optional request parameters are set to their default values
            queryString["timezoneOffset"] = "0";
            queryString["verbose"] = "true";
            queryString["spellCheck"] = "false";
            queryString["staging"] = "false";

            var uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + luisAppId + "?" + queryString;
            var response = await client.GetAsync(uri);

            var strResponseContent = await response.Content.ReadAsStringAsync();

            // Return the JSON result from LUIS
            JObject json = JObject.Parse(strResponseContent.ToString());
            JObject topScoringIntent = JObject.Parse(json.GetValue("topScoringIntent").ToString());

            return topScoringIntent.GetValue("intent").ToString();
        }
    }
}