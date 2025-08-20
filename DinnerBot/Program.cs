using DinnerBot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<CafeteriaExchanger>();

builder.Services.AddSingleton<ITelegramBotClient>
    (new TelegramBotClient(builder.Configuration["TELEGRAM_TOKEN"]!));
builder.Services.AddSingleton<IUpdateHandler, UpdateHandler>();
builder.Services.AddHostedService<PollingService>();

var host = builder.Build();
host.Run();
