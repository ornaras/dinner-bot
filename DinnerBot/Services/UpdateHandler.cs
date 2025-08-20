using DinnerBot.Constants;
using DinnerBot.Handlers;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DinnerBot.Services;

public class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger, CacheService cache) : IUpdateHandler
{
    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleError: {Exception}", exception);
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

    private async Task OnMessage(Message msg)
    {
        if (!string.IsNullOrWhiteSpace(msg.Text))
        {
            if (msg.Text.StartsWith('/'))
                await OnCommand(msg);
            else
            {
                if (msg.Text == "Открыть каталог")
                {
                    foreach (var category in await cache.Categories.GetValue())
                    {
                        var images = new IAlbumInputMedia[category.Plates.Count];
                        for (var i = 0; i < images.Length; i++)
                            images[i] = new InputMediaPhoto(InputFile.FromString(category.Plates[i].PictureUrl));
                        foreach (var chunk in images.Chunk(9))
                            await bot.SendMediaGroup(msg.Chat, chunk);

                        var builder = new StringBuilder();
                        builder.AppendLine($"Категория \"<b>{category.Name}</b>\":");
                        for(var i = 1; i <= category.Plates.Count; i++)
                        {
                            var plate = category.Plates[i - 1];
                            builder.AppendLine($"{i}) <code>{plate.Name}</code> <b>[{plate.Price} руб. | {plate.Mass}]</b>");
                        }
                        await bot.SendMessage(msg.Chat, builder.ToString(), ParseMode.Html, replyMarkup: Keyboards.GenerateCategoryKeyboard(category));
                    }
                }
            }
        }
    }

    private async Task OnCommand(Message msg)
    {
        var args = msg.Text![1..].Split(' ');
        await (args[0] switch
        {
            "start" => Commands.StartCommand(msg.Chat),
            _ => Commands.InvalidCommand(msg.Chat, msg.Text, logger),
        });
    }
}