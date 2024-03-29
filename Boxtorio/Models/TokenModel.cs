﻿namespace Boxtorio.Models;

public sealed class TokenModel
{
    public TokenModel(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
