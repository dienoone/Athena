﻿using Athena.Application.Common.Exceptions;
using Athena.Infrastructure.Common;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Athena.Infrastructure.Identity
{
    internal partial class UserService
    {
        private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
        {
            EnsureValidTenant();

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            const string route = "code-redirection";
            var endpointUri = new Uri(string.Concat($"https://athena-student.vercel.app/", route));
            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
            return verificationUri;
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken)
        {
            Console.WriteLine(userId);
            var user = await _userManager.Users
                .Where(u => u.Id == userId && u.EmailConfirmed == false)
                .FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new InternalServerException(_t["An error occurred while confirming E-Mail."]);

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded;
        }

        public async Task<string> ConfirmPhoneNumberAsync(string userId, string code)
        {
            EnsureValidTenant();

            var user = await _userManager.FindByIdAsync(userId);

            _ = user ?? throw new InternalServerException(_t["An error occurred while confirming Mobile Phone."]);

            var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);

            return result.Succeeded
                ? user.EmailConfirmed
                    ? string.Format(_t["Account Confirmed for Phone Number {0}. You can now use the /api/tokens endpoint to generate JWT."], user.PhoneNumber)
                    : string.Format(_t["Account Confirmed for Phone Number {0}. You should confirm your E-mail before using the /api/tokens endpoint to generate JWT."], user.PhoneNumber)
                : throw new InternalServerException(string.Format(_t["An error occurred while confirming {0}"], user.PhoneNumber));
        }
    }
}