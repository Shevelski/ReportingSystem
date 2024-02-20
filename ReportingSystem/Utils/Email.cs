using System.Net.Mail;
using System.Net;

namespace ReportingSystem.Utils
{
    public class Email
    {
        public void AfterRegistration(string email, string password, string firstName)
        {
            string fromEmail = "reportingsystemnews@gmail.com";
            string subject = "Дякуємо за реєстрацію. З повагою, Адміністрація Reporting System";
            string body = $@"
                            <p>Дякуємо за увагу до нашого проекту, {firstName}.</p> 
                            <p>Ви зареєструвалися в системі Reporting System.</p> 
                            <p>Ваші параметри при реєстрації:</p>
                            <p><strong>Логін:</strong> {email}</p>
                            <p><strong>Пароль:</strong> {password}</p>";

            // Налаштування SmtpClient для Gmail
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587; // порт сервера SMTP для Gmail
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("reportingsystemnews@gmail.com", "tjrb lagg yzur yhwr");
            smtpClient.EnableSsl = true; // обов'язково включити SSL для Gmail


            // Створення об'єкта MailMessage
            MailMessage mailMessage = new MailMessage(fromEmail, email, subject, body);
            mailMessage.IsBodyHtml = true; // Встановлення, якщо тіло листа має HTML-формат

            try
            {
                // Відправлення листа
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при відправленні листа: " + ex.Message);
            }
        }
    }
}
