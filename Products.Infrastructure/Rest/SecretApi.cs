﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Products.Domain.Entities;
using Products.Domain.Rest;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infrastructure.Rest
{
    public class SecretApi : ISecretApi
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public SecretApi(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _clientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> GetSecretAsync(string secret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
            _configuration["AWS_SECRET_URL"]);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("AuthorizationToken", "7793B690-9EC7-4240-A152-1D3046693CB3");
            request.Content = new StringContent(
                JsonConvert.SerializeObject(new { secret }),
                Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response>(result);

                return obj.Content.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
