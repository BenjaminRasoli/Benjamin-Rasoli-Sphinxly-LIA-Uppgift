## 🛠️ **Kontroller - Förklaring**

Projektet innehåller en controller, `CustomDataController`, som hanterar API-anrop och rendering av data för Star Wars-filmer. Den är uppdelad i tre huvudsakliga actions:

### 1. **`Movies()` - Visa en lista med alla filmer**

* **Syfte**: Hämtar alla filmer från Star Wars API och visar dem i en lista.
* **Vad den gör**:

  * Gör ett HTTP GET-anrop till Star Wars API för att hämta en lista med filmer.
  * Deserialiserar svaret till en `StarWarsResponse`-modell som innehåller alla filmer.
  * Skickar listan med filmer till vyn `movies.cshtml` för att rendera filminformationen på sidan.

**Kod:**

```csharp
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
```

### 2. **`Movie(int id)` - Visa detaljer om en specifik film**

* **Syfte**: Hämtar detaljer om en enskild film baserat på filmens ID och visar informationen på en separat detaljsida.
* **Vad den gör**:

  * Tar emot ett `id` från URL\:en (t.ex. `/movie/1`).
  * Gör ett HTTP GET-anrop till API\:t för att hämta detaljerad information om en specifik film.
  * Deserialiserar svaret till en `StarWarsEntity`-modell, som innehåller detaljer om filmen (t.ex. titel, regissör, öppningscrawl).
  * Skickar den deserialiserade data till vyn `movie.cshtml` för att visa filmens detaljer på sidan.

**Kod:**

```csharp
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
```

### 3. **Redirect till Filmlista**

* **Syfte**: En fallback-route om användaren försöker besöka en ogiltig eller icke-existerande film.
* **Vad den gör**:

  * Om en användare försöker komma åt en film som inte finns (felaktigt ID), omdirigeras de automatiskt till listan av alla filmer (`/movies`).

**Kod:**

```csharp
[HttpGet("/movie")]
public IActionResult RedirectToMovieList()
{
    return Redirect("/movies");
}
```

## 🎬 **`movies.cshtml` - Förklaring**

**Syfte**: Vyn `movies.cshtml` används för att visa en lista med alla Star Wars-filmer som hämtats från API\:t.

### Vad den gör:

* **Modell**: Vyn tar emot en `StarWarsResponse`-modell, som innehåller en lista (`Results`) med alla filmer.
* **Looping**: För varje film i listan (`Model.Results`), skapar vyn en **movie card** som visar:

### Hur den fungerar:

* **HTML-struktur**: Vyn bygger en HTML-struktur för att visa varje film i en **movie card**. Varje kort innehåller information om filmen och en länk till dess detaljerade vy.
* **Länk**: För varje film genereras en länk (`<a href="/movie/@id">View Details</a>`) som leder till detaljsidan för just den filmen.

**Kodexempel (i `movies.cshtml`):**

```html
@model StarWarsResponse

@if (Model.Results != null && Model.Results.Any())
{
    <div class="movies-container">
        @foreach (var movie in Model.Results)
        {
            var id = movie.url?.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();
            <div class="movie-card">
                <h2>@movie.Title</h2>
                <p><strong>Director:</strong> @movie.Director</p>
                <p><strong>Release:</strong> @movie.release_date</p>
                <a href="/movie/@id">View Details</a>
            </div>
        }
    </div>
}
else
{
    <p>No movies found.</p>
}
```
