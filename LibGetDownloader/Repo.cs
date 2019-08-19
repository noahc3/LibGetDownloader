using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace LibGetDownloader {
    public class Repo {
        public string Url;
        public Package[] Packages;
        public static Repo GetRepo(string Url) {
            using (HttpClient client = new HttpClient()) {
                string repoJson = client.GetAsync(Url + "repo.json").Result.Content.ReadAsStringAsync().Result;
                Repo repo = JsonConvert.DeserializeObject<Repo>(repoJson);
                repo.Url = Url;
                return repo;
            }
        }

        public bool PackageExists(string packageName) {
            foreach(Package k in Packages) {
                if (k.Name == packageName) return true;
            }

            return false;
        }

        public Package GetPackage(string packageName) {
            foreach(Package k in Packages) {
                if (k.Name == packageName) return k;
            }

            throw new KeyNotFoundException("The package named \"" + packageName + "\" was not present in the repo.");
        }

        public byte[] DownloadPackageToMemory(string packageName) {
            using (HttpClient client = new HttpClient()) {
                Package package = GetPackage(packageName);
                string zipUrl = $"{Url}zips/{package.Name}.zip";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, zipUrl);
                HttpResponseMessage resp = client.SendAsync(request).Result;
                if (!resp.IsSuccessStatusCode) {
                    throw new UrlNotFoundException($"The package zip {packageName} was not found at the URL {zipUrl}.");
                } else {
                    return client.GetAsync(zipUrl).Result.Content.ReadAsByteArrayAsync().Result;
                }
            }
        }

        public void DownloadPackageToDisk(string packageName, string path, bool extract = false) {
            byte[] buffer = DownloadPackageToMemory(packageName);
            
            if (extract) {
                Directory.CreateDirectory(path);
                MemoryStream stream = new MemoryStream(buffer);
                FastZip zip = new FastZip();
                zip.ExtractZip(stream, path, FastZip.Overwrite.Always, x => true, "", "", true, false);
            } else {
                Directory.CreateDirectory(new FileInfo(path).DirectoryName);
                File.WriteAllBytes(path, buffer);
            }
        }
    }

    public class UrlNotFoundException : Exception {
        public UrlNotFoundException(string message) : base(message) {

        }
    }
}
