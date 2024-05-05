using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindNotes.Functions
{
    internal class Gemini
    {
        #region CGPT Json
        public class jsonstrings
        {
            public List<GeminiSystem> contents;
            public class GeminiSystem
            {
                public string role { get; set; }
                public List<PartsSystem> parts;
                public class PartsSystem
                {
                    public string text { get; set; }
                }

            }
        }
        #endregion

        static string GeminiKey = "";

        public static string SendToGemini(string Request, string HighlightedText)
        {
            jsonstrings data = new jsonstrings
            {
                contents = new List<jsonstrings.GeminiSystem>
                {
                    new jsonstrings.GeminiSystem
                    {
                        role = "user",
                        parts = new List<jsonstrings.GeminiSystem.PartsSystem>
                        {
                            new jsonstrings.GeminiSystem.PartsSystem
                            {
                                text = $"{Request}\n\n{HighlightedText}"
                            }
                        }
                    }
                }
            };

            Clipboard.SetText(JsonConvert.SerializeObject(data));
            Debug.WriteLine("Done!");

            var client = new RestClient($"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={GeminiKey}");

            var request = new RestRequest();

            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", JsonConvert.SerializeObject(data), ParameterType.RequestBody);

            var response = client.Execute(request);

            if (response.IsSuccessful == true)
            {
                dynamic ResponseJSON = JsonConvert.DeserializeObject(response.Content);
                return (string)ResponseJSON.candidates[0].content.parts[0].text;
            }
            else
                return response.Content;

        }
    }
}
