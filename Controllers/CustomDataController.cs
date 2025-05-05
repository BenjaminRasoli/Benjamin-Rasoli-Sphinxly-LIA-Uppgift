using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Easyweb.Controllers;

public class CustomDataController : Controller
{

    [HttpGet("/movies")]
    public async Task<IActionResult> Movies()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync("https://swapi.py4e.com/api/films/?format=json");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound("Could not fetch movie list.");
        }
        var json = await response.Content.ReadAsStringAsync();
        var movies = JsonConvert.DeserializeObject<StarWarsResponse>(json);

        return View("~/Views/Pages/movies.cshtml", movies);
    }

    [HttpGet("/movie/{id}")]
    public async Task<IActionResult> Movie(int id)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync($"https://swapi.py4e.com/api/films/{id}/?format=json");

        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction("Movies"); 
        }

        var json = await response.Content.ReadAsStringAsync();
        var movie = JsonConvert.DeserializeObject<StarWarsEntity>(json);

        return View("~/Views/customData/movie.cshtml", movie);
    }

    [HttpGet("/movie")]
    public IActionResult RedirectToMovieList()
    {
        return Redirect("/movies");
    }
}
