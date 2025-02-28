
using BlockingCountriesApi.Models.Request;
using BlockingCountriesApi.Models;
using BlockingCountriesApi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using BlockingCountriesApi.Services;
using Polly;
using System.Net;
using BlockingCountriesApi.Interfaces;
using BlockingCountriesApi.Repositories;
using BlockingCountriesApi.Services.BackgroundServices;

namespace BlockingCountriesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddSingleton<IBlockedCountryRepository, BlockedCountryRepository>();
            builder.Services.AddSingleton<ITemporalBlockRepository, TemporalBlockRepository>();
            builder.Services.AddSingleton<ILogRepository, LogRepository>();
            builder.Services.AddSingleton<BlockedCountryService>();
            builder.Services.AddSingleton<TemporalBlockService>();
            builder.Services.AddSingleton<GeoLocationService>();
            builder.Services.AddSingleton<LogService>();

            builder.Services.AddHttpClient<GeoLocationService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["GeoLocation:BaseUrl"]!);
            })
            .AddPolicyHandler(Policy.HandleResult<HttpResponseMessage>(r =>
                r.StatusCode == HttpStatusCode.TooManyRequests
            ).WaitAndRetryAsync(2, _ => TimeSpan.FromSeconds(30)));

            // Background Service
            builder.Services.AddHostedService<TemporalBlockCleanupService>();

            // Validation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddScoped<IValidator<BlockCountryRequest>, BlockCountryRequestValidator>();
            builder.Services.AddScoped<IValidator<TemporalBlockRequest>, TemporalBlockRequestValidator>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
