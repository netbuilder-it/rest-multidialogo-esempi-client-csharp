using Newtonsoft.Json;
using RestMultidialogoClient.Domain;
using RestMultidialogoClient.Dto;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestMultidialogoClient
{
    class Program
    {
        private static HttpClient http;
        private static bool done = false;

        static void Main(string[] args)
        {
            http = Utils.CreateHttpClient();
            TokenWallet.ReadTokens();

            while (!done)
            {
                Menu();

                int choice = Utils.GetChoice();

                if (choice <= 0)
                {
                    done = true;
                    continue;
                }

                Authenticate();

                switch (choice)
                {
                    case 1:
                        Scenario_1().Wait();
                        break;
                    case 2:
                        Scenario_2().Wait();
                        break;
                    case 3:
                        Scenario_3().Wait();
                        break;
                    case 4:
                        Scenario_4().Wait();
                        break;
                    case 5:
                        Scenario_5().Wait();
                        break;
                    case 6:
                        Scenario_6().Wait();
                        break;
                    case 7:
                        Scenario_7().Wait();
                        break;
                    case 8:
                        Scenario_8().Wait();
                        break;
                    case 9:
                        Scenario_9().Wait();
                        break;
                    default:
                        Console.WriteLine("Scelta errata");
                        break;
                }
            }
        }

        private static void Authenticate()
        {
            if (String.IsNullOrEmpty(TokenWallet.GetCurrentTokens().Token))
            {
                Console.WriteLine("Richiesta di un nuovo token");
                Login(Constants.REST_MULTIDIALOGO_STAGE_USERNAME, Constants.REST_MULTIDIALOGO_STAGE_PASSWORD).Wait();
            }
            else
            {
                Console.WriteLine("Riutilizzo token salvato " + TokenWallet.GetCurrentTokens().Token);
            }
        }

        private static void Menu()
        {
            Console.WriteLine("");
            Console.WriteLine("-------------------");
            Console.WriteLine("Scenari disponibili");
            Console.WriteLine("-------------------");
            Console.WriteLine(" 1 - Invio a 3 destinatari: 3 con posta tradizionale ma affrancatura diversa a carico dell'account (es. studio o condominio)");
            Console.WriteLine(" 2 - Invio a 3 destinatari: 1 con posta tradizionale, 2 MultiCerta che generano due canali alternativi con affrancatura diversa a carico dell'account (es. studio o condominio)");
            Console.WriteLine(" 3 - Esempio con errore: file globale associato a un destinatario");
            Console.WriteLine(" 4 - Legge loghi impostati in preferenze");
            Console.WriteLine(" 5 - Legge PEC impostata in preferenze");
            Console.WriteLine(" 6 - Elenco utenti collegati");
            Console.WriteLine(" 7 - Invio Certificazione Unica 2020");
            Console.WriteLine(" 8 - Legge tipo di affrancatura impostata in preferenze");
            Console.WriteLine(" 9 - Invio messaggio SMS a 2 destinatari");
            Console.WriteLine(" 0 - Fine");
            Console.WriteLine("Scegli lo scenario: ");
        }

        // Invio a 3 destinatari: 3 con posta tradizionale ma affrancatura diversa a carico dell'account (es. studio o condominio)
        private static async Task Scenario_1()
        {
            try
            {
                string account = Utils.GetAccount();
                string uploadSessionId = await GetUploadSessionId(account);

                File personale1 = await PostFile(account, uploadSessionId, "personale1", "application/pdf", "private", null);
                File personale2 = await PostFile(account, uploadSessionId, "personale2", "application/pdf", "private", null);
                File personale3 = await PostFile(account, uploadSessionId, "personale3", "application/pdf", "private", null);
                File globale1 = await PostFile(account, uploadSessionId, "globale1", "application/pdf", "global", AttachmentOptions.CreateAttachmentOptions(true, "bw", "A4", 80, true, false));

                Attachments attachments = Attachments.CreateAttachments(uploadSessionId, new List<File> { personale1, personale2, personale3, globale1 });

                Recipient recipient1 = Recipient.CreateRecipient("Via Emilia Ovest 129/2", "43126", "Parma", "PR", "it",
                        "pt", "RACCOMANDATA1",
                        "person", "Winton", "Marsalis", "Multidialogo Srl",
                        "esempio1@catchall.netbuilder.it", null,
                        new List<string> { personale1.Id },
                        "sendposta", null,
                        null);

                Recipient recipient2 = Recipient.CreateRecipient("Via Zarotto 63", "43123", "Collecchio", "PR", "it",
                        "pt", "RACCOMANDATA1AR",
                        "person", "Clara", "Schumann", "ASA Srl",
                        "esempio2@catchall.netbuilder.it", null,
                        new List<string> { personale2.Id },
                        "sendposta", null,
                        null);

                Recipient recipient3 = Recipient.CreateRecipient("Via Zarotto 63", "43123", "Collecchio", "PR", "it",
                        "pt", "PRIORITARIA1",
                        "person", "Amilcare", "Ponchielli", "AM Spa",
                        "esempio3@catchall.netbuilder.it", "info@pec.testtest.it",
                        new List<string> { personale3.Id },
                        "sendposta", null,
                        null);

                PostQueueDto postQueueDto = PostQueue.CreatePostQueue(Sender.CreateSender(), attachments,
                        new List<Recipient> { recipient1, recipient2, recipient3 },
                        "Convocazione assemblea", "Caro sei convocato per l'assemblea. Visualizza l'allegato. Grazie.",
                        false, false, false, false, "Test scenario 1", null);

                await SendPostQueueRequest(account, postQueueDto);
            }
            catch (ApiDialogException e)
            {
                Console.WriteLine("Errore: " + e.Message);
            }
        }

        // Invio a 3 destinatari: 1 con posta tradizionale, 2 MultiCerta che generano due canali alternativi con affrancatura diversa a carico dell'account (es. studio o condominio)
        private static async Task Scenario_2()
        {
            try
            {
                string account = Utils.GetAccount();
                string uploadSessionId = await GetUploadSessionId(account);

                File personale1 = await PostFile(account, uploadSessionId, "personale1", "application/pdf", "private", null);
                File personale2 = await PostFile(account, uploadSessionId, "personale2", "application/pdf", "private", null);
                File personale3 = await PostFile(account, uploadSessionId, "personale3", "application/pdf", "private", null);
                File globale1 = await PostFile(account, uploadSessionId, "globale1", "application/pdf", "global", AttachmentOptions.CreateAttachmentOptions(true, "bw", "A4", 80, true, false));

                Attachments attachments = Attachments.CreateAttachments(uploadSessionId, new List<File> { personale1, personale2, personale3, globale1 });

                Recipient recipient1 = Recipient.CreateRecipient("Via Emilia Ovest 129/2", "43126", "Parma", "PR", "it",
                        "pt", "RACCOMANDATA1",
                        "person", "Winton", "Marsalis", "Multidialogo Srl",
                        "esempio1@catchall.netbuilder.it", null,
                        new List<string> { personale1.Id },
                        "sendposta", null,
                        null);

                Recipient recipient2 = Recipient.CreateRecipient("Via Zarotto 63", "43123", "Collecchio", "PR", "it",
                        "pt", "RACCOMANDATA1AR",
                        "person", "Clara", "Schumann", "ASA Srl",
                        Constants.MULTICERTA_ENABLED_ADDRESS, null,
                        new List<string> { personale2.Id },
                        "multicerta", "sendposta",
                        null);

                Recipient recipient3 = Recipient.CreateRecipient("Via Zarotto 63", "43123", "Collecchio", "PR", "it",
                        "pt", "PRIORITARIA1",
                        "person", "Amilcare", "Ponchielli", "AM Spa",
                        Constants.MULTICERTA_ENABLED_ADDRESS, "info@pec.testtest.it",
                        new List<string> { personale3.Id },
                        "multicerta", "sendposta",
                        null);

                PostQueueDto postQueueDto = PostQueue.CreatePostQueue(Sender.CreateSender(), attachments,
                        new List<Recipient> { recipient1, recipient2, recipient3 },
                        "Convocazione assemblea", "Caro sei convocato per l'assemblea. Visualizza l'allegato. Grazie.",
                        true, false, false, false, "Test scenario 1", null);

                await SendPostQueueRequest(account, postQueueDto);
            }
            catch (ApiDialogException e)
            {
                Console.WriteLine("Errore: " + e.Message);
            }
        }

        // Esempio con errore: file globale associato a un destinatario
        private static async Task Scenario_3()
        {
            try
            {
                string account = Utils.GetAccount();
                string uploadSessionId = await GetUploadSessionId(account);

                File personale1 = await PostFile(account, uploadSessionId, "personale1", "application/pdf", "private", null);
                File personale2 = await PostFile(account, uploadSessionId, "personale2", "application/pdf", "private", null);
                File globale1 = await PostFile(account, uploadSessionId, "globale1", "application/pdf", "global", AttachmentOptions.CreateAttachmentOptions(true, "bw", "A4", 80, true, false));

                Attachments attachments = Attachments.CreateAttachments(uploadSessionId, new List<File> { personale1, personale2, globale1 });

                Recipient recipient1 = Recipient.CreateRecipient("Via Emilia Ovest 129/2", "43126", "Parma", "PR", "it",
                        "pt", "RACCOMANDATA1",
                        "person", "Winton", "Marsalis", "Multidialogo Srl",
                        "esempio1@catchall.netbuilder.it", null,
                        new List<string> { personale1.Id },
                        "sendposta", null,
                        null);

                Recipient recipient2 = Recipient.CreateRecipient("Via Zarotto 63", "43123", "Collecchio", "PR", "it",
                        "pt", "RACCOMANDATA1AR",
                        "person", "Clara", "Schumann", "ASA Srl",
                        "esempio2@catchall.netbuilder.it", null,
                        new List<string> { globale1.Id, personale2.Id },
                        "sendposta", null,
                        null);

                PostQueueDto postQueueDto = PostQueue.CreatePostQueue(Sender.CreateSender(), attachments,
                        new List<Recipient> { recipient1, recipient2 },
                        "Convocazione assemblea", "Caro sei convocato per l'assemblea. Visualizza l'allegato. Grazie.",
                        false, false, false, false, "Test scenario 1", null);

                await SendPostQueueRequest(account, postQueueDto);
            }
            catch (ApiDialogException e)
            {
                Console.WriteLine("Errore: " + e.Message);
            }
        }

        // Leggi loghi impostati in preferenze
        private static async Task Scenario_4()
        {
            string account = Utils.GetAccount();
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/" + account + "/preferences";

            HttpResponseMessage response = await SendRequest(url, null, "Get");
            if (response == null || response.Content == null)
            {
                return;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            UserPreferencesData userPreferences = JsonConvert.DeserializeObject<UserPreferencesData>(responseBody);

            Console.WriteLine("Loghi:");
            Console.WriteLine("------------------------");
            Console.WriteLine("Id | Label | Default");
            Console.WriteLine("------------------------");
            foreach (SenderLetterWatermarkPreferences l in userPreferences.GetLoghi())
            {
                Console.WriteLine(l.ToString());
            }
        }

        // Leggi PEC impostata in preferenze
        private static async Task Scenario_5()
        {
            string account = Utils.GetAccount();
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/" + account + "/preferences";

            HttpResponseMessage response = await SendRequest(url, null, "Get");
            if (response == null || response.Content == null)
            {
                return;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            UserPreferencesData userPreferences = JsonConvert.DeserializeObject<UserPreferencesData>(responseBody);

            Console.WriteLine("Pec:");
            Console.WriteLine("---------");
            Console.WriteLine("Indirizzo");
            Console.WriteLine("---------");
            foreach (string p in userPreferences.GetPec())
            {
                Console.WriteLine(p);
            }
        }

        // Elenco utenti collegati
        private static async Task Scenario_6()
        {
            string account = "me";
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/" + account + "/users?include=user-profiles";

            HttpResponseMessage response = await SendRequest(url, null, "Get");
            if (response == null || response.Content == null)
            {
                return;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            UserResponse userResponse = JsonConvert.DeserializeObject<UserResponse>(responseBody);

            Console.WriteLine("Utenti:");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Id | IsActive | Group | Username | Displayname");
            Console.WriteLine("----------------------------------------------");
            foreach (UserExtended u in userResponse.GetUsers())
            {
                Console.WriteLine(u);
            }
        }

        // Invio Certificazione Unica 2020
        private static async Task Scenario_7()
        {
            string account = "me";
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/" + account + "/tax-withholding-transmission-sessions";
            CuPostRequestDto cuPostRequestDto = CuPostRequestDto.CreatePostRequestDto("esempio_cu");
            string json = JsonConvert.SerializeObject(cuPostRequestDto);

            HttpResponseMessage response = await SendRequest(url, json, "Post");
            if (response == null || response.Content == null)
            {
                return;
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("CU inviata");
            }
            else
            {
                Console.WriteLine("Si è verificato un errore! Dettagli:");
                HandleErrors(responseBody, cuPostRequestDto);
            }
        }

        // Legge tipo di affrancatura impostata in preferenze
        private static async Task Scenario_8()
        {
            string account = Utils.GetAccount();
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/" + account + "/preferences";

            HttpResponseMessage response = await SendRequest(url, null, "Get");
            if (response == null || response.Content == null)
            {
                return;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            UserPreferencesData userPreferences = JsonConvert.DeserializeObject<UserPreferencesData>(responseBody);
            Console.WriteLine("Affrancatura impostata: " + userPreferences.GetSenderPostageType());
        }

        private static async Task Scenario_9()
        {
            var account = Utils.GetAccount();

            // sender
            var sender = SmsSender.createWithPhoneNumber(
                    Constants.SENDER_PHONE_NUMBER_UUID,
                    Constants.SENDER_NOTIFICATION_ADDRESS
            );

            // options
            SmsQueueOptions options = SmsQueueOptions.create(null, "Promemoria #1443");

            // request
            Dto.PostSmsQueueRequest request = new Dto.PostSmsQueueRequest(
                    sender,
                    "Promemoria",
                    "Ciao {name}",
                    options);

            // add global custom data
            request.customData.Add(new CustomDataElement("my-identifier", "xyz", "hidden"));

            // recipients
            request.recipients.Add(
                    new Domain.SmsRecipient(
                            "+393660000001",
                            new List<Keyword>() { new Keyword("name", "Mario") },
                            null
                    )
            );

            request.recipients.Add(
                    new Domain.SmsRecipient(
                            "+393660000002",
                            new List<Keyword>() {
                                new Keyword("name", "Maria")
                            },
                            // add custom data to this particular recipient
                            new List<CustomDataElement>() {
                                 new CustomDataElement("recipient-id", "100", "hidden")
                            }
                    )
            );

            await sendPostSmsQueueRequest(account, new PostSmsQueueRequestDto(request));
        }

        private static async Task sendPostSmsQueueRequest(string account, PostSmsQueueRequestDto postQueueDto)
        {
            var url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/" + account + "/sms-queues";
            var json = JsonConvert.SerializeObject(postQueueDto);

            var response = await SendRequest(url, json, "Post");

            if (response == null || response.Content == null)
            {
                throw new ApiDialogException("Impossibile creare la coda");
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            string status = Utils.GetResponseStatus(responseBody);
            if (status.Equals("CREATED"))
            {
                Console.WriteLine("Coda creata");
            }
            else
            {
                Console.WriteLine("Si è verificato un errore! Dettagli:");
                HandleErrors(responseBody, postQueueDto);
            }
        }

        private static async Task SendPostQueueRequest(string account, PostQueueDto postQueueDto)
        {
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/" + account + "/queues";
            string json = JsonConvert.SerializeObject(postQueueDto);

            HttpResponseMessage response = await SendRequest(url, json, "Post");

            if (response == null || response.Content == null)
            {
                throw new ApiDialogException("Impossibile creare la coda");
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            string status = Utils.GetResponseStatus(responseBody);
            if (status.Equals("CREATED"))
            {
                Console.WriteLine("Coda creata");
            }
            else
            {
                Console.WriteLine("Si è verificato un errore! Dettagli:");
                HandleErrors(responseBody, postQueueDto);
            }
        }

        private static void HandleErrors(string response, Object postQueueDto)
        {
            Dto.ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response);

            Console.WriteLine(errorResponse.detail);

            foreach (Parameter p in errorResponse.GetParameters())
            {
                Console.WriteLine(p.code + " - " + p.parameter);
                ErrorUtils.ParseDotNotation(p.parameter, postQueueDto);
                foreach (string m in p.messages)
                {
                    Console.WriteLine(m);
                }
            }
        }

        private static async Task<File> PostFile(string account, string uploadSessionId, string fileName, String mimeType, string visibility, AttachmentOptions options)
        {
            string fileContent = Utils.CreateFileContent(fileName, mimeType);
            string json = Utils.CreatePostFilePayload(fileName, fileContent);
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/" + account + "/upload-sessions/" + uploadSessionId + "/uploaded-files";

            HttpResponseMessage response = await SendRequest(url, json, "Post");

            if (response == null || response.Content == null)
            {
                throw new ApiDialogException("Impossibile ottenere fileId");
            }
            string fileId = Utils.GetResponseId(await response.Content.ReadAsStringAsync());
            Console.WriteLine("PostFile Id = " + fileId);
            return new File(fileId, visibility, options);
        }

        private static async Task<string> GetUploadSessionId(string account)
        {
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/" + account + "/upload-sessions";

            HttpResponseMessage response = await SendRequest(url, Constants.EMPTY_JSON, "Post");

            if (response == null || response.Content == null)
            {
                throw new ApiDialogException("Impossibile ottenere uploadSessionid");
            }
            string uploadSessionId = Utils.GetResponseId(await response.Content.ReadAsStringAsync());
            Console.WriteLine("UploadSessionId = " + uploadSessionId);
            return uploadSessionId;
        }

        private static async Task<HttpResponseMessage> SendRequest(string url, string json, string method)
        {
            HttpResponseMessage response = null;
            bool done = false;

            do
            {
                response = await ExecuteCall(url, json, method);

                if (response.IsSuccessStatusCode)
                {
                    done = true;
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    Console.WriteLine("Server error (500)!");
                    return null;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("NotFound error (404)!");
                    return null;
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Console.WriteLine("Forbidden error (403)!");
                    return null;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Bad request error (400)!");
                    done = true;

                }
                else if (response.StatusCode == HttpStatusCode.ProxyAuthenticationRequired)
                {
                    Console.WriteLine("Proxy Authentication Required (407)!");
                    done = true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Token scaduto -> Refresh");
                    HttpResponseMessage res = await LoginRefresh(Constants.REST_MULTIDIALOGO_STAGE_USERNAME);
                    if (res.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("Refresh token scaduto -> Login");
                        Login(Constants.REST_MULTIDIALOGO_STAGE_USERNAME, Constants.REST_MULTIDIALOGO_STAGE_PASSWORD).Wait();
                    }
                }
            } while (!done);

            return response;
        }

        private static async Task<HttpResponseMessage> ExecuteCall(string url, string json, string method)
        {
            HttpRequestMessage request = Utils.CreateCurrTokenRequest(url, json, method);
            return await http.SendAsync(request);
        }

        private static async Task<HttpResponseMessage> LoginRefresh(string username)
        {
            string token = TokenWallet.GetCurrentTokens().RefreshToken;
            string json = RefreshTokenRequest.CreateRefreshTokenRequestAsJson(username, token);
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/login/refresh";

            HttpResponseMessage res = await http.SendAsync(Utils.CreateRequest(url, token, Utils.CreateStringContent(json), "Post"));

            if (res.IsSuccessStatusCode)
            {
                string responseContent = await res.Content.ReadAsStringAsync();
                AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);
                Console.WriteLine("Refresh token ricevuto: " + authResponse.GetTokenResponse().RefreshToken);
                StoreTokens(authResponse);
            }
            return res;
        }

        private static void StoreTokens(AuthResponse authResponse)
        {
            TokenWallet.WriteTokens(authResponse.GetTokenResponse());
        }

        private static async Task Login(string username, string password)
        {
            string json = LoginRequestData.CreateLoginRequestDataAsJson(username, password);
            string url = Constants.REST_MULTIDIALOGO_STAGE_HOST + "/users/login";

            HttpResponseMessage response = await http.SendAsync(Utils.CreateRequest(url, null, Utils.CreateStringContent(json), "Post"));
            string responseContent = await response.Content.ReadAsStringAsync();

            AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);
            Console.WriteLine("Nuovo token ricevuto: " + authResponse.GetTokenResponse().Token);

            StoreTokens(authResponse);
        }

    }
}
