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
}
