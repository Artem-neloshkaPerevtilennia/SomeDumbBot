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
      if (message.Text.Contains("hui"))
      {
        await botClient.SendTextMessageAsync(message.Chat.Id, "сам такий");
        return;
      }
    }

    private static async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
      throw new NotImplementedException();
    }
  }
}
