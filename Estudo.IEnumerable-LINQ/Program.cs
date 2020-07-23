using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

public class Program
{
    public static void Main()
    {
        var sw = new Stopwatch();
        sw.Start();

        var urls = new string[]
        {
            "https://elemarjr.com/pt",
            "https://eximia.co/pt",
            "https://eximiaco.tech/pt",
            "https://microsoft.com"

        };

        var srcs = urls
            .Select(DownloadContent)
            .Select(GetHtmlDocument)
            .SelectMany(doc => doc.DocumentNode.SelectNodes(@"//img[@src]"))
            .Select(node => node.Attributes["src"].Value);

        foreach (var result in srcs)
        {
            Console.WriteLine($"Time: {sw.ElapsedMilliseconds / 1000.0} seg");
            Console.WriteLine(result);
        }

    }

    public static HtmlDocument GetHtmlDocument(string source)
    {
        HtmlDocument result = new HtmlDocument();
        result.LoadHtml(source);
        return result;
    }

    public static string DownloadContent(string url)
    {
        var client = new WebClient();
        client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        var data = client.OpenRead(url);
        var reader = new StreamReader(data);

        return reader.ReadToEnd();
    }

}