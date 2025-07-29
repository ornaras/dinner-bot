using Telegram.Bot;
using Telegram.Bot.Polling;

namespace DinnerBot.Services;

public class PollingService(ITelegramBotClient botClient,  IUpdateHandler updateHandler, ILogger<PollingService> logger) : BackgroundService
{
    private readonly ReceiverOptions receiverOptions = new() { DropPendingUpdates = true, AllowedUpdates = [] };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Запуск Telegram-бота...");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var me = await botClient.GetMe(stoppingToken);
                logger.LogInformation("Запуск приемника событий {BotName}", me.Username ?? "???");

                await botClient.ReceiveAsync(updateHandler, receiverOptions, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError("Во время работы произошла критическая ошибка: {Exception}", ex);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
