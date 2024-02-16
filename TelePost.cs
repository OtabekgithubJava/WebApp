using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AttacheBot
{
    public class TelePost
    {
        public async Task MainTelePost()
        {
            TelegramBotClient botClient = new TelegramBotClient("6838046291:AAEn-wEGto5X1aOKcsv8NSZDvGpLLSYFJhM");
            using CancellationTokenSource cts = new();
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token);
            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();
        }
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                if (update.Message is not { } message)
                    return;
                Console.WriteLine($"User -> {message.Chat.FirstName}\nUserId -> {message.Chat.Id}\n\nMessage ->{message.Text}\n\n");

                var handler = update.Type switch
                {

                    UpdateType.Message => ControlBot.EssentialAsyncMessage(botClient, update, cancellationToken),
                    _ => ControlBot.OtherMessage(botClient, update, cancellationToken),
                };
                await handler;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No no no Bro, error >>> {ex.Message}");
            }

        }

        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}