using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using RestMultidialogoClient.Dto;

namespace RestMultidialogoClient
{
    static class Utils
    {
        public static string CreateFileContent(string fileName, string mimeType)
        {
            byte[] buffer = ReadFileFromResources(fileName);
            return "data:" + mimeType + ";base64," + Convert.ToBase64String(buffer);
        }

        private static byte[] ReadFileFromResources(string fileName)
        {
            return ((byte[])(RestMultidialogoClient.Properties.Resources.ResourceManager.GetObject(fileName)));
        }

        public static string CreatePostFilePayload(string fileName, string fileContent)
        {
            UploadFileRequestData uploadFileRequestData = UploadFileRequestData.CreateUploadFileRequestData(fileName, fileContent);
            return JsonConvert.SerializeObject(uploadFileRequestData);
        }

        public static int GetChoice()
        {
            return Convert.ToInt32(Console.ReadLine());
        }

        public static string GetAccount()
        {
            Console.WriteLine("Inserire l'account desiderato: \"me\" per l'amministratore, l'id per uno dei sottoutenti (condomini)");
            Console.WriteLine("NB: la API che ritorna gli id dei sottoutenti disponibili è mostrata nell'esempio 6 - utenti collegati");
            return Console.ReadLine();
        }

        public static string GetResponseId(string respBody)
        {
            return GetGenericResponse(respBody).GetId();
        }

        private static GenericResponse GetGenericResponse(string respBody)
        {
            return JsonConvert.DeserializeObject<GenericResponse>(respBody);
        }

        public static string GetResponseStatus(string respBody)
        {
            return GetGenericResponse(respBody).Status;
        }

        public static StringContent CreateStringContent(string json)
        {
            return String.IsNullOrEmpty(json) ? null : new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static HttpClient CreateHttpClient()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return http;
        }

        public static HttpRequestMessage CreateRequest(string url, string token, StringContent json, string method)
        {
            return method.Equals("Post") ? BuildRequest(url, token, json, "Post") : BuildRequest(url, token, json, "Get");
        }

        private static HttpRequestMessage BuildRequest(string url, string token, StringContent json, string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), new Uri(url));
            request.Headers.Add("X-api-client-name", Constants.X_API_CLIENT_NAME);
            //
            // ATTENZIONE: 
            // l'api key dovrebbe essere custodita in maniera sicura, non salvata in chiaro su disco, ad esempio. 
            //
            request.Headers.Add("X-api-key", Constants.X_API_KEY);
            request.Headers.Add("X-api-client-version", Constants.X_API_CLIENT_VERSION);
            request.Headers.Add("Accept-Language", "it");
            if (!String.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", "Bearer " + token);
            }
            request.Content = json;
            return request;
        }

        public static HttpRequestMessage CreateCurrTokenRequest(string url, string json, string method)
        {
            return CreateRequest(url, TokenWallet.GetCurrentTokens().Token, CreateStringContent(json), method);
        }
    }
}
