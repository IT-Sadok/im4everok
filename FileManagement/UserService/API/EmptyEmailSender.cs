
using Microsoft.AspNetCore.Identity;

namespace FileService.API
{
    public class EmptyEmailSender<TUser> : IEmailSender<TUser> where TUser : class
    {
        public Task SendConfirmationLinkAsync(TUser user, string email, string confirmationLink)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(TUser user, string email, string resetCode)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(TUser user, string email, string resetLink)
        {
            return Task.CompletedTask;
        }
    }
}
