// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using send_mail_signa;
using System.Net.Mail;
using System.Security.Cryptography;

Console.WriteLine(Environment.CurrentDirectory);

string messagePath = Environment.CurrentDirectory + @"\EmailData.txt";
string signaturePath = Environment.CurrentDirectory + @"\EmailSignature.txt";
string publicKeyPath = Environment.CurrentDirectory + @"\EmailPublicKey.txt";

MailAddress fromAddress = new MailAddress("stanislav.inshakov@mail.ru"); 
MailAddress toAddress = new MailAddress("ainur.mazitov@mail.ru"); 

//Записываем сообщение в файл
string messageBody = "Сообщение которое будет подписано";

FileService.WriteFile(messagePath,messageBody);


//Создаем подпись
Signature signature = new Signature();

//Получаем хэш из текста письма
var messageHash = Crypto.GetHash(messageBody);

//Получаем подпись и записываем его в файл
var messageSignature = signature.SignData(messageHash);
FileService.WriteFile(signaturePath, messageSignature);


//Получаем публичный ключ и записываем его в файл
var publicKey = signature._publicKey;
var jsonPublicKey = JsonConvert.SerializeObject(publicKey);
FileService.WriteFile(publicKeyPath, jsonPublicKey);



//Формируем и отправляем письмо
MailMessage message = new MailMessage(fromAddress, toAddress)
{
    Subject = "Лабораторная работа 2",
    Body = "Письмо и подпись с ключом во вложении",
    Sender = fromAddress,
};
message.Attachments.Add(new Attachment(messagePath));
message.Attachments.Add(new Attachment(signaturePath));
message.Attachments.Add(new Attachment(publicKeyPath));


SmtpService.SendMessage(message);


var emailPaths = ImapService.GetLastEmail(fromAddress.Address);

if (emailPaths != null && emailPaths.Count > 0)
{
    //Здесь можно поменять текст сообщения чтоб проверить работает ли подпись
    var textEmail = FileService.ReadFile(emailPaths["EmailData.txt"]);
    var textEmailHash = Crypto.GetHash(textEmail);


    var textSignature = FileService.ReadSignature(emailPaths["EmailSignature.txt"]);

    var textKey = FileService.ReadFile(emailPaths["EmailPublicKey.txt"]);
    var newKey = JsonConvert.DeserializeObject<RSAParameters>(textKey);


    signature._publicKey = newKey;

    var decision = signature.VerifySignature(textEmailHash, textSignature);

    if (decision)
        Console.WriteLine("Подпись действительна");
    else
        Console.WriteLine("Подпись недействительна");
}
else
    Console.WriteLine("Писем не обнаружено");

Console.WriteLine("Done");


