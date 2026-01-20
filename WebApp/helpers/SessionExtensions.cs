using System.Text.Json;

namespace WebApp.Helpers;

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? Get<T>(this ISession session, string key)
    {
        var json = session.GetString(key);
        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }
}
