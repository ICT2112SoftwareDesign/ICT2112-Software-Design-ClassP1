using System;
using CleanBrilliantCompany.Interfaces;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;

namespace CleanBrilliantCompany.Models
{
    public class ChatbotService : IChatbot
    {
        static string projectId = "teak-clone-454005-d5";
        static string jsonKeyPath = @"../CleanBrilliantCompany/teak-clone-454005-d5-5fa367197d61.json"; 

        public (string responseText, Dictionary<string, string> parameters) submitQuery(string query)
        {
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsonKeyPath);
            return detectIntent(projectId, "session123", query);
        }

        public static (string responseText, Dictionary<string, string> parameters) detectIntent(string projectId, string sessionId, string userMessage)
        {
            SessionsClient client = SessionsClient.Create();
            SessionName session = new SessionName(projectId, sessionId);
            
            TextInput textInput = new TextInput { Text = userMessage, LanguageCode = "en" };
            QueryInput queryInput = new QueryInput { Text = textInput };
            DetectIntentResponse response = client.DetectIntent(session, queryInput);

            string chatbotResponse = response.QueryResult.FulfillmentText;
            string intent = response.QueryResult.Intent.DisplayName; // Get detected intent

             // Extract parameters from Dialogflow response
            var parameters = new Dictionary<string, string>();
            foreach (var param in response.QueryResult.Parameters.Fields)
            {
                parameters[param.Key] = param.Value.ToString(); // Convert parameter values to string
            }

            return (chatbotResponse, parameters);
        }
    }
}