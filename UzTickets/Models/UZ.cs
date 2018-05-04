using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UzTickets.Models
{
    public class UZ
    {
        public class Station
        {
            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("region")]
            public object Region { get; set; }

            [JsonProperty("value")]
            public long Value { get; set; }
        }

        public class TrainsResponse
        {
            [JsonProperty("data")]
            public Data Data { get; set; }
        }

        public class Data
        {
            [JsonProperty("list")]
            public List<List> List { get; set; }
        }

        public class List
        {
            [JsonProperty("num")]
            public string Num { get; set; }

            [JsonProperty("category")]
            public long Category { get; set; }

            [JsonProperty("travelTime")]
            public string TravelTime { get; set; }

            [JsonProperty("from")]
            public From From { get; set; }

            [JsonProperty("to")]
            public From To { get; set; }

            //[JsonProperty("types")]
            //public List<TypeElement> Types { get; set; }

            //[JsonProperty("child")]
            //public Child Child { get; set; }

            [JsonProperty("allowStudent")]
            public long AllowStudent { get; set; }

            [JsonProperty("allowBooking")]
            public long AllowBooking { get; set; }

            [JsonProperty("allowRoundtrip")]
            public long AllowRoundtrip { get; set; }

            [JsonProperty("isCis")]
            public long IsCis { get; set; }

            [JsonProperty("isEurope")]
            public long IsEurope { get; set; }

            [JsonProperty("allowPrivilege")]
            public long AllowPrivilege { get; set; }

            [JsonProperty("noReserve")]
            public long NoReserve { get; set; }
        }

        //public class Child
        //{
        //    [JsonProperty("minDate")]
        //    public DateTimeOffset MinDate { get; set; }
        //
        //    [JsonProperty("maxDate")]
        //    public DateTimeOffset MaxDate { get; set; }
        //}

        public class From
        {
            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("station")]
            public string Station { get; set; }

            [JsonProperty("stationTrain")]
            public string StationTrain { get; set; }

            [JsonProperty("date")]
            public string Date { get; set; }

            [JsonProperty("time")]
            public string Time { get; set; }

            [JsonProperty("sortTime")]
            public long SortTime { get; set; }

            [JsonProperty("srcDate")]
            public DateTimeOffset? SrcDate { get; set; }
        }

        //public class TypeElement
        //{
        //    [JsonProperty("id")]
        //    public string Id { get; set; }
        //
        //    [JsonProperty("title")]
        //    public string Title { get; set; }
        //
        //    [JsonProperty("letter")]
        //    public string Letter { get; set; }
        //
        //    [JsonProperty("places")]
        //    public long Places { get; set; }
        //}
    }
}
