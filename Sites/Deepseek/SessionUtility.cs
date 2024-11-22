using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class SessionUtility
{
    // Save IEnumerable<T> to session
    public static void SetList<T>(this ISession session, string key, IEnumerable<T> list)
    {
        // Serialize the list to JSON
        var jsonList = JsonConvert.SerializeObject(list);

        // Save the JSON string in session
        session.SetString(key, jsonList);
    }

    // Retrieve IEnumerable<T> from session
    public static IEnumerable<T> GetList<T>(this ISession session, string key)
    {
        // Retrieve the JSON string from session
        var jsonList = session.GetString(key);

        // Deserialize the JSON string to IEnumerable<T>
        return string.IsNullOrEmpty(jsonList) ? default : JsonConvert.DeserializeObject<IEnumerable<T>>(jsonList);
    }
}