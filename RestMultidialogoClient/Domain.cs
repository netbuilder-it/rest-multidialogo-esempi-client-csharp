using System;
using System.Collections.Generic;

namespace RestMultidialogoClient.Domain
{
    public class Email
    {
        public readonly string displayAddress;
        public readonly string notificationAddress;
        public readonly string certifiedAddress;

        public Email(string displayAddress, string notificationAddress, string certifiedAddress)
        {
            this.displayAddress = displayAddress;
            this.notificationAddress = notificationAddress;
            this.certifiedAddress = certifiedAddress;
        }

        public string DisplayAddress => displayAddress;

        public string NotificationAddress => notificationAddress;

        public string CertifiedAddress => certifiedAddress;
    }

    public class Sender
    {
        public readonly string companyName;
        public readonly string streetAddress;
        public readonly string admLvl3;
        public readonly string admLvl2;
        public readonly string country;
        public readonly string zipCode;
        public readonly string vatCode;
        public readonly Email email;

        public Sender(string companyName, string streetAddress, string admLvl3, string admLvl2, string country, string zipCode, string vatCode, Email email)
        {
            this.companyName = companyName;
            this.streetAddress = streetAddress;
            this.admLvl3 = admLvl3;
            this.admLvl2 = admLvl2;
            this.country = country;
            this.zipCode = zipCode;
            this.vatCode = vatCode;
            this.email = email;
        }

        public static Sender CreateSender()
        {
            Email email = new Email(Constants.SENDER_DISPLAY_ADDRESS, Constants.SENDER_NOTIFICATION_ADDRESS, Constants.SENDER_CERTIFIED_ADDRESS);
            return new Sender(Constants.SENDER_COMPANY_NAME,
                    Constants.SENDER_STREET_ADDRESS,
                    Constants.SENDER_ADM_LVL3,
                    Constants.SENDER_ADM_LVL2,
                    Constants.SENDER_COUNTRY,
                    Constants.SENDER_ZIP_CODE,
                    Constants.SENDER_VAT_CODE,
                    email
            );
        }
    }

    public class PrintOptions
    {
        public readonly bool frontBack;
        public readonly string colorMode;
        public readonly string sheetFormat;
        public readonly int? weight;
        public readonly bool staple;
        public readonly bool globalStaple;

        public PrintOptions(bool frontBack, string colorMode, string sheetFormat, int? weight, bool staple, bool globalStaple)
        {
            this.frontBack = frontBack;
            this.colorMode = colorMode;
            this.sheetFormat = sheetFormat;
            this.weight = weight;
            this.staple = staple;
            this.globalStaple = globalStaple;
        }
    }

    public class AttachmentOptions
    {
        public readonly PrintOptions print;

        public AttachmentOptions(PrintOptions print)
        {
            this.print = print;
        }

        public static AttachmentOptions CreateAttachmentOptions(bool frontBack, string colorMode, string sheetFormat, int weight, bool staple, bool globalStaple)
        {
            return new AttachmentOptions(new PrintOptions(frontBack, colorMode, sheetFormat, weight, staple, globalStaple));
        }
    }

    public class File
    {
        public readonly string id;
        public readonly string visibility;
        public readonly AttachmentOptions options;

        public File(string id, string visibility, AttachmentOptions options)
        {
            this.id = id;
            this.visibility = visibility;
            this.options = options;
        }

        public string Id => id;
    }

    public class Attachments
    {
        public readonly string uploadSessionId;
        public readonly List<File> files;

        public Attachments(string uploadSessionId, List<File> files)
        {
            this.uploadSessionId = uploadSessionId;
            this.files = files;
        }

        public static Attachments CreateAttachments(string uploadSessionId, List<File> fileList)
        {
            return new Attachments(uploadSessionId, fileList);
        }

    }

    public class PostalInfoName
    {
        public readonly string type;
        public readonly string firstname;
        public readonly string lastname;
        public readonly string companyName;

        public PostalInfoName(string type, string firstname, string lastname, string companyName)
        {
            this.type = type;
            this.firstname = firstname;
            this.lastname = lastname;
            this.companyName = companyName;
        }
    }

    public class Postage
    {
        public readonly string vector;
        public readonly string type;

        public Postage(string vector, string type)
        {
            this.vector = vector;
            this.type = type;
        }
    }

    public class PostalInfo
    {
        public readonly string streetAddress;
        public readonly string zipCode;
        public readonly string admLvl3;
        public readonly string admLvl2;
        public readonly string countryCode;
        public readonly Postage postage;
        public readonly PostalInfoName name;

        public PostalInfo(string streetAddress, string zipCode, string admLvl3, string admLvl2, string countryCode, Postage postage, PostalInfoName name)
        {
            this.streetAddress = streetAddress;
            this.zipCode = zipCode;
            this.admLvl3 = admLvl3;
            this.admLvl2 = admLvl2;
            this.countryCode = countryCode;
            this.postage = postage;
            this.name = name;
        }

        public static PostalInfo CreatePostalInfo(string streetAddress, string zipCode, string admLvl3, string admLvl2, string countryCode, string postageVector, string postageType, string type, string firstname, string lastname, string companyName)
        {
            Postage postage = postageVector != null ? new Postage(postageVector, postageType) : null;
            return new PostalInfo(streetAddress, zipCode, admLvl3, admLvl2, countryCode, postage, new PostalInfoName(type, firstname, lastname, companyName));
        }

    }

    public class RecipientAttachment
    {
        public readonly List<string> files;

        public RecipientAttachment(List<string> files)
        {
            this.files = files;
        }
    }

    public class Carrier
    {
        public readonly string channel;
        public readonly string alternativeChannel;

        public Carrier(string channel, string alternativeChannel)
        {
            this.channel = channel;
            this.alternativeChannel = alternativeChannel;
        }
    }

    public class Keyword
    {
        public readonly string placeholder;
        public readonly string value;

        public Keyword(string placeholder, string value)
        {
            this.placeholder = placeholder;
            this.value = value;
        }
    }

    public class MessageOptions
    {
        public readonly List<Keyword> keywords;

        public MessageOptions(List<Keyword> keywords)
        {
            this.keywords = keywords;
        }
    }

    public class Recipient
    {
        public readonly string email;
        public readonly string pec;
        public readonly PostalInfo postalInfo;
        public readonly RecipientAttachment attachments;
        public readonly Carrier carrier;
        public readonly MessageOptions messageOptions;
        public readonly List<CustomDataElement> customData;

        public Recipient(string email, string pec, PostalInfo postalInfo, RecipientAttachment attachments, Carrier carrier, MessageOptions messageOptions, List<CustomDataElement> customData)
        {
            this.email = email;
            this.pec = pec;
            this.postalInfo = postalInfo;
            this.attachments = attachments;
            this.carrier = carrier;
            this.messageOptions = messageOptions;
            this.customData = customData;
        }

        private static Recipient CreateRecipient(string email, string pec, PostalInfo postalInfo, List<string> attachmentFiles, string channel, string alternativeChannel, List<Keyword> keywords, List<CustomDataElement> customData)
        {
            MessageOptions messageOptions = keywords != null ? new MessageOptions(keywords) : null;
            return new Recipient(email, pec, postalInfo, new RecipientAttachment(attachmentFiles), new Carrier(channel, alternativeChannel), messageOptions, customData);
        }

        public static Recipient CreateRecipient(
                string streetAddress, string zipCode, string admLvl3, string admLvl2, string countryCode, string postageVector, string postageType, string type, string firstname, string lastname, string companyName,
                string email, string pec, List<string> attachmentFiles, string channel, string alternativeChannel, List<Keyword> keywords)
        {
            PostalInfo postalInfo = PostalInfo.CreatePostalInfo(streetAddress, zipCode, admLvl3, admLvl2, countryCode, postageVector, postageType, type, firstname, lastname, companyName);
            return CreateRecipient(email, pec, postalInfo, attachmentFiles, channel, alternativeChannel, keywords, null);
        }

        public static Recipient CreateRecipient(
                string streetAddress, string zipCode, string admLvl3, string admLvl2, string countryCode, string postageVector, string postageType, string type, string firstname, string lastname, string companyName,
                string email, string pec, List<string> attachmentFiles, string channel, string alternativeChannel, List<Keyword> keywords, List<CustomDataElement> customData)
        {
            PostalInfo postalInfo = PostalInfo.CreatePostalInfo(streetAddress, zipCode, admLvl3, admLvl2, countryCode, postageVector, postageType, type, firstname, lastname, companyName);
            return CreateRecipient(email, pec, postalInfo, attachmentFiles, channel, alternativeChannel, keywords, customData);
        }
    }

    public class Message
    {
        public readonly string subject;
        public readonly string body;

        public Message(string subject, string body)
        {
            this.subject = subject;
            this.body = body;
        }
    }

    public class Billing
    {
        public readonly string invoiceTag;

        public Billing(string invoiceTag)
        {
            this.invoiceTag = invoiceTag;
        }
    }

    public class Multicerta
    {
        public readonly bool legalValue;

        public Multicerta(bool legalValue)
        {
            this.legalValue = legalValue;
        }
    }

    public class Deadline
    {
        public readonly string acknowledgement;
        public readonly int duration;

        public Deadline(string acknowledgement, int duration)
        {
            this.acknowledgement = acknowledgement;
            this.duration = duration;
        }
    }


    public class Postal
    {
        public readonly bool expedite;
        public readonly PrintOptions print;

        public Postal(bool expedite, PrintOptions print)
        {
            this.expedite = expedite;
            this.print = print;
        }
    }

    public class Options
    {
        public readonly Billing billing;
        public readonly Multicerta multicerta;
        public readonly Deadline deadline;
        public readonly Postal postal;

        public Options(Billing billing, Multicerta multicerta, Deadline deadline, Postal postal)
        {
            this.billing = billing;
            this.multicerta = multicerta;
            this.deadline = deadline;
            this.postal = postal;
        }
    }

    public class CustomDataElement
    {
        public readonly string key;
        public readonly string value;
        public readonly string visibility;

        public CustomDataElement(string key, string value, string visibility)
        {
            this.key = key;
            this.value = value;
            this.visibility = visibility;
        }

        public static List<CustomDataElement> CreateCustomData(string key, string value, string visibility)
        {
            List<CustomDataElement> ret = new List<CustomDataElement>();
            ret.Add(new CustomDataElement(key, value, visibility));
            return ret;
        }

    }

    public class PostQueue
    {
        public readonly string type;
        public readonly Sender sender;
        public readonly Attachments attachments;
        public readonly List<Recipient> recipients;
        public readonly Message message;
        public readonly Options options;
        public readonly string topic;
        public readonly List<CustomDataElement> customData;

        public PostQueue(string type, Sender sender, Attachments attachments, List<Recipient> recipients, Message message, Options options, string topic, List<CustomDataElement> customData)
        {
            this.type = type;
            this.sender = sender;
            this.attachments = attachments;
            this.recipients = recipients;
            this.message = message;
            this.options = options;
            this.topic = topic;
            this.customData = customData;
        }

        public static PostQueueDto CreatePostQueue(Sender sender, Attachments attachments, List<Recipient> recipients, string subject, string body, bool useMulticerta, bool useMulticertaLegal, bool staple, bool globalStaple, string topic, List<CustomDataElement> customData)
        {
            Message message = new Message(subject, body);
            Billing billing = new Billing("INVOICE_" + topic.Replace(" ", "_").ToUpper());
            Multicerta multicerta = null;
            Deadline deadline = null;
            if (useMulticerta)
            {
                multicerta = new Multicerta(useMulticertaLegal);
                deadline = new Deadline("read", 3600);
            }
            Postal postal = new Postal(true, new PrintOptions(true, "color", null, null, staple, globalStaple));
            Options options = new Options(billing, multicerta, deadline, postal);
            return new PostQueueDto(new Dto.DataPayload<PostQueue>(new PostQueue("concrete", sender, attachments, recipients, message, options, topic, customData)));
        }
    }

    public class PostQueueDto
    {
        public readonly Dto.DataPayload<PostQueue> data;

        public PostQueueDto(Dto.DataPayload<PostQueue> data)
        {
            this.data = data;
        }
    }

    public class SmsSender
    {
        public string aliasUuid;
        public string phoneNumberUuid;
        public string notificationAddress;


        public SmsSender(string aliasUuid, string phoneNumberUuid, string notificationAddress)
        {
            this.aliasUuid = aliasUuid;
            this.phoneNumberUuid = phoneNumberUuid;
            this.notificationAddress = notificationAddress;
        }

        public static SmsSender createWithAlias(string aliasUuid, string notificationEmailAddress)
        {
            return new SmsSender(aliasUuid, null, notificationEmailAddress);
        }

        public static SmsSender createWithPhoneNumber(string phoneNumberUuid, string notificationEmailAddress)
        {
            return new SmsSender(null, phoneNumberUuid, notificationEmailAddress);
        }
    }

    public class SmsQueueOptions
    {
        public DateTime? scheduleAt;
        public SmsQueueBillingOptions billing;

        public SmsQueueOptions(DateTime? ScheduleAt, SmsQueueBillingOptions billingOptions)
        {
            scheduleAt = ScheduleAt;
            billing = billingOptions;
        }

        public static SmsQueueOptions create(DateTime? scheduleAt, string invoiceTag)
        {
            SmsQueueBillingOptions billingOptions = null;
            if (!String.IsNullOrWhiteSpace(invoiceTag))
            {
                billingOptions = new SmsQueueBillingOptions(invoiceTag);
            }
            return new SmsQueueOptions(scheduleAt, billingOptions);
        }
    }

    public class SmsQueueBillingOptions
    {
        public string invoiceTag;

        public SmsQueueBillingOptions(string invoiceTag)
        {
            this.invoiceTag = invoiceTag;
        }
    }

    public class SmsRecipient
    {
        public string phoneNumber;
        public List<Keyword> keywords;
        public List<CustomDataElement> customData;

        public SmsRecipient(string phoneNumber, List<Keyword> keywords, List<CustomDataElement> customData)
        {
            this.phoneNumber = phoneNumber;
            this.customData = customData;
            this.keywords = keywords;
        }
    }
}
