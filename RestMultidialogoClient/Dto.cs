using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RestMultidialogoClient.Dto
{
    public class DataPayload<T>
    {
        public string id;
        public string type;
        public T attributes;

        public string GetId()
        {
            return id;
        }

        public T GetAttributes()
        {
            return attributes;
        }

        public DataPayload(T attributes)
        {
            this.attributes = attributes;
        }
    }

    public class LoginRequest
    {
        public readonly string username;
        public readonly string password;
        public readonly string grantType;

        public LoginRequest(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.grantType = "password";
        }
    }

    public class LoginRequestData
    {
        public DataPayload<LoginRequest> data;

        public LoginRequestData(DataPayload<LoginRequest> data)
        {
            this.data = data;
        }

        private static LoginRequestData CreateLoginRequestData(string username, string password)
        {
            return new LoginRequestData(new DataPayload<LoginRequest>(new LoginRequest(username, password)));
        }

        public static string CreateLoginRequestDataAsJson(string username, string password)
        {
            LoginRequestData loginRequestData = CreateLoginRequestData(username, password);
            return JsonConvert.SerializeObject(loginRequestData);
        }
    }

    public class RefreshToken
    {
        public string username;
        public string refreshToken;
        public string grantType;

        public RefreshToken(string username, string refreshToken)
        {
            this.username = username;
            this.refreshToken = refreshToken;
            this.grantType = "refresh-token";
        }
    }

    public class RefreshTokenRequest
    {
        public DataPayload<RefreshToken> data;

        public RefreshTokenRequest(DataPayload<RefreshToken> data)
        {
            this.data = data;
        }

        private static RefreshTokenRequest CreateRefreshTokenRequest(string username, string refreshToken)
        {
            return new RefreshTokenRequest(new DataPayload<RefreshToken>(new RefreshToken(username, refreshToken)));
        }

        public static string CreateRefreshTokenRequestAsJson(string username, string token)
        {
            RefreshTokenRequest refreshTokenRequest = CreateRefreshTokenRequest(username, token);
            return JsonConvert.SerializeObject(refreshTokenRequest);
        }
    }

    public class TokenResponse
    {
        private string token;
        private string refreshToken;
        private string category;

        public string Token { get => token; set => token = value; }
        public string RefreshToken { get => refreshToken; set => refreshToken = value; }
        public string Category { get => category; set => category = value; }

        public TokenResponse(string token, string refreshToken)
        {
            this.token = token;
            this.refreshToken = refreshToken;
        }
    }

    public class AuthResponse
    {
        public string status;
        public DataPayload<TokenResponse> data;

        public TokenResponse GetTokenResponse()
        {
            return data?.GetAttributes() ?? null;
        }
    }

    public class GenericResponse
    {
        public string status;
        public DataPayload<Object> data;

        public string GetId()
        {
            return data?.GetId();
        }

        public string Status => status;
    }

    public class UploadFileRequest
    {
        public string filename;
        public string fileData;
        public string customId;

        public UploadFileRequest(string filename, string fileData)
        {
            this.filename = filename;
            this.fileData = fileData;
        }
    }

    public class UploadFileRequestData
    {
        public DataPayload<UploadFileRequest> data;

        public UploadFileRequestData(DataPayload<UploadFileRequest> data)
        {
            this.data = data;
        }

        public static UploadFileRequestData CreateUploadFileRequestData(string fileName, string fileContent)
        {
            return new UploadFileRequestData(new DataPayload<UploadFileRequest>(new UploadFileRequest(fileName, fileContent)));
        }
    }

    public class Parameter
    {
        public string parameter;
        public string code;
        public List<string> messages;
    }

    public class Source
    {
        public List<Parameter> parameters;
    }

    public class ErrorResponse
    {
        public string id;
        public string status;
        public string code;
        public string title;
        public string detail;
        public Source source;

        public List<Parameter> GetParameters()
        {
            return source?.parameters ?? new List<Parameter>();
        }
    }

    public class AppPush
    {
        public bool blocked;
        public bool delivered;
        public bool refused;
    }

    public class Credit
    {
        public float serviceThreshold;
        public float postageThreshold;
        public string notificationEmail;
    }

    public class DailyDigest
    {
        public List<string> queue;
    }

    public class Space
    {
        public int threshold;
        public string notificationEmail;
    }

    public class NotificationPreferences
    {
        public AppPush appPush;
        public Credit credit;
        public DailyDigest dailyDigest;
        public Space space;
    }

    public class SenderMulticertaPostageUserPreferences
    {
        public string legalPostageTypeLabel;
        public string postageTypeLabel;
    }

    public class SenderMulticertaDeadlineUserPreferences
    {
        public string acknowledgement;
        public int duration;
        public string legalAcknowledgement;
        public int legalDuration;
    }

    public class SenderMulticertaPreferences
    {
        public SenderMulticertaPostageUserPreferences postage;
        public SenderMulticertaDeadlineUserPreferences deadline;
    }

    public class SenderEmailPreferences
    {
        public List<string> certifiedAddresses;
        public string notificationAddress;
        public string displayAddress;
    }

    public class SenderFaxPreferences
    {
        public string name;
        public string prefix;
        public string number;
        public string notificationEmail;
        public string cover;
    }

    public class SenderSmsPreference
    {
        public string type;
        public string display;
        public string prefix;
        public string number;
        public string alias;
        public string notificationEmail;
    }

    public class SenderLetterWatermarkPreferences
    {
        public int id;
        public string label;
        public bool @default;

        public override string ToString()
        {
            return $"{id} {label} {@default}";
        }
    }

    public class SenderLetterPrintPreferences
    {
        public bool frontBack;
        public string colorMode;
        public string sheetFormat;
        public int weight;
        public bool staple;
    }

    public class SenderLetterPostageVectorsPreferences
    {
        public string type;
        public string vector;
    }

    public class SenderLetterPostageVectorEnabling
    {
        public string vector;
        public string enabledAt;
        public string disabledAt;
    }

    public class SenderLetterPostagePreferences
    {
        public string type;
        public List<SenderLetterPostageVectorsPreferences> vectors;
        public List<SenderLetterPostageVectorEnabling> vectorEnablings;
        public bool topicOnReturnReceipt;
    }

    public class SenderLetterPreferences
    {
        public List<SenderLetterWatermarkPreferences> watermarks;
        public SenderLetterPostagePreferences postage;
        public SenderLetterPrintPreferences print;
        public string zipCode;
        public string streetAddress;
        public string admLvl2;
        public string admLvl3;
        public string countryCodeOrStateName;
        public string notificationEmail;

        public string GetPostageType()
        {
            return postage?.type;
        }
    }

    public class SenderPreferences
    {
        public SenderMulticertaPreferences multicerta;
        public SenderEmailPreferences email;
        public SenderFaxPreferences fax;
        public SenderSmsPreference sms;
        public SenderLetterPreferences letter;

        public List<SenderLetterWatermarkPreferences> GetLoghi()
        {
            return letter?.watermarks ?? new List<SenderLetterWatermarkPreferences>();
        }

        public List<string> GetPec()
        {
            return email?.certifiedAddresses ?? new List<string>();
        }

        public string GetPostageType()
        {
            return letter?.GetPostageType();
        }
    }

    public class MultiboxRecipientEnvelopeRecipientListPreferences
    {
        public string subject;
    }

    public class MultiboxRecipientEnvelopePreferences
    {
        public string package;
        public MultiboxRecipientEnvelopeRecipientListPreferences recipientList;
    }

    public class MultiboxRecipientPreferences
    {
        public string firstname;
        public string lastname;
        public string companyName;
        public string addressee;
        public string streetAddress;
        public string zipCode;
        public string admLvl2;
        public string admLvl3;
        public MultiboxRecipientEnvelopePreferences envelop;
    }

    public class RecipientPreferences
    {
        public MultiboxRecipientPreferences multibox;
    }

    public class UserPreference
    {
        public NotificationPreferences notification;
        public SenderPreferences sender;
        public RecipientPreferences recipient;

        public List<SenderLetterWatermarkPreferences> GetLoghi()
        {
            return sender?.GetLoghi() ?? new List<SenderLetterWatermarkPreferences>();
        }

        public List<string> GetPec()
        {
            return sender?.GetPec() ?? new List<string>();
        }

        public string GetSenderPostageType()
        {
            return sender?.GetPostageType();
        }
    }

    public class UserPreferenceSet
    {
        public int userId;
        public UserPreference preset;
        public UserPreference current;
        public UserPreference requested;
    }

    public class UserPreferencesData
    {
        public DataPayload<UserPreferenceSet> data;

        public List<SenderLetterWatermarkPreferences> GetLoghi()
        {
            List<SenderLetterWatermarkPreferences> ret = new List<SenderLetterWatermarkPreferences>();
            if (data.GetAttributes().requested != null)
            {
                ret.AddRange(data.GetAttributes().requested.GetLoghi());
            }
            if (data.GetAttributes().current != null)
            {
                ret.AddRange(data.GetAttributes().current.GetLoghi());
            }
            if (data.GetAttributes().preset != null)
            {
                ret.AddRange(data.GetAttributes().preset.GetLoghi());
            }
            return ret;
        }

        public List<string> GetPec()
        {
            List<string> ret = new List<string>();
            if (data.GetAttributes().requested != null)
            {
                ret.AddRange(data.GetAttributes().requested.GetPec());
            }
            if (data.GetAttributes().current != null)
            {
                ret.AddRange(data.GetAttributes().current.GetPec());
            }
            if (data.GetAttributes().preset != null)
            {
                ret.AddRange(data.GetAttributes().preset.GetPec());
            }
            return ret;
        }

        public string GetSenderPostageType()
        {
            UserPreferenceSet attributes = data.GetAttributes();
            return attributes.requested?.GetSenderPostageType()
                ?? attributes.current?.GetSenderPostageType()
                ?? attributes.preset?.GetSenderPostageType();
        }
    }

    public class User
    {
        public bool isActive;
        public string group;

        public override string ToString()
        {
            return $"{isActive} {group}";
        }
    }

    public class Profile
    {
        public string id;
        public string type;
    }

    public class ProfileData
    {
        public Profile data;
    }

    public class ProfileEnv
    {
        public ProfileData profile;
    }

    public class UserDto
    {
        public string id;
        public string type;
        public User attributes;
        public ProfileEnv relationships;

        public override string ToString()
        {
            return $"{id} {type} {attributes}";
        }
    }

    public class Meta
    {
        public int total;
    }

    public class UserProfile
    {
        public string username;
        public string displayName;

        public override string ToString()
        {
            return $"{username} {displayName}";
        }
    }

    public class UserExtended
    {
        public string id;
        public User user;
        public UserProfile profile;

        public override string ToString()
        {
            return $"{id} {user} {profile}";
        }
    }

    public class UserResponse
    {
        public string status;
        public Meta meta;
        public List<UserDto> data;
        public List<DataPayload<UserProfile>> included;

        public List<UserExtended> GetUsers()
        {
            List<UserExtended> ret = new List<UserExtended>();
            List<UserDto> users = data ?? new List<UserDto>();
            foreach (UserDto u in users)
            {
                UserExtended ue = new UserExtended();
                ue.id = u.id;
                ue.user = u.attributes;
                ue.profile = included?.Find(x => x.id.Equals(ue.id)).GetAttributes() ?? null;
                ret.Add(ue);
            }
            return ret;
        }
    }

    public class CuTrack
    {
        public string fileData;

        public CuTrack(string fileContent)
        {
            this.fileData = fileContent;
        }
    }

    public class CuPostRequest
    {
        public string label;
        public string countryCode;
        public string billingMode;
        public CuTrack track;
        public string type;

        public CuPostRequest(string fileContent)
        {
            this.label = "Esempio";
            this.countryCode = "it";
            this.billingMode = "CLAIM";
            this.type = "CU";
            this.track = new CuTrack(fileContent);
        }
    }

    public class CuPostRequestDto
    {
        public DataPayload<CuPostRequest> data;

        public CuPostRequestDto(CuPostRequest postRequest)
        {
            this.data = new DataPayload<CuPostRequest>(postRequest);
        }

        public static CuPostRequestDto CreatePostRequestDto(string filename)
        {
            string fileContent = Utils.CreateFileContent(filename, "text/plain");
            return new CuPostRequestDto(new CuPostRequest(fileContent));
        }

        public static string CreatePostRequestDtoAsJson(string filename)
        {
            return JsonConvert.SerializeObject(CreatePostRequestDto(filename));
        }
    }
}
