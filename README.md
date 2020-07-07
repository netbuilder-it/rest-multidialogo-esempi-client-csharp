# API Rest Multidialogo

## Esempi d'uso
Questa è un'applicazione di tipo console scritta in C# 8 che mostra alcuni esempi d'uso significativi delle API di Rest Multidialogo.

## Requisiti
E' richiesto il framework .NET Core 3.0.

## Impostazioni iniziali 
Nel file Constants.cs è necessario impostare una serie di parametri di identificazione, che sono spiegati sotto.

Le credenziali fornite per l'accesso alla piattaforma di stage beta.multidialogo.it:

```
    public const string REST_MULTIDIALOGO_STAGE_USERNAME = "inserire_username";
    public const string REST_MULTIDIALOGO_STAGE_PASSWORD = "inserire_password";
```
I dati del mittente:
```
    public const string SENDER_DISPLAY_ADDRESS = "";
    public const string SENDER_NOTIFICATION_ADDRESS = "";
    public const string SENDER_CERTIFIED_ADDRESS = "";
    public const string SENDER_COMPANY_NAME = "";
    public const string SENDER_STREET_ADDRESS = "";
    public const string SENDER_ADM_LVL3 = "";
    public const string SENDER_ADM_LVL2 = "";
    public const string SENDER_COUNTRY = "";
    public const string SENDER_ZIP_CODE = "";
    public const string SENDER_VAT_CODE = "";
```
L'indirizzo del destinatario che ha la Multicerta attiva (necessario per l'esempio d'uso della Multicerta):
```
    public const string MULTICERTA_ENABLED_ADDRESS = "";
```
I parametri per l'autenticazione del client:
```
    public const string X_API_CLIENT_NAME = "";
    public const string X_API_KEY = "";
    public const string X_API_CLIENT_VERSION = "";
```

Le credenziali di accesso e di identificazione del client verranno fornite separatamente.

## Build e esecuzione
L'applicazione si può compilare con i seguenti comandi da command shell:
```
dotnet clean
dotnet build
```
Per eseguirla:
```
dotnet run
```
