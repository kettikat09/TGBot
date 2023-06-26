using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.VisualBasic;
using System.Net.Http;
using Newtonsoft.Json.Linq;

public class Bot
{
    TelegramBotClient botClient = new TelegramBotClient("6158324212:AAGq2BfBofSTx6khTNlwpa9nZ6AjEY1-EzY");
    CancellationToken cancellationToken = new CancellationToken(); ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
    public async Task Start()
    {
        botClient.StartReceiving(HandlerUpdateAsync, HandlerErrorAsync, receiverOptions, cancellationToken); var botMe = await botClient.GetMeAsync();
        Console.WriteLine($"Бот {botMe.Username} почав працювати"); Console.ReadKey();
    }
    private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update?.Message?.Text!=null)
        {
            await HandlerMessageAsync(botClient, update.Message);
        }
    }
    private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
    {
        if (message.Text.ToLower() == "/start")
        {
            ReplyKeyboardMarkup replyKeyboardMarkup1 = new
                   (new[]
                       {
                            new KeyboardButton[] { "Овен", "Телець", "Близнюки" },
                            new KeyboardButton[] { "Рак", "Лев", "Діва" },
                            new KeyboardButton[] { "Терези", "Скорпіон", "Стрілець" },
                            new KeyboardButton[] { "Козеріг", "Водолій", "Риби" }
                       }
                   )
            {
                ResizeKeyboard = true
            };
            await botClient.SendTextMessageAsync(message.Chat, "Привіт! Зараз я розповім твій гороскоп на сьогодні", replyMarkup: replyKeyboardMarkup1); 
           
            return;
        }
        else
        {
            string swagger = $"https://localhost:7084/api/Kursova/horoscope?zodiac={message.Text}";
            HttpClient httpClient = new HttpClient();
                HttpResponseMessage responseHoroscope = await httpClient.GetAsync(swagger);
            responseHoroscope.EnsureSuccessStatusCode();

                string Response = await responseHoroscope.Content.ReadAsStringAsync();
                Console.WriteLine(Response);

                await botClient.SendTextMessageAsync(message.Chat, Response);
        }
    }
    private Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Помилка в телеграм бот АПІ:\n {apiRequestException.ErrorCode}" + $"\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        //Test test = new Test();                //test.Start();
        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}