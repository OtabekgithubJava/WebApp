using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using AttacheBot;

namespace AttacheBot
{
    public class ChannelMessageAttache
    {
        public static async Task CreateButton(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var replyKeyboard = new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("ChannelName\U0001f7e1"),
                        new KeyboardButton("PostText\U0001f7e2"),
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Image\U0001f7e1"),
                        new KeyboardButton("Link\U0001f7e2")
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("⬅️Back"),
                        new KeyboardButton("Save🔵")
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Send channel")
                    }
                })
                {
                    ResizeKeyboard = true,
                };

            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Ok, we shall begin >>> ",
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);
        }
        public static async Task EditButtons(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var replyKeyboard = new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Edit ChannelName🟡"),
                        new KeyboardButton("Edit PostText🟢"),
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Edit Image🟡"),
                        new KeyboardButton("Edit Link🟢")
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("⬅️Back"),
                        new KeyboardButton("Edit Save🔵")
                    }})
                {
                    ResizeKeyboard = true,
                };

            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Ok, Go on Bro >>> ",
                replyMarkup: replyKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}