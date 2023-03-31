using RestSharp;
using System.Text.Json;

namespace OnaxTools.Http
{
    public interface IRestClientService
    {
        Task<T> PostAsync<T, U>(string url, U payload, IDictionary<string, string> headers);
        Task<T> GetAsync<T>(string url, IDictionary<string, string> headers);
    }

    public class RestClientService : IRestClientService
    {
        public RestClientService()
        {

        }
        /// <summary>
        /// Generic Async rest client operation with Post method
        /// </summary>
        /// <typeparam name="T">Expected return data type</typeparam>
        /// <typeparam name="U">Data type of Payload</typeparam>
        /// <param name="url">Full absolute URL path</param>
        /// <param name="payload">Paylod body</param>
        /// <param name="headers">Optional - Dictionary of headers</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T, U>(string url, U payload, IDictionary<string, string> headers)
        {
            T objResp = default(T);
            try
            {
                var options = new RestClientOptions()
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(url, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, value: header.Value);
                    }
                }
                var body =JsonSerializer.Serialize(payload);
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);

                objResp =JsonSerializer.Deserialize<T>(response.Content);
            }
            catch (Exception ex)
            {
                OnaxTools.Logger.LogException(ex, nameof(GetAsync));
            }
            return objResp;
        }


        public async Task<T> GetAsync<T>(string url, IDictionary<string, string> headers)
        {
            T objResp = default(T);
            try
            {
                var options = new RestClientOptions()
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("Content-Type", "application/json");
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, value: header.Value);
                    }
                }
                RestResponse response = await client.ExecuteAsync(request);

                objResp = JsonSerializer.Deserialize<T>(response.Content);
            }
            catch (Exception ex)
            {
                OnaxTools.Logger.LogException(ex, nameof(GetAsync));
            }
            return objResp;
        }
    }
}
