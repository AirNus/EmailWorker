// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using send_mail_signa;
using send_mail_signa.Services;
using send_mail_signa.EDS;
using System.Net.Mail;

string messagePath = Environment.CurrentDirectory + @"\EmailData.txt";
string signaturePath = Environment.CurrentDirectory + @"\EmailSignature.txt";
string publicKeyPath = Environment.CurrentDirectory + @"\EmailPublicKey.txt";


//Записываем сообщение в файл
string messageBody = "Сообщение которое будет подписано";
FileService.WriteFile(messagePath,messageBody);

//Получаем хэш из текста письма
var messageHash = Crypto.GetHash(messageBody);

//Создаем подпись
Signature signature = new Signature();
//Получаем подпись и записываем его в файл
var messageSignature = signature.SignData(messageHash);
FileService.WriteFile(signaturePath, messageSignature);

//Получаем публичный ключ и записываем его в файл
var publicKey = signature._publicKey;
var jsonPublicKey = JsonConvert.SerializeObject(publicKey);
FileService.WriteFile(publicKeyPath, jsonPublicKey);


//Формируем и отправляем письмо
string emailFrom = "stanislav.inshakov@mail.ru";
string emailTo = "ainur.mazitov@mail.ru";

string title = "Лабораторная работа 2";
string text = "Письмо и подпись с ключом во вложении";


string[] paths = new string[] { messagePath, signaturePath, publicKeyPath };
EmailService message = new EmailService();
message.CreateEmail(emailFrom, emailTo, title, text);
message.AddAttachments(paths);



SmtpService.SendMessage(message.message);



var emailPaths = ImapService.GetLastEmail(emailFrom);


if (emailPaths != null && emailPaths.Count > 0)
{
    if (message.CheckEmailSignature(emailPaths))
        Console.WriteLine("Подпись действительна");
    else
        Console.WriteLine("Подпись недействительна");
}
else
    Console.WriteLine("Писем не обнаружено");

Console.WriteLine("Done");


