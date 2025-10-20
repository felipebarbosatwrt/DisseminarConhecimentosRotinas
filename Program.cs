using DisseminarConhecimentosRotinas.Models;
using DisseminarConhecimentosRotinas.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var executionService = host.Services.GetRequiredService<ExecutionService>();
        await executionService.ExecuteAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                services.Configure<SmtpSettings>(hostContext.Configuration.GetSection("SmtpSettings"));
                services.AddSingleton<FileService>();
                services.AddSingleton<EmailService>();
                services.AddHttpClient<ViaCepService>();
                services.AddTransient<ExecutionService>();
            });
}