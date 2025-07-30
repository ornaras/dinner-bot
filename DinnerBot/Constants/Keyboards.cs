using Telegram.Bot.Types.ReplyMarkups;

namespace DinnerBot.Constants;

public static class Keyboards
{
    public readonly static ReplyKeyboardMarkup kbStart = new(new KeyboardButton("Открыть каталог"));
}
