using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobsity.Chatbot.Specs.Drivers
{
    public static class ClientExtension
    {
        public static HttpClient WithServiceUser(this HttpClient client)
        {
            var headers = client.DefaultRequestHeaders
                .Where(x => x.Key.StartsWith("X-TestServerAuthentication-", StringComparison.InvariantCulture))
                .Select(x => x.Key).ToList();
            headers.ForEach(h => client.DefaultRequestHeaders.Remove(h));
            return client;
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, Uri uri, T data)
            => await SendAsync(client, HttpMethod.Post, uri, data).ConfigureAwait(false);

        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient client, Uri uri, T data)
            => await SendAsync(client, HttpMethod.Put, uri, data).ConfigureAwait(false);

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
            => JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync().ConfigureAwait(false));

        private static async Task<HttpResponseMessage> SendAsync<T>(HttpClient client, HttpMethod method, Uri uri, T data)
        {
            using var message = new HttpRequestMessage(method, uri);

            switch (method.Method.ToUpperInvariant())
            {
                case "PUT":
                case "POST":
                    message.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    break;
            }

            return await client.SendAsync(message).ConfigureAwait(false);
        }
    }
}