using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using DotNetEnv;
using System.Text.RegularExpressions;

namespace SomeDumbBot
{
  class Program
  {
    static void Main(string[] args)
    {
      Env.Load();

      var botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");

      if (botToken == null) return;

      var bot = new TelegramBotClient(botToken);
      bot.StartReceiving(Update, Error);
      Console.ReadKey();
    }

    private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
      if (update.Type == UpdateType.Message && update.Message?.Text != null)
      {
        await HandleMessage(botClient, update.Message);
      }
      else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
      {
        await HandleCallbackQuery(botClient, update.CallbackQuery);
      }
    }

    private static async Task HandleCallbackQuery(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery)
    {
      if (callbackQuery.Message == null) return;

      switch (callbackQuery.Data)
      {
        case "integral":
          await SendAlbum(telegramBotClient, callbackQuery.Message.Chat.Id, "1_TmW1-k_uMhRlsWJQSo3e8r3obWaI4Lg", "1jCmc64x92n1yl-jKEbp4coCi81R2Rg5L", "1bu_kJIqT1-auY_CnCmy0lT_a7ghu1Ndd");
          return;

        case "derivative":
          await SendPhoto(telegramBotClient, callbackQuery.Message.Chat.Id, "1jfuoznsb-THDrkay4JKcVscrih461JYk", "Таблиця похідних");
          return;

        case "backToMenu":
          await telegramBotClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Прєт 👋. Я можу кинути тобі формулки з матану 🤓☝️. Для цього введи команду /formula");
          return;

        default:
          await telegramBotClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Нема такої опції");
          return;
      }
    }

    private static async Task HandleMessage(ITelegramBotClient botClient, Message message)
    {
      if (message.Text == null) return;

      switch (message.Text.ToLower())
      {
        case "/start":
          await SendMainMenu(botClient, message.Chat.Id);
          return;

        case "/formula":
          await ChooseOptionInline(botClient, message.Chat.Id);
          return;

        default:
          await botClient.SendTextMessageAsync(message.Chat.Id, "Unknown option");
          return;
      }
    }

    private static async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
      throw new NotImplementedException();
    }

    private static async Task SendPhoto(ITelegramBotClient telegramBotClient, long chatId, string fileId, string caption)
    {
      string url = $"https://drive.google.com/uc?export=view&id={fileId}";
      var inputFile = InputFile.FromUri(url);
      await telegramBotClient.SendPhotoAsync(chatId, inputFile, caption: caption);
      Console.WriteLine("закинув");
    }

    private static async Task ChooseOptionInline(ITelegramBotClient telegramBotClient, long chatId)
    {
      var buttons = new InlineKeyboardButton[][]
      {
        [
          InlineKeyboardButton.WithCallbackData("Таблиця інтегралів ∫", "integral"),
          InlineKeyboardButton.WithCallbackData("Таблиця похідних 📈", "derivative"),
        ],
        [
          InlineKeyboardButton.WithCallbackData("Назад до меню 🔙", "backToMenu")
        ]
      };

      await telegramBotClient.SendTextMessageAsync(chatId, "Яку саме формулу тобі треба? 📝",
      replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    private static async Task SendMainMenu(ITelegramBotClient telegramBotClient, long chatId)
    {
      await telegramBotClient.SendTextMessageAsync(chatId, "Прєт 👋. Я можу кинути тобі формулки з матану 🤓☝️. Для цього введи команду /formula");
    }

    private static async Task SendAlbum(ITelegramBotClient telegramBotClient, long chatId, params string[] filesId)
    {
      IAlbumInputMedia[] albumInputMedia = new IAlbumInputMedia[filesId.Length];
      for (int i = 0; i < filesId.Length; i++)
      {
        albumInputMedia[i] = new InputMediaPhoto(new InputFileUrl($"https://drive.google.com/uc?export=view&id={filesId[i]}"))
        {
          Caption = $"ст. {i + 1}"
        };
      }

      await telegramBotClient.SendMediaGroupAsync(chatId, albumInputMedia);
    }
  }
}
