﻿using ShareMyCalendar.API.Responses;

namespace ShareMyCalendar.API.Mappers;

public static class LoginSuccessResponseMapper
{
    public static LoginSuccessResponse Map(ThomasMathers.Infrastructure.IAM.Responses.LoginSuccessResponse source) => new()
    {
        Id = source.User.Id,
        UserName = source.User.UserName,
        AccessToken = source.AccessToken
    };
}
