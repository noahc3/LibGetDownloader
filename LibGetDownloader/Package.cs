using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http;

namespace LibGetDownloader {
    public class Package {
        public string Name;
        public string Title;
        public string Author;
        public string Version;
        public string Category;
        public string Details;
        public string Description;

        public string Url;
        public string License;
        public string Changelog;

        public string Binary;
        public string Updated;

        public long FileSize;
        public long Extracted;
        
        [JsonProperty(PropertyName = "web_dls")]
        public long WebDownloads;
        [JsonProperty(PropertyName = "app_dls")]
        public long AppDownloads;

        public override string ToString() {
            return $"[{Name}] ({Version}) \"{Title}\" - {Description} - {AppDownloads + WebDownloads} downloads";
        }

        public string ToDetailedString() {
            string[] lines = new string[] {
                $"[{Name}]",
                $"    Title: {Title}",
                $"    Version: {Version}",
                $"    Author: {Author}",
                $"    Category: {Category.ToString()}",
                $"    Description: {Description}",
                $"    URL: {Url}",
                $"    License: {License}",
                $"    Binary: {Binary}",
                $"    File Size: {FileSize}",
                $"    Extracted Size: {Extracted}",
                $"    Web Downloads: {WebDownloads}",
                $"    App Downloads: {AppDownloads}",
            };
            return String.Join(Environment.NewLine, lines);
        }

    }
}
