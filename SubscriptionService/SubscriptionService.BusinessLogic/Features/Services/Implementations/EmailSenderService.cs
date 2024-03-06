using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Features.Services.Interfaces;
using SubscriptionService.BusinessLogic.Models.Subscription;
using SubscriptionService.BusinessLogic.Options;

namespace SubscriptionService.BusinessLogic.Features.Services.Implementations
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SMTPConfig _SmtpConfig;
        private readonly EmailSender _emailSender;
        private readonly IEmailMessageRenderer _messageRenderer;

        public EmailSenderService(
            IEmailMessageRenderer messageRenderer,
            IOptions<SMTPConfig> smtpConfig, 
            IOptions<EmailSender> emailSender)
        {
            _SmtpConfig = smtpConfig.Value;
            _emailSender = emailSender.Value;
            _messageRenderer = messageRenderer;
        }

        public void SendSubscriptionMadeMessage(SubscriptionWithUserInfo info)
        {
            var message = CreateMessage(info, EmailTemplates.subscriptionMade);
            SendMessage(message);
        }

        public void SendSubscriptionCanceledMessage(SubscriptionWithUserInfo info)
        {
            var message = CreateMessage(info, EmailTemplates.subscriptionCanceled);
            SendMessage(message);
        }

        private MimeMessage CreateMessage(SubscriptionWithUserInfo info, string templatePath)
        {
            var message = new MimeMessage();
            message.To.Add(MailboxAddress.Parse(info.UserInfo.Email));
            message.Subject = $"Listn Music – {info.UserInfo.Region} Region.";

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = _messageRenderer.Render(templatePath, info).Result
            };

            return message;
        }

        private void SendMessage(MimeMessage message)
        {
            message.From.Add(MailboxAddress.Parse(_emailSender.UserName));

            using var smtp = new SmtpClient();
            smtp.Connect(_SmtpConfig.Host, _SmtpConfig.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSender.UserName, _emailSender.Password);
            smtp.Send(message);
            smtp.Disconnect(true);
        }
    }
}
