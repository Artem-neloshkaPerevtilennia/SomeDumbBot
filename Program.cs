using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DotNetEnv;

namespace SomeDumbBot
{
  class Program
  {
    static void Main(string[] args)
    {
      Env.Load();

      var botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");

      if (botToken == null)
      {
        Console.WriteLine("Помилка завантаження бот-токена");
        return;
      }

      var bot = new TelegramBotClient(botToken);
      bot.StartReceiving(Update, Error);
      Console.ReadKey();
    }

    private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
      if (update.Type == UpdateType.Message && update.Message?.Text != null)
      {
        await HandleMessage.HandleTextMessage(botClient, update.Message);
      }
      else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
      {
        await HandleMessage.SendIntegralDerivativeByChoise(botClient, update.CallbackQuery);
      }
    }

    private static async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
      throw new NotImplementedException();
    }
  }
}
