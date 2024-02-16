using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using System.Threading;
using File = System.IO.File;
using Telegram.Bot.Types.ReplyMarkups;
using System.ComponentModel.Design;
using Newtonsoft.Json;
using System;

namespace OnlineMarketbot
{
    public class TelegramBotHandler
    {
        public string Token { get; set; }
        public object currenttime = DateTime.Now.ToString("HH:mm");

        public TelegramBotHandler(string token)
        {
            this.Token = token;
        }

        public async Task BotHandle()
        {
            var botClient = new TelegramBotClient($"{this.Token}");

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
                );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();

        }







        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;


            var chatId = message.Chat.Id;

            {
                var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                            new KeyboardButton[] { "Telefonlar ro'yxati", "Savatchaga qo'shish" },
                            new KeyboardButton[] { "Savatchani ko'rish", "Savatchani tozalash" },
                            new KeyboardButton[] { "Buyurtma qilish", "Biz bilan bo'lanish"},
                        })
                {
                    ResizeKeyboard = true
                };

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Biroz kuting",
                    replyMarkup: replyKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            if (message.Text == "Telefonlar ro'yxati") 
            {
                await SeeListPhone(update.Message.From, chatId, botClient, cancellationToken);
            }

            if(message.Text == "Savatchaga qo'shish")
            {
                await AddToBasket(update.Message.From, chatId, botClient, cancellationToken);
            }




        }


        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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

        private static async Task RegisterUserAsync(Telegram.Bot.Types.User user, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
        }

        private async Task SeeListPhone(Telegram.Bot.Types.User user, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            /* string filepath = "C:\\Users\\VICTUS\\Desktop\\.Net\\OnlineMarketbot\\phones.txt";
             string fileContent = File.ReadAllText(filepath);

             await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: fileContent,
                    cancellationToken: cancellationToken);
             string json = File.ReadAllText(filepath);*/


            string filePath = "C:\\Users\\VICTUS\\Desktop\\.Net\\OnlineMarketbot\\userss.json";
            var service = new GenericCRUDService<Phones>(filePath);

            var allPersons = service.Read();


            foreach (var i in  allPersons)
            {
                await Console.Out.WriteLineAsync(i.Id + ' ' + i.Name + ' ' + i.Salary);
                await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text:  i.Id + "  " + i.Name + "  " + i.Salary + " $",
                        cancellationToken: cancellationToken);
                }
           


        }
        private async Task AddToBasket(Telegram.Bot.Types.User user, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken) {
          
            await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Qaysi mahsulotni savatchaga qo'shmoqchisiz. Shu mahsulotni Id raqamini kiriting",
                    cancellationToken: cancellationToken);

            
            string filePath = "C:\\Users\\VICTUS\\Desktop\\.Net\\OnlineMarketbot\\savatcha.json";
            var service = new GenericCRUDService<Phones>(filePath);


            await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "Savatchaga qo'shildi",
                   cancellationToken: cancellationToken);

        }


    }
        /*private class User
        {
            public string Phone { get; set; }
            public string Password { get; set; }
        }*/

    }
