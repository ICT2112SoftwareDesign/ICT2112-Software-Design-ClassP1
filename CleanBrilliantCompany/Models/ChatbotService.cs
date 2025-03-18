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

        public string submitQuery(string query)
        {
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsonKeyPath);
            return DetectIntent(projectId, "session123", query);
        }

        public static string DetectIntent(string projectId, string sessionId, string userMessage)
        {
            SessionsClient client = SessionsClient.Create();
            SessionName session = new SessionName(projectId, sessionId);
            
            TextInput textInput = new TextInput { Text = userMessage, LanguageCode = "en" };
            QueryInput queryInput = new QueryInput { Text = textInput };
            DetectIntentResponse response = client.DetectIntent(session, queryInput);

            return response.QueryResult.FulfillmentText; // This is the bot's reply
        }
    }
}