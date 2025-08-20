using DinnerBot.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DinnerBot.Handlers;

public static class Commands
{
    public static ITelegramBotClient BotClient
    {
        get => _botClient;
        set
        {
            if (_botClient == null!)
                _botClient = value;
        }
    }
    private static ITelegramBotClient _botClient = null!;

    public static async Task StartCommand(ChatId chat)
    {
        await BotClient.SendMessage(chat, "Вы переведены в главное меню", replyMarkup: Keyboards.kbStart);
    }

    public static async Task InvalidCommand(ChatId chat, string command, ILogger logger)
    {
        logger.LogDebug("В чате {chat} ввели неизвестную команду \"{command}\"", chat.Identifier, command);
        await BotClient.SendMessage(chat, "Неизвестная команда");
    }
}
