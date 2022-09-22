// See https://aka.ms/new-console-template for more information
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

var clientTelegram = new TelegramBotClient("5638372685:AAGXr0OrJCh5z57Wl885NDXBwOIYGzqXGM0");
clientTelegram.StartReceiving(Update,Error);

Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
{
    throw new NotImplementedException();
}

async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken arg3)
{
    var message = update.Message;

    if(message.Text != null)
    {
        Console.WriteLine($"{message.Chat.FirstName}  |  {message.Text}");
        if (message.Text.ToLower().Contains("hello"))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,"World");
            return;
        }
    }
    if(message.Photo != null)
    {
        var fileId = update.Message.Photo.Last().FileId;
        var fileInfo = await botClient.GetFileAsync(fileId);
        var filePath = fileInfo.FilePath;

        string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/download.jpg";
        await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
        await botClient.DownloadFileAsync(filePath, fileStream);
        fileStream.Close();

        byte[] bytes = File.ReadAllBytes(destinationFilePath);
        Console.WriteLine(Convert.ToBase64String(bytes));
    }

}

Console.ReadLine();
