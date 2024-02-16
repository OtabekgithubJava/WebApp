using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using AttacheBot;

namespace AttacheBot
{
    public class ControlBot
    {
        public static string? PostText;
        public static string? ChannelName;
        public static string? Photo;
        public static string? Link;

        public static bool IsPostText = false;
        public static bool IsChannelName = false;
        public static bool IsPhoto = false;
        public static bool IsLink = false;

        public static async Task EssentialAsyncMessage(ITelegramBotClient botClient, Update? update, CancellationToken cancellationToken)
        {
            Task handler = update.Message.Type switch
            {
                MessageType.Text => TextAsyncFunction(botClient, update, cancellationToken),
                MessageType.Photo => PostPhotoAsyncFunction(botClient, update, cancellationToken),
                _ => OtherMessage(botClient, update, cancellationToken),
            };
        }
        public static async Task TextAsyncFunction(ITelegramBotClient botClient, Update? update, CancellationToken cancellationToken)
        {
            var message = update.Message.Text;
            if (message == "/start")
            {
                IsChannelName = false;
                IsPostText = false;
                IsPhoto = false;
                ChannelName = null; Photo = null; PostText = null;

                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: "\n\n\n\t\tWelcome to PostBot🔴🟡🟢\n\n\n"+
                    "\t\t\tFor creating post you have to click /create_post command >>> ",
                    cancellationToken: cancellationToken);
            }
            else if (message == "/create_post")
            {
                await ChannelMessageAttache.CreateButton(botClient, update, cancellationToken);
            }
            else if (message == "⬅️Back")
            {
                IsChannelName = false;
                IsPostText = false;
                IsPhoto = false;
                ChannelName = null; Photo = null; PostText = null;
            }
            else if (message == "/see")
            {
                await SeePost(botClient, update, cancellationToken);
            }
            else if (message == "/edit")
            {
                if (Photo != null && ChannelName != null && PostText != null)
                {
                    await ChannelMessageAttache.EditButtons(botClient, update, cancellationToken);
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        replyToMessageId: update.Message.MessageId,
                        text: "For editing post, you have to create a new Post!!!",
                        cancellationToken: cancellationToken);
                }
            }
            else if (message == "Send channel")
            {
                if (Photo != null && ChannelName != null && PostText != null)
                {
                    await botClient.SendPhotoAsync(
                        chatId: $"@{ChannelName}",
                        photo: InputFile.FromFileId(Photo),
                        caption: $"{PostText}\nJump in channel >>> @{ChannelName}\n{Link}",
                        cancellationToken: cancellationToken);
                }
            }
            else if (message == "ChannelName🟡" || message == "Edit ChannelName🟡")
            {
                IsChannelName = true;

                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: "Please isert channelname >>> !",
                    cancellationToken: cancellationToken);
            }

            else if (message == "PostText🟢" || message == "Edit PostText🟢")
            {
                IsPostText = true;
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: "Please post a new post >>> !",
                    cancellationToken: cancellationToken);
            }

            else if (message == "Image🟡" || message == "Edit Image🟡")
            {
                IsPhoto = true;
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: "Please send an image >>> !",
                    cancellationToken: cancellationToken);
            }
            else if (message == "Link🟢" || message == "Edit Link🟢")
            {
                IsLink = true;
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: "Please send a link >>> !",
                    cancellationToken: cancellationToken);
            }
            else if (message == "Save🔵" || message == "Edit Save🔵")
            {
                Console.WriteLine($"{IsChannelName} {IsPostText} {IsPhoto}");
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: "👌",
                    cancellationToken: cancellationToken);
            }
            else
            {
                if (IsPostText)
                {
                    PostText = message;
                    IsPostText = false;
                    await botClient.SendTextMessageAsync
                        (
                        chatId: update.Message.Chat.Id,
                        replyToMessageId: update.Message.MessageId,
                        text: "Accepted✅ ",
                        cancellationToken: cancellationToken
                        );
                }
                else if (IsChannelName)
                {
                    ChannelName = message;
                    IsChannelName = false;
                    await botClient.SendTextMessageAsync
                        (
                        chatId: update.Message.Chat.Id,
                        replyToMessageId: update.Message.MessageId,
                        text: "Accepted✅ ",
                        cancellationToken: cancellationToken
                        );
                }
                else if (IsLink)
                {
                    Link = message;
                    IsLink = false;
                    await botClient.SendTextMessageAsync
                        (
                        chatId: update.Message.Chat.Id,
                        replyToMessageId: update.Message.MessageId,
                        text: "Accepted✅ ",
                        cancellationToken: cancellationToken
                        );
                }
                else
                {
                    return;
                }
            }

        }
        public static async Task PostPhotoAsyncFunction(ITelegramBotClient botClient, Update? update, CancellationToken cancellationToken)
        {
            if (IsPhoto)
            {
                Photo = update.Message.Photo.Last().FileId;
                IsPhoto = false;
                await botClient.SendTextMessageAsync
                    (
                    chatId: update.Message.Chat.Id,
                    replyToMessageId: update.Message.MessageId,
                    text: "Accepted✅ ",
                    cancellationToken: cancellationToken
                    );
            }
        }
        public static async Task OtherMessage(ITelegramBotClient botClient, Update? update, CancellationToken cancellationToken)
        {
            return;
        }
        public static async Task SeePost(ITelegramBotClient botClient, Update? update, CancellationToken cancellationToken)
        {
            if (Photo == null)
            {
                if (ChannelName != null && PostText != null)
                {
                    var message = update.Message;
                    await botClient.SendTextMessageAsync
                        (
                        chatId: message.Chat.Id,
                        disableNotification: true,
                        replyToMessageId: message.MessageId,
                        text: $"{PostText}\nJump in channel >>> @{ChannelName}\n{Link}",
                        replyMarkup: new ReplyKeyboardRemove(),
                        cancellationToken: cancellationToken
                        );
                }
            }

            else
            {
                if (ChannelName != null && PostText != null)
                {
                    var message = update.Message;
                    await botClient.SendPhotoAsync
                        (
                        chatId: message.Chat.Id,
                        disableNotification: true,
                        replyToMessageId: message.MessageId,
                        caption: $"{PostText}\nJump in channel >>> @{ChannelName}\n{Link}",
                        photo: InputFile.FromFileId(Photo),
                        replyMarkup: new ReplyKeyboardRemove(),
                        cancellationToken: cancellationToken
                        );
                }
            }
        }
    }
}


//using PostBotChannel;
//using Telegram.Bot;
//using Telegram.Bot.Exceptions;
//using Telegram.Bot.Polling;
//using Telegram.Bot.Types;
//using Telegram.Bot.Types.Enums;

//namespace PostBotTelegram
//{
//    public class ControlBot
//    {
//        public async Task MainServer()
//        {
//            TelegramBotClient botClient = new TelegramBotClient("6770468069:AAH4z3QA1t0bJqOa6HFtFXc8FGLtDDltlc8");
//            using CancellationTokenSource cts = new();
//            ReceiverOptions receiverOptions = new()
//            {
//                AllowedUpdates = Array.Empty<UpdateType>()
//            };
//            botClient.StartReceiving(
//                updateHandler: HandleUpdateAsync,
//                pollingErrorHandler: HandlePollingErrorAsync,
//                receiverOptions: receiverOptions,
//                cancellationToken: cts.Token);
//            var me = await botClient.GetMeAsync();
//            Console.WriteLine($"Start listening for @{me.Username}");
//            Console.ReadLine();
//            cts.Cancel();
//        }
//        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
//        {
//            try
//            {
//                if (update.Message is not { } message)
//                    return;
//                Console.WriteLine($"User -> {message.Chat.FirstName}\nUserId -> {message.Chat.Id}\n\nMessage ->{message.Text}\n\n");

//                var handler = update.Type switch
//                {

//                    UpdateType.Message => Program.EssentialAsyncMessage(botClient, update, cancellationToken),
//                    _ => Program.OtherMessage(botClient, update, cancellationToken),
//                };
//                await handler;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"No no no Bro, error => {ex.Message}");
//            }

//        }

//        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
//        {
//            var ErrorMessage = exception switch
//            {
//                ApiRequestException apiRequestException
//                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
//                _ => exception.ToString()
//            };

//            Console.WriteLine(ErrorMessage);
//            return Task.CompletedTask;
//        }
//    }
//}