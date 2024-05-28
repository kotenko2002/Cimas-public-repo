namespace Cimas.IntegrationTests.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string url, StringContent content)
        {
            var request = new HttpRequestMessage()
            {
                Content = content,
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url, UriKind.Relative)
            };

            return await client.SendAsync(request);
        }
    }
}
