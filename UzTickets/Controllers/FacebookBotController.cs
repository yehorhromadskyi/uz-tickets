using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UzTickets.Services;
using static UzTickets.Models.Facebook;
using static UzTickets.Models.UZ;

namespace UzTickets.Controllers
{
    public class FacebookBotController : Controller
    {
        readonly string sendmessage = "https://graph.facebook.com/v2.12/me/messages?access_token={0}";

        readonly string getstation = "https://booking.uz.gov.ua/train_search/station/?term={0}";
        readonly string gettrains = "https://booking.uz.gov.ua/train_search/";

        readonly IConfiguration _configuration;
        readonly ITextRecognitionService _textRecognitionService;

        public FacebookBotController(
            IConfiguration configuration,
            ITextRecognitionService textRecognitionService)
        {
            _configuration = configuration;
            _textRecognitionService = textRecognitionService;
        }

        [HttpGet("message")]
        public IActionResult Subscribe(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.verify_token")] string token,
            [FromQuery(Name = "hub.challenge")] string challenge)
        {
            var query = Request.QueryString;

            if (mode == "subscribe" &&
                token == _configuration["facebookSubscriptionToken"])
            {
                return Json(int.Parse(challenge));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("message")]
        public ActionResult ReceivePost([FromBody] BotRequest request)
        {
            Task.Run(async () =>
            {
                foreach (var entry in request.entry)
                {
                    foreach (var message in entry.messaging)
                    {
                        if (string.IsNullOrWhiteSpace(message?.message?.text))
                            continue;

                        var messageText = message.message.text;

                        var recognizedMessage = await _textRecognitionService.AnalyzeMessageAsync(messageText);

                        var fromStationsResponse = GetRaw(string.Format(getstation, recognizedMessage.From));
                        var fromStations = JsonConvert.DeserializeObject<List<Station>>(fromStationsResponse);
                        var fromStation = fromStations.FirstOrDefault();

                        var toStationsResponse = GetRaw(string.Format(getstation, recognizedMessage.To));
                        var toStations = JsonConvert.DeserializeObject<List<Station>>(toStationsResponse);
                        var toStation = toStations.FirstOrDefault();

                        var trainsResponseSerialized = 
                            PostRaw(gettrains, $"from={fromStation.Value}&to={toStation.Value}&date={DateTime.Today.Year}-{recognizedMessage.Month}-{recognizedMessage.Day}", "x-www-form-urlencoded");

                        var trainsResponse = JsonConvert.DeserializeObject<TrainsResponse>(trainsResponseSerialized);

                        var l = trainsResponse.Data.List.Select(i =>
                            $"{i.Num} {i.From.StationTrain}({i.From.Time}) - {i.To.StationTrain}({i.To.Time})");

                        var result = string.Join("\n\n", l);

                        var messageResponse = new
                        {
                            recipient = new
                            {
                                message.sender.id
                            },
                            message = new
                            {
                                text = result
                            }
                        };

                        PostRaw(string.Format(sendmessage, _configuration["facebookAccessToken"]),
                            JObject.FromObject(messageResponse).ToString());
                    }
                }
            });

            return Ok();
        }

        private string GetRaw(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

        private string PostRaw(string url, string data, string contentType = "json")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = $"application/{contentType}";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(data);
            }

            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
