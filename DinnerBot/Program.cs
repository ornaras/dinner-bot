using DinnerBot.Services;
using Telegram.Bot.Polling;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IUpdateHandler, UpdateHandler>();
builder.Services.AddHostedService<PollingService>();

var host = builder.Build();
host.Run();
