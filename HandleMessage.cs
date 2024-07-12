using Telegram.Bot;
using Telegram.Bot.Types;

namespace SomeDumbBot
{
  public class HandleMessage
  {
    public static async Task SendIntegralDerivativeByChoise(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery)
    {
      if (callbackQuery.Message == null) return;

      switch (callbackQuery.Data)
      {
        case "integral":
          await SendMessage.SendAlbum(telegramBotClient, callbackQuery.Message.Chat.Id, ["1_TmW1-k_uMhRlsWJQSo3e8r3obWaI4Lg", "1jCmc64x92n1yl-jKEbp4coCi81R2Rg5L", "1bu_kJIqT1-auY_CnCmy0lT_a7ghu1Ndd"]);
          return;

        case "derivative":
          await SendMessage.SendPhoto(telegramBotClient, callbackQuery.Message.Chat.Id, "1jfuoznsb-THDrkay4JKcVscrih461JYk", "Таблиця похідних");
          return;

        case "backToMenu":
          await SendMessage.SendMainMenu(telegramBotClient, callbackQuery.Message.Chat.Id);
          return;

        default:
          await telegramBotClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Нема такої опції");
          return;
      }
    }

    public static async Task HandleTextMessage(ITelegramBotClient telegramBotClient, Message message)
    {
      if (message.Text == null) return;

      switch (message.Text.ToLower())
      {
        case "/start":
          await SendMessage.SendMainMenu(telegramBotClient, message.Chat.Id);
          return;

        case "/formula":
          await SendMessage.InlineButtonsIntegralDerivativeChoice(telegramBotClient, message.Chat.Id);
          return;

        case "/memes":
          await SendMessage.SendRandomStickerOrGif(telegramBotClient, message.Chat.Id);
          return;

        default:
          await telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Unknown option");
          return;
      }
    }
  }
}