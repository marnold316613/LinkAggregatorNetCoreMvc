using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace LinkAggregatorMVC.Services
{
    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "email-smtp.us-east-2.amazonaws.com", //or another email sender provider
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                
                Credentials = new NetworkCredential("AKIA5FTZBAT6FW4RPHPB", "BOx7zmsdMv14WdPKiCa2VpkCUb5xJtnt2r6MI6BanxFg")
            };

            return client.SendMailAsync("michael.arnold.316@outlook.com", email, subject, htmlMessage);
        }
    }
}
