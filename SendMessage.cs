using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SomeDumbBot
{
  class SendMessage
  {
    public static async Task SendRandomStickerOrGif(ITelegramBotClient telegramBotClient, long chatId)
    {
      Random random = new();

      int min = 0;
      int max = 100;

      int randomNum = random.Next(min, max);

      switch (randomNum % 2)
      {
        case 0:
          string[]? stickerIds = Environment.GetEnvironmentVariable("Stickers")?.Split(',');

          if (stickerIds == null || stickerIds.Length == 0)
          {
            Console.WriteLine("нема стікерів за таким ім'ям");
            return;
          }

          int stickerIdNumMin = 0;
          int stickerIdNumMax = stickerIds.Length;

          int stickerIdNum = random.Next(stickerIdNumMin, stickerIdNumMax);
          string stickerId = stickerIds[stickerIdNum];

          InputFileId? stickerToSend = new(Environment.GetEnvironmentVariable(stickerId));

          if (stickerToSend == null)
          {
            Console.WriteLine($"немає стікера з ID: {stickerId}");
            return;
          }

          await telegramBotClient.SendStickerAsync(chatId, sticker: stickerToSend);
          Console.WriteLine("стікер відправлено");
          return;

        case 1:
          string[]? gifIds = Environment.GetEnvironmentVariable("Gifs")?.Split(',');

          if (gifIds == null || gifIds.Length == 0)
          {
            Console.WriteLine("нема гіфок за таким ім'ям");
            return;
          }

          int gifIdNumMin = 0;
          int gifIdNumMax = gifIds.Length;

          int gifIdNum = random.Next(gifIdNumMin, gifIdNumMax);
          string gifId = gifIds[gifIdNum];

          InputFileId? gifToSend = new(Environment.GetEnvironmentVariable(gifId));

          if (gifToSend == null)
          {
            System.Console.WriteLine($"немає гіфки за ID: {gifId}");
            return;
          }

          await telegramBotClient.SendAnimationAsync(chatId, gifToSend);
          Console.WriteLine("гіфку відправлено");
          return;

        default:
          Console.WriteLine("щось пішло не так");
          return;
      }
    }

    public static async Task SendPhoto(ITelegramBotClient telegramBotClient, long chatId, string fileId, string caption)
    {
      string url = $"https://drive.google.com/uc?export=view&id={fileId}";
      InputFile inputFile = InputFile.FromUri(url);

      if (inputFile == null)
      {
        Console.WriteLine($"не існує фотки за посиланням: https://drive.google.com/uc?export=view&id={fileId}");
        return;
      }

      await telegramBotClient.SendPhotoAsync(chatId, inputFile, caption: caption);
      Console.WriteLine("закинув фотку");
      return;
    }

    public static async Task InlineButtonsIntegralDerivativeChoice(ITelegramBotClient telegramBotClient, long chatId)
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
      return;
    }

    public static async Task SendMainMenu(ITelegramBotClient telegramBotClient, long chatId)
    {
      await telegramBotClient.SendTextMessageAsync(chatId, "Прєт 👋. Я можу кинути тобі формулки з матану 🤓☝️ та рандомний мємасік з викладачів ІП-32. Для цього введи команду /formula або /memes");
    }

    public static async Task SendAlbum(ITelegramBotClient telegramBotClient, long chatId, params string[] filesId)
    {
      IAlbumInputMedia[] albumInputMedia = new IAlbumInputMedia[filesId.Length];
      for (int i = 0; i < filesId.Length; i++)
      {
        string photoUrl = $"https://drive.google.com/uc?export=view&id={filesId[i]}";
        InputFileUrl photoToSend = new(photoUrl);

        if (photoToSend == null)
        {
          Console.WriteLine("такого фото в моєму альбомі нема");
          return;
        }

        albumInputMedia[i] = new InputMediaPhoto(photoToSend)
        {
          Caption = $"ст. {i + 1}"
        };
      }

      await telegramBotClient.SendMediaGroupAsync(chatId, albumInputMedia);
      Console.WriteLine("відправив фотоколаж");
      return;
    }
  }
}