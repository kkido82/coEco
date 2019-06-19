using System.Net;
using System.Net.Mail;

namespace CoEco.Services.Services
{
    public interface IMailService
    {
        void Send(MailRequest request);
    }

    public class MailRequest
    {
        public MailRequest()
        {
            IsHtml = true;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }

    public class SmtpOptions
    {
        public SmtpOptions(string host, int port, string username, string password, bool enableSsl)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            EnableSsl = enableSsl;
        }
        public string Host { get; }
        public int Port { get; }
        public string Username { get; }
        public string Password { get; }
        public bool EnableSsl { get; }
    }

    public class SmtpMailService : IMailService
    {
        private readonly SmtpOptions options;

        public SmtpMailService(SmtpOptions options)
        {
            this.options = options;
        }
        public void Send(MailRequest request)
        {
            //var client = new SmtpClient("smtp.gmail.com", 587)
            //{
            //    Credentials = new NetworkCredential("myusername@gmail.com", "mypwd"),
            //    EnableSsl = true
            //};

            var client = new SmtpClient(options.Host, options.Port)
            {
                Credentials = new NetworkCredential(options.Username, options.Password),
                EnableSsl = options.EnableSsl
            };

            var msg = new MailMessage(request.From, request.To)
            {
                IsBodyHtml = request.IsHtml,
                Subject = request.Subject,
                Body = request.Body
            };
            
            client.Send(msg);
        }
    }
}
