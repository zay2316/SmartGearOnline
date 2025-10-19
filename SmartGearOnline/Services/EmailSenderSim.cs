using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SmartGearOnline.Services
{
    public class EmailSenderSim : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Simulates implimentation of email sending
            Console.WriteLine($"Simulated email to: {email}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}