﻿namespace Quizlet.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public string Secret { get; init; } = null!;
        public int ExpiryMinutes { get; init; }
    }
}
