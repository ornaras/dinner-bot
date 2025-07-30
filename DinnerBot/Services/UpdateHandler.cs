using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace DinnerBot.Services;

public class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger) : IUpdateHandler
{
    private static readonly InputPollOption[] PollOptions = ["Hello", "World!"];

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleError: {Exception}", exception);
        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } msg } => OnMessage(msg),
            _ => Task.CompletedTask
        });
    }

    private Task OnMessage(Message msg)
    {
        if (!string.IsNullOrWhiteSpace(msg.Text))
        {
            if (msg.Text.StartsWith('/'))
                return OnCommand(msg);
        }
        return Task.CompletedTask;
    }

    private async Task OnCommand(Message msg)
    {
        var args = msg.Text![1..].Split(' ');
        await (args[0] switch
        {
            "start" => Commands.StartCommand(msg.Chat.Id)
        });
    }
}