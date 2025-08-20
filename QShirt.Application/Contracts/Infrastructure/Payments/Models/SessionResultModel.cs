using AutoMapper;
using Stripe.Checkout;
using System.Text.Json.Serialization;

namespace QShirt.Application.Contracts.Infrastructure.Payments.Models;

public class SessionResultModel
{
    //[JsonPropertyName("id")]
    //public string Id { get; set; }

    //[JsonPropertyName("object")]
    //public string Object { get; set; }

    //[JsonPropertyName("after_expiration")]
    //public object AfterExpiration { get; set; }

    //[JsonPropertyName("allow_promotion_codes")]
    //public object AllowPromotionCodes { get; set; }

    //[JsonPropertyName("amount_subtotal")]
    //public int AmountSubtotal { get; set; }

    [JsonPropertyName("amount_total")]
    public int AmountTotal { get; set; }

    //[JsonPropertyName("billing_address_collection")]
    //public object BillingAddressCollection { get; set; }

    //[JsonPropertyName("cancel_url")]
    //public object CancelUrl { get; set; }

    //[JsonPropertyName("client_reference_id")]
    //public object ClientReferenceId { get; set; }

    //[JsonPropertyName("consent")]
    //public object Consent { get; set; }

    //[JsonPropertyName("consent_collection")]
    //public object ConsentCollection { get; set; }

    //[JsonPropertyName("created")]
    //public DateTime? Created { get; set; }

    //[JsonPropertyName("currency")]
    //public string Currency { get; set; }

    //[JsonPropertyName("custom_fields")]
    //public List<object> CustomFields { get; set; }

    //[JsonPropertyName("custom_text")]
    //public CustomText CustomText { get; set; }

    //[JsonPropertyName("customer")]
    //public object Customer { get; set; }

    //[JsonPropertyName("customer_creation")]
    //public string CustomerCreation { get; set; }

    //[JsonPropertyName("customer_details")]
    //public object CustomerDetails { get; set; }

    //[JsonPropertyName("customer_email")]
    //public object CustomerEmail { get; set; }

    //[JsonPropertyName("expires_at")]
    //public int ExpiresAt { get; set; }

    //[JsonPropertyName("invoice")]
    //public object Invoice { get; set; }

    //[JsonPropertyName("invoice_creation")]
    //public InvoiceCreation InvoiceCreation { get; set; }

    //[JsonPropertyName("livemode")]
    //public bool Livemode { get; set; }

    //[JsonPropertyName("locale")]
    //public object Locale { get; set; }

    //[JsonPropertyName("metadata")]
    //public Metadata Metadata { get; set; }

    //[JsonPropertyName("mode")]
    //public string Mode { get; set; }

    //[JsonPropertyName("payment_intent")]
    //public object PaymentIntent { get; set; }

    //[JsonPropertyName("payment_link")]
    //public object PaymentLink { get; set; }

    //[JsonPropertyName("payment_method_collection")]
    //public string PaymentMethodCollection { get; set; }

    //[JsonPropertyName("payment_method_options")]
    //public PaymentMethodOptions PaymentMethodOptions { get; set; }

    //[JsonPropertyName("payment_method_types")]
    //public List<string> PaymentMethodTypes { get; set; }

    [JsonPropertyName("payment_status")]
    public string PaymentStatus { get; set; }

    //[JsonPropertyName("phone_number_collection")]
    //public PhoneNumberCollection PhoneNumberCollection { get; set; }

    //[JsonPropertyName("recovered_from")]
    //public object RecoveredFrom { get; set; }

    //[JsonPropertyName("setup_intent")]
    //public object SetupIntent { get; set; }

    //[JsonPropertyName("shipping_address_collection")]
    //public object ShippingAddressCollection { get; set; }

    //[JsonPropertyName("shipping_cost")]
    //public object ShippingCost { get; set; }

    //[JsonPropertyName("shipping_details")]
    //public object ShippingDetails { get; set; }

    //[JsonPropertyName("shipping_options")]
    //public List<object> ShippingOptions { get; set; }

    //[JsonPropertyName("status")]
    //public string Status { get; set; }

    //[JsonPropertyName("submit_type")]
    //public object SubmitType { get; set; }

    //[JsonPropertyName("subscription")]
    //public object Subscription { get; set; }

    //[JsonPropertyName("success_url")]
    //public string SuccessUrl { get; set; }

    //[JsonPropertyName("total_details")]
    //public TotalDetails TotalDetails { get; set; }

    //[JsonPropertyName("url")]
    //public string Url { get; set; }


}

public class CustomText
{
    [JsonPropertyName("shipping_address")]
    public object ShippingAddress { get; set; }

    [JsonPropertyName("submit")]
    public object Submit { get; set; }
}

public class InvoiceCreation
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("invoice_data")]
    public InvoiceData InvoiceData { get; set; }
}

public class InvoiceData
{
    [JsonPropertyName("account_tax_ids")]
    public object AccountTaxIds { get; set; }

    [JsonPropertyName("custom_fields")]
    public object CustomFields { get; set; }

    [JsonPropertyName("description")]
    public object Description { get; set; }

    [JsonPropertyName("footer")]
    public object Footer { get; set; }

    [JsonPropertyName("issuer")]
    public object Issuer { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata Metadata { get; set; }

    [JsonPropertyName("rendering_options")]
    public object RenderingOptions { get; set; }
}

public class Metadata
{
}

public class PaymentMethodOptions
{
}

public class PhoneNumberCollection
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}

public class TotalDetails
{
    [JsonPropertyName("amount_discount")]
    public int AmountDiscount { get; set; }

    [JsonPropertyName("amount_shipping")]
    public int AmountShipping { get; set; }

    [JsonPropertyName("amount_tax")]
    public int AmountTax { get; set; }
}


internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Session, SessionResultModel>();
    }
}


