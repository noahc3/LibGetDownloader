using System;
using System.Collections.Generic;
using CommandLine;
using System.Linq;
using LibGetDownloader;


namespace LibGetDownloaderCli {
    class Program {
        static void Main(string[] args) {
            Parser.Default.ParseArguments<ListOptions, InfoOptions, GetOptions>(args).MapResult(
                (ListOptions options) => {
                    Console.WriteLine("Repo URL: " + options.Repository);
                    Repo repo = Repo.GetRepo(options.Repository);

                    if (options.Categories.Contains("all")) {
                        foreach (Package k in repo.Packages) {
                            Console.WriteLine(k.ToString());
                        }
                    } else {
                        foreach (Package k in repo.Packages.Where(x => options.Categories.Contains(x.Category.ToString()))) {
                            Console.WriteLine(k.ToString());
                        }
                    }
                    return 0;
                },
                (InfoOptions options) => {
                    Console.WriteLine("Repo URL: " + options.Repository);
                    Repo repo = Repo.GetRepo(options.Repository);

                    if (String.IsNullOrWhiteSpace(options.Package)) {
                        Console.WriteLine("Specify a package (-p) to display info for.");
                        return 1;
                        
                    } else if (!repo.PackageExists(options.Package)) {
                        Console.WriteLine("Package \"" + options.Package + "\" doesn't exist in the repository!");
                        return 1;
                    } else {
                        Console.WriteLine(repo.GetPackage(options.Package).ToDetailedString());
                        return 0;
                    }
                },
                (GetOptions options) => {
                    Console.WriteLine("Repo URL: " + options.Repository);
                    Repo repo = Repo.GetRepo(options.Repository);

                    if (String.IsNullOrWhiteSpace(options.Package)) {
                        Console.WriteLine("Specify a package (-p) to display info for.");
                        return 1;
                    } else if (String.IsNullOrWhiteSpace(options.Directory)) {
                        Console.WriteLine("Specify a directory (-d) to download and (if selected) extract the package to.");
                        return 1;
                    } else if (!repo.PackageExists(options.Package)) {
                        Console.WriteLine("Package \"" + options.Package + "\" doesn't exist in the repository!");
                        return 1;
                    } else {
                        repo.DownloadPackageToDisk(options.Package, options.Directory, options.Extract);
                        Console.WriteLine("Downloaded " + options.Package + " successfully!");
                        return 0;
                    }
                },
                (errs) => 1);


        }
    }

    [Verb("list", HelpText = "List all packages in a repository.")]
    public class ListOptions {
        [Option('r', "repo", Required = true, HelpText = "libget repository to use.")]
        public string Repository { get; set; }

        [Option('c', "categories", HelpText = ("Semicolon separated list of categories to include."), Default = new string[] { "all" }, Separator = ';')]
        public IEnumerable<string> Categories { get; set; }
    }

    [Verb("info", HelpText = "Display detailed information on a specified package from a repository.")]
    public class InfoOptions {
        [Option('r', "repo", Required = true, HelpText = "libget repository to use.")]
        public string Repository { get; set; }

        [Option('p', "package", Required = true, HelpText = "List information on a given package name.", Default = null)]
        public string Package { get; set; }
    }

    [Verb("get", HelpText = "Get (download) a specified package from a repository.")]
    public class GetOptions {
        [Option('r', "repo", Required = true, HelpText = "libget repository to use.")]
        public string Repository { get; set; }

        [Option('p', "package", Required = true, HelpText = "List information on a given package name.", Default = null)]
        public string Package { get; set; }

        [Option('d', "directory", Required = true, HelpText = "File path to download a zip to, or a directory to extract files to. ", Default = null)]
        public string Directory { get; set; }

        [Option('e', "extract", HelpText = "Extract downloaded package ZIP to specified directory.", Default = false)]
        public bool Extract { get; set; }
    }


    
}
