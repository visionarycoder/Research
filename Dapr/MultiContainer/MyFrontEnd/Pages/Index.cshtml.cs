using Dapr.Client;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyFrontEnd.Pages
{

    public class IndexModel(ILogger<IndexModel> logger, DaprClient daprClient) : PageModel
    {

        private readonly ILogger<IndexModel> logger = logger;
        private readonly DaprClient daprClient = daprClient;

        public async Task OnGet()
        {
            logger.LogDebug("Getting weather forecast data from MyBackEnd");
            var forecasts = await daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(HttpMethod.Get,"MyBackEnd","weatherforecast");
            ViewData["WeatherForecastData"] = forecasts;
            var forecastList = new List<WeatherForecast>(forecasts);
            logger.LogDebug($"Got weather forecast data ({forecastList.Count()}) from MyBackEnd");
        }

    }

}
