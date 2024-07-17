namespace Athena.Domain.Entities
{
    public class ResetPasswordToken : BaseEntity, IAggregateRoot
    {
        public string Email { get; set; } // The user's ID associated with the token
        public string Code { get; set; } // The OTP or reset token code
        public DateTime ExpirationTime { get; set; } // The token's expiration time
        public bool Used { get; set; } // A flag to indicate whether the token has been used

        public ResetPasswordToken(string email, string code, DateTime expirationTime, bool used)
        {
            Email = email;
            Code = code;
            ExpirationTime = expirationTime;
            Used = used;
        }

        public ResetPasswordToken Update(bool used)
        {
            Used = used;
            return this;
        }
    }
}
