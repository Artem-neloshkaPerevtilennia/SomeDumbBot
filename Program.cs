using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherBot
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var bot = new TelegramBotClient("7012783078:AAH3CIDoJ7mvfWr2TCMsz2W6XeKcg3ZKgts");
      bot.StartReceiving(Update, Error);
      Console.ReadLine();
    }

    private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
      var message = update.Message;
      if (message.Text == null) return;

      switch (message.Text.ToLower())
      {
        case "/integral":
          await SendPhoto(botClient, message.Chat.Id, "photos/integral_page1.jpg", "Інтеграли (ст. 1)");
          await SendPhoto(botClient, message.Chat.Id, "photos/integral_page2.jpg", "Інтеграли (ст. 2)");
          await SendPhoto(botClient, message.Chat.Id, "photos/integral_page3.jpg", "Інтеграли (ст. 3)");
          return;

        case "/derivative":
          await SendPhoto(botClient, message.Chat.Id, "photos/derivative.jpg", "Похідні");
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

    private static async Task SendPhoto(ITelegramBotClient telegramBotClient, long chatId, string path, string caption)
    {
      if (System.IO.File.Exists(path))
      {
        using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
          var inputFile = new InputFileStream(fileStream);

          await telegramBotClient.SendPhotoAsync(chatId, inputFile, caption: caption);
        }
      }
      else
      {
        Console.WriteLine("Помилка відправки");
        return;
      }
    }
  }
}
