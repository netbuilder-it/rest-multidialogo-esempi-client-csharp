using System;
using System.Configuration;
using RestMultidialogoClient.Dto;

namespace RestMultidialogoClient
{
    static class TokenWallet
    {
        //
        // ATTENZIONE: 
        // i token dovrebbero essere custoditi in maniera sicura. Ad esempio _non_ salvati in chiaro su disco. 
        //

        private static TokenResponse tokens;

        public static TokenResponse GetCurrentTokens()
        {
            return tokens;
        }

        public static void ReadTokens()
        {
            string token = ConfigurationManager.AppSettings["Token"];
            string refreshToken = ConfigurationManager.AppSettings["RefreshToken"];
            SetCurrentTokens(new TokenResponse(token, refreshToken));
        }

        private static void SetCurrentTokens(TokenResponse tokenResponse)
        {
            tokens = tokenResponse;
        }

        public static void WriteTokens(TokenResponse tokenResponse)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings["Token"] == null)
                {
                    settings.Add("Token", tokenResponse.Token);
                }
                else
                {
                    settings["Token"].Value = tokenResponse.Token;
                }
                if (settings["RefreshToken"] == null)
                {
                    settings.Add("RefreshToken", tokenResponse.RefreshToken);
                }
                else
                {
                    settings["RefreshToken"].Value = tokenResponse.RefreshToken;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                SetCurrentTokens(tokenResponse);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
