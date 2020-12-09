using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RicoCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RicoCore.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _emailSettings;

        public EmailSender(IConfiguration configuration, IOptions<EmailSettings> emailOptions)
        {
            _configuration = configuration;
            _emailSettings = emailOptions.Value;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                #region RestClient

                //RestClient client = new RestClient
                //{
                //    BaseUrl = new Uri(_emailSettings.ApiBaseUri),
                //    Authenticator = new HttpBasicAuthenticator("api", _emailSettings.ApiKey)
                //};
                //RestRequest request = new RestRequest();
                //request.AddParameter("domain", _emailSettings.Domain, ParameterType.UrlSegment);
                //request.Resource = "{domain}/messages";
                //request.AddParameter("from", _emailSettings.From);
                //request.AddParameter("to", email);
                //request.AddParameter("subject", subject);
                //request.AddParameter("html", htmlMessage);
                //request.Method = Method.POST;

                //TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

                //client.ExecuteAsync(
                //    request, r => taskCompletion.SetResult(r));

                //RestResponse response = (RestResponse)(await taskCompletion.Task);

                #endregion

                #region SmtpClient

                SmtpClient client = new SmtpClient(_configuration["MailSettings:Server"])
                {
                    UseDefaultCredentials = false,
                    Port = int.Parse(_configuration["MailSettings:Port"]),
                    EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                    Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["MailSettings:FromEmail"], _configuration["MailSettings:FromName"]),
                };
                mailMessage.To.Add(email);
                mailMessage.Body = message;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                client.Send(mailMessage);
                return Task.CompletedTask;

                #endregion

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
