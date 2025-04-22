using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BusinessNewsApp.Models;

public class NewsController : Controller
{
    private readonly HttpClient _httpClient;

    public NewsController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index()
{
    string apiKey = "732934aee2ef451d8ecc9140fd1a6141";
    string url = $"https://newsapi.org/v2/top-headlines?country=us&category=business&apiKey={apiKey}";

    try
    {
        var response = await _httpClient.GetStringAsync(url);
        var apiData = JsonConvert.DeserializeObject<NewsApiResponse>(response);

        var articles = apiData.Articles.Select(a => new NewsArticle
        {
            SourceName = a.Source?.Name,
            Title = a.Title,
            Url = a.Url
        }).ToList();

        return View(articles);
    }
    catch (HttpRequestException ex)
    {
        ViewBag.Error = "Failed to fetch news: " + ex.Message;
        return View(new List<NewsArticle>());
    }
}
}
