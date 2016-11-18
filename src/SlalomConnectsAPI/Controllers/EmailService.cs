using SlalomConnectsAPI.Models;
using System;
using System.Net.Mail;
using System.Net.Mime;

namespace SlalomConnectsAPI.Controllers
{
    public class EmailService
    {
        public void SendEmail(EventRequest eventRequest)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(eventRequest.Email));

                // From
                mailMsg.From = new MailAddress("events@slalommeetupweb.azurewebsites.net", "Slalom Meetup");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "Slalom Meetup - We're finding some friends for " + eventRequest.EventType;
                string text = "Hey! We've got you in line to find a group for " + eventRequest.EventType + ". Once we find some others to meet up with you, we'll let you know!";

                string html = @"<p>Hey! We've got you in line to find a group for " + eventRequest.EventType + ". Once we find some others to meet up with you, we'll let you know!</p>";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("azure_fdbdefa0e44ecae9fe0ac53bdf27ec3f@azure.com", "slalom2016");
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendEmail(EventGroup eventGroup)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                foreach (var eventRequest in eventGroup.EventRequests)
                {
                    mailMsg.To.Add(new MailAddress(eventRequest.Email));
                }

                // From
                mailMsg.From = new MailAddress("events@slalommeetupweb.azurewebsites.net", "Slalom Meetup");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "Slalom Meetup - Your " + eventGroup.EventType;
                string text = "Hey! Are you ready for " + eventGroup.EventType + "? Meet your fellow Slalomites on Floor 15 near the elevators at " + eventGroup.StartTime + "!" +
                              "Have fun!";

                string html = @"<p>Hey!< br > " +
                              "Are you ready for " + eventGroup.EventType + "?" +
                              "Meet your fellow Slalomites on Floor 15 near the elevators at " + eventGroup.StartTime + "!<br>" +
                              "Have fun! </p>";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("azure_fdbdefa0e44ecae9fe0ac53bdf27ec3f@azure.com", "slalom2016");
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}