namespace HackatonPDF.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Prescription
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("fontSize", NullValueHandling = NullValueHandling.Ignore)]
        public long? FontSize { get; set; }

        [JsonProperty("textColor", NullValueHandling = NullValueHandling.Ignore)]
        public string TextColor { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Data data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("Name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Age", NullValueHandling = NullValueHandling.Ignore)]
        public string Age { get; set; }

        [JsonProperty("PatientName", NullValueHandling = NullValueHandling.Ignore)]
        public string PatientName { get; set; }

        [JsonProperty("Allowance", NullValueHandling = NullValueHandling.Ignore)]
        public string Allowance { get; set; }

        [JsonProperty("DayCount", NullValueHandling = NullValueHandling.Ignore)]
        public long? DayCount { get; set; }

        [JsonProperty("Gender", NullValueHandling = NullValueHandling.Ignore)]
        public string Gender { get; set; }

        [JsonProperty("Date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Date { get; set; }

        [JsonProperty("Prescription", NullValueHandling = NullValueHandling.Ignore)]
        public string Prescription { get; set; }

        [JsonProperty("Clearance", NullValueHandling = NullValueHandling.Ignore)]
        public string Clearance { get; set; }

        [JsonProperty("DocAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string DocAddress { get; set; }

        [JsonProperty("DocPhone", NullValueHandling = NullValueHandling.Ignore)]
        public string DocPhone { get; set; }

        [JsonProperty("DocEmail", NullValueHandling = NullValueHandling.Ignore)]
        public string DocEmail { get; set; }

        [JsonProperty("DocName", NullValueHandling = NullValueHandling.Ignore)]
        public string DocName { get; set; }

        [JsonProperty("DocTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string DocTitle { get; set; }

        [JsonProperty("DocSign", NullValueHandling = NullValueHandling.Ignore)]
        public string DocSign { get; set; }

        [JsonProperty("EmailList", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> EmailList { get; set; }
    }

    public partial class Prescription
    {
        public static Prescription FromJson(string json) => JsonConvert.DeserializeObject<Prescription>(json, HackatonPDF.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Prescription self) => JsonConvert.SerializeObject(self, HackatonPDF.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}

