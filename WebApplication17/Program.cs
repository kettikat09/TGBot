using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using System;
using Microsoft.OpenApi.Models;

public class Program
{
    static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
        });

        builder.Services.AddScoped<Client>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        });
        Bot bot = new Bot();
        bot.Start();
        List<Tuple<string, long>> outputList = new List<Tuple<string, long>>();
        app.MapControllers();

        await app.RunAsync();
    }
}





