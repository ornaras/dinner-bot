using DinnerBot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace DinnerBot.Constants;

internal static class Keyboards
{
    public readonly static ReplyKeyboardMarkup kbStart = new(new KeyboardButton("Открыть каталог"));

    public static InlineKeyboardMarkup GenerateCategoryKeyboard(Category category)
    {
        var markup = new InlineKeyboardMarkup();

        var buttons = new InlineKeyboardButton[category.Plates.Count];
        for (var i = 0; i < buttons.Length; i++)
            buttons[i] = new InlineKeyboardButton($"{i + 1}", $"addPlate{category.Plates[i].Id}");
        foreach (var chunk in buttons.Chunk(3))
            markup.AddNewRow(chunk);
        return markup;
    }
}
