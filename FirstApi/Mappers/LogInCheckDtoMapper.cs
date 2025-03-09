using FirstApi.Models;

public static class ActiveTokenDtoMapper
{
    public static ActiveToken FormUserToActiveToken(this User user, string token)
    {
        return new ActiveToken
        {
            UserId = user.UserId,
            UserToken = token,
            TokenIssuedAtUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            TokenExpiryAtUtc = DateTime.SpecifyKind(DateTime.UtcNow.AddHours(8), DateTimeKind.Utc),
        };
    }
}
