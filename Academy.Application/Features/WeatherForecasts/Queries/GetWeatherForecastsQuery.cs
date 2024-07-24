using Academy.Application.Common.Interfaces;
using System.ComponentModel;

namespace Academy.Application.Features.WeatherForecasts.Queries
{
    public record GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecast>>;



    public class GetWeatherForecastsQueryHandler : IRequestHandler<GetWeatherForecastsQuery, IEnumerable<WeatherForecast>>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
        {
            var rng = new Random();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
    }

    public class GenerateRandomBrandRequest : IRequest<string>
    {
        public int NSeed { get; set; }
    }

    public class GenerateRandomBrandRequestHandler : IRequestHandler<GenerateRandomBrandRequest, string>
    {
        private readonly IJobService _jobService;

        public GenerateRandomBrandRequestHandler(IJobService jobService) => _jobService = jobService;

        public Task<string> Handle(GenerateRandomBrandRequest request, CancellationToken cancellationToken)
        {
            string jobId = _jobService.Enqueue<INumberGeneratorJob>(x => x.GenerateAsync(request.NSeed, default));
            //string jobId = _jobService.Schedule<INumberGeneratorJob>(x => x.GenerateAsync(request.NSeed, default), TimeSpan.FromSeconds(5));
            return Task.FromResult(jobId);
        }
    }

    public interface INumberGeneratorJob : IScopedService
    {
        [DisplayName("Generate Random Brand example job on Queue notDefault")]
        Task GenerateAsync(int nSeed, CancellationToken cancellationToken);
    }
}