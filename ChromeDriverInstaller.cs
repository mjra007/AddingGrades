using AddinGrades;
using Microsoft.Win32;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


#nullable enable
public class ChromeDriverInstaller
{
    private static readonly HttpClient httpClient = new HttpClient()
    {
        BaseAddress = new Uri("https://googlechromelabs.github.io/chrome-for-testing/")
    };

    public string CachedChromeVersion;

    public Task Install() => this.Install((string)null, false);

    public Task Install(string chromeVersion) => this.Install(chromeVersion, false);

    public Task Install(bool forceDownload) => this.Install((string)null, forceDownload);

    public async Task Install(string chromeVersion, bool forceDownload)
    { 
        if (chromeVersion == null)
            chromeVersion = await this.GetChromeVersion();
        chromeVersion = chromeVersion.Substring(0, chromeVersion.LastIndexOf('.'));
        HttpResponseMessage async1 = await ChromeDriverInstaller.httpClient.GetAsync("LATEST_RELEASE_" + chromeVersion);
        if (!async1.IsSuccessStatusCode)
        {
            if (async1.StatusCode == HttpStatusCode.NotFound)
                throw new Exception("ChromeDriver version not found for Chrome version " + chromeVersion);
            throw new Exception($"ChromeDriver version request failed with status code: {async1.StatusCode}, reason phrase:  {async1.ReasonPhrase}");
        }
        string chromeDriverVersion = await async1.Content.ReadAsStringAsync();
        string zipName;
        string driverName;
        string os;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            os = "win32";
            zipName = "chromedriver-win32.zip";
            driverName = "chromedriver.exe";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            os = "linux64";
            zipName = "chromedriver-linux64.zip";
            driverName = "chromedriver";
        }
        else
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                throw new PlatformNotSupportedException("Your operating system is not supported.");
            os = "mac64";
            zipName = "chromedriver-mac64.zip";
            driverName = "chromedriver";
        }
        string targetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)?? "", chromeVersion);
        string chromeDrivertargetPath = Path.Combine(targetPath, driverName);
        string error;
        if (!forceDownload && File.Exists(chromeDrivertargetPath))
        {
            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = chromeDrivertargetPath,
                ArgumentList = {
                    "--version"
                },
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });

            try
            {
                string existingChromeDriverVersion = await process.StandardOutput.ReadToEndAsync();
                error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                process.Kill(true);
                existingChromeDriverVersion = existingChromeDriverVersion.Split(" ")[1];
                if (chromeDriverVersion == existingChromeDriverVersion)
                {
                    chromeDriverVersion = (string)null;
                    zipName = (string)null;
                    driverName = (string)null;
                    os = (string)null;
                    targetPath = (string)null;
                    chromeDrivertargetPath = (string)null;
                    return;
                }
                if (!string.IsNullOrEmpty(error))
                    throw new Exception("Failed to execute " + driverName + " --version");
            }
            finally
            {
                process?.Dispose();
            }
        }
        HttpResponseMessage downloadZip = await httpClient.GetAsync($"https://edgedl.me.gvt1.com/edgedl/chrome/chrome-for-testing/{chromeDriverVersion}/{os}/{zipName}");

        if (!downloadZip.IsSuccessStatusCode)
            throw new Exception($"ChromeDriver download request failed with status code: {downloadZip.StatusCode}, reason phrase: {downloadZip.ReasonPhrase}");

        if (!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);
        using (Stream zipFileStream = await downloadZip.Content.ReadAsStreamAsync())
        {
            using ZipArchive zipArchive = new(zipFileStream, ZipArchiveMode.Read);
            using FileStream chromeDriverWriter = new(chromeDrivertargetPath, FileMode.Create);
            using Stream chromeDriverStream = zipArchive.GetEntry(zipName.Replace(".zip", "") + "/chromedriver.exe")
                .Open();
            await chromeDriverStream.CopyToAsync((Stream)chromeDriverWriter);
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))

        {
            var process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = "chmod",
                    ArgumentList = { "+x", targetPath },
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            );
            try
            {
                error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                process.Kill(true);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception("Failed to make chromedriver executable");
            }
            finally
            {
                process?.Dispose();
            }
        }
    }

    public async Task<string> GetChromeVersion()
    {
        if (CachedChromeVersion is not null) return CachedChromeVersion;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var chromePath = (string)Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", null,
                null);
            if (chromePath == null)
            {
                throw new Exception("Google Chrome not found in registry");
            }

            var fileVersionInfo = FileVersionInfo.GetVersionInfo(chromePath); 
            CachedChromeVersion = fileVersionInfo.FileVersion;
            return fileVersionInfo.FileVersion;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            try
            {
                using var process = Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "google-chrome",
                        ArgumentList = { "--product-version" },
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    }
                );
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                process.Kill(true);

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
                CachedChromeVersion = output;
                return output;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred trying to execute 'google-chrome --product-version'", ex);
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            try
            {
                using var process = Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome",
                        ArgumentList = { "--version" },
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    }
                );
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                process.Kill(true);

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }

                output = output.Replace("Google Chrome ", ""); 
                CachedChromeVersion = output;
                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occurred trying to execute '/Applications/Google Chrome.app/Contents/MacOS/Google Chrome --version'",
                    ex
                );
            }
        }

        throw new PlatformNotSupportedException("Your operating system is not supported.");
    }

    public static ChromeDriver SetupChromeDriver(bool headless = true)
    {
        Console.WriteLine("Setting up chromedriver");
        ChromeDriverInstaller chromeDriverInstaller = new();
        string chromeVersion = chromeDriverInstaller.GetChromeVersion().Result; 
        string targetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", chromeVersion.Substring(0, chromeVersion.LastIndexOf('.')));
        string chromeDrivertargetPath = Path.Combine(targetPath, "chromedriver.exe");
        if (File.Exists(chromeDrivertargetPath) is false)
        {
            Program.LoggerPanel.WriteLineToPanel("It looks like chromedriver is not installed... This might take a while!");
            Program.LoggerPanel.WriteLineToPanel($"Installing chromedriver {chromeVersion}");
            chromeDriverInstaller.Install().Wait();
        }
        string result = chromeDriverInstaller.GetChromeVersion().Result;
        string driverPath = Path.Combine(Environment.CurrentDirectory, result.Substring(0, result.LastIndexOf('.')), "chromedriver.exe");
        ChromeOptions options = new ChromeOptions();
        ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService(driverPath);
        if (headless)
        {
            defaultService.HideCommandPromptWindow = true;
            options.AddArguments(nameof(headless));
        }
        return new ChromeDriver(defaultService, options);
    }
}
