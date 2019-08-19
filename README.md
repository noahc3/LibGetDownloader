# LibGetDownloader

This is a .NET Standard library for interfacing with remote libget repositories. This allows easy retrieval of package information and downloading of packages from libget repositories.

For more information on libget, see https://github.com/vgmoose/libget


## Library

The library is created in C# for .NET Standard 2.0. You can use it in any .NET Standard, .NET Core, .NET Framework, etc. project.

### Examples

All examples should have `using LibGetDownloader;`

Create a "Repo" object:

```csharp
Repo repo = Repo.GetRepo("https://switchbru.com/appstore/");
```

Check if a package exists in the repository:

```csharp
if (!repo.PackageExists("Goldleaf")) {
  Console.WriteLine("Package doesn't exist!");
} else {
  Console.WriteLine("Package exists!");
}
```

Get the Package object for a package:

```csharp
Package p = repo.GetPackage("Goldleaf");
```

You can view all available properties [here](https://github.com/noahc3/LibGetDownloader/blob/832ed506ecdf73d08a99979916718b69e5dbf3c9/LibGetDownloader/Package.cs#L10).

Example:

```csharp
// Equivalent to p.ToDetailedString();
string[] lines = new string[] {
    $"[{Name}]",
    $"    Title: {p.Title}",
    $"    Version: {p.Version}",
    $"    Author: {p.Author}",
    $"    Category: {p.Category.ToString()}",
    $"    Description: {p.Description}",
    $"    URL: {p.Url}",
    $"    License: {p.License}",
    $"    Binary: {p.Binary}",
    $"    File Size: {p.FileSize}",
    $"    Extracted Size: {p.Extracted}",
    $"    Web Downloads: {p.WebDownloads}",
    $"    App Downloads: {p.AppDownloads}",
};
Console.WriteLine(String.Join(Environment.NewLine, lines));

//Example Output:
//[Goldleaf]
//    Title: Goldleaf
//    Version: 0.6.1
//    Author: XorTroll
//    Category: tool
//    Description: Nintendo Switch title installer & manager
//    URL: https://github.com/XorTroll/Goldleaf/releases
//    License: GPLv3
//    Binary: /switch/Goldleaf/Goldleaf.nro
//    File Size: 4861
//    Extracted Size: 11383
//    Web Downloads: 2436
//    App Downloads: 19983
```

Downloading a Package ZIP to disk without extracting:

```csharp
repo.DownloadPackageToDisk("Goldleaf", "out/Goldleaf.zip");
```

Downloading a Package ZIP to disk and extract:

```csharp
repo.DownloadPackageToDisk("Goldleaf", "out/Goldleaf", true);
```

Downloading a Package ZIP and get a byte[] buffer in memory:

```csharp
byte[] buffer = repo.DownloadPackageToMemory("Goldleaf");
```

## CLI Tool

The CLI tool (LibGetDownloaderCli) is a working implementation of the library in a simple cross-platform .NET Core console application.

```
> LibGetDownloaderCli.exe --help
LibGetDownloaderCli 1.0.0
Copyright (C) 2019 noahc3
  list       List all packages in a repository.
  info       Display detailed information on a specified package from a repository.
  get        Get (download) a specified package from a repository.
  help       Display more information on a specific command.
  version    Display version information.
```

```
> LibGetDownloaderCli.exe list --help
LibGetDownloaderCli 1.0.0
Copyright (C) 2019 noahc3
  -r, --repo          Required. libget repository to use.
  -c, --categories    (Default: all) Semicolon separated list of categories to include.
  --help              Display this help screen.
  --version           Display version information.
```

```
> LibGetDownloaderCli.exe info --help
LibGetDownloaderCli 1.0.0
Copyright (C) 2019 noahc3
  -r, --repo       Required. libget repository to use.
  -p, --package    Required. List information on a given package name.
  --help           Display this help screen.
  --version        Display version information.
```

```
> LibGetDownloaderCli.exe get --help
LibGetDownloaderCli 1.0.0
Copyright (C) 2019 noahc3
  -r, --repo         Required. libget repository to use.
  -p, --package      Required. List information on a given package name.
  -d, --directory    Required. File path to download a zip to, or a directory to extract files to.
  -e, --extract      (Default: false) Extract downloaded package ZIP to specified directory.
  --help             Display this help screen.
  --version          Display version information.
```



