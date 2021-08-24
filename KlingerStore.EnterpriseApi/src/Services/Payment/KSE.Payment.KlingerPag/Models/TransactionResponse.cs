using System;
using System.Collections.Generic;


namespace KSE.Payment.KlingerPag.Models
{
    public class TransactionResponse
    {
        public string Object { get; set; }
        public string Status { get; set; }
        public object Refuse_reason { get; set; }
        public string Status_reason { get; set; }
        public string Acquirer_response_code { get; set; }
        public string Acquirer_name { get; set; }
        public string Acquirer_id { get; set; }
        public string Authorization_code { get; set; }
        public object Soft_descriptor { get; set; }
        public int Tid { get; set; }
        public int Nsu { get; set; }
        public DateTime Date_created { get; set; }
        public DateTime Date_updated { get; set; }
        public int Amount { get; set; }
        public int Authorized_amount { get; set; }
        public int Paid_amount { get; set; }
        public int Refunded_amount { get; set; }
        public int Installments { get; set; }
        public int Id { get; set; }
        public int Cost { get; set; }
        public string Card_holder_name { get; set; }
        public string Card_last_digits { get; set; }
        public string Card_first_digits { get; set; }
        public string Card_brand { get; set; }
        public object Card_pin_mode { get; set; }
        public bool Card_magstripe_fallback { get; set; }
        public bool Cvm_pin { get; set; }
        public object Postback_url { get; set; }
        public string Payment_method { get; set; }
        public string Capture_method { get; set; }
        public object Antifraud_score { get; set; }
        public object Boleto_url { get; set; }
        public object Boleto_barcode { get; set; }
        public object Boleto_expiration_date { get; set; }
        public string Referer { get; set; }
        public string Ip { get; set; }
        public object Subscription_id { get; set; }
        public object Phone { get; set; }
        public object Address { get; set; }
        public Customer Customer { get; set; }
        public Billing Billing { get; set; }
        public Shipping Shipping { get; set; }
        public List<Item> Items { get; set; }
        public Card Card { get; set; }
        public object Split_rules { get; set; }        
        public object Reference_key { get; set; }
        public object Device { get; set; }
        public object Local_transaction_id { get; set; }
        public object Local_time { get; set; }
        public bool Fraud_covered { get; set; }
        public object Fraud_reimbursed { get; set; }
        public object Order_id { get; set; }
        public string Risk_level { get; set; }
        public object Receipt_url { get; set; }
        public object Payment { get; set; }
        public object Addition { get; set; }
        public object Discount { get; set; }
        public object Private_label { get; set; }
        public object Pix_qr_code { get; set; }
        public object Pix_expiration_date { get; set; }
    }

    public class Document
    {
        public string Object { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
    }

    public class Customer
    {
        public string Object { get; set; }
        public int Id { get; set; }
        public string External_id { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public object Document_number { get; set; }
        public string Document_type { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Phone_numbers { get; set; }
        public object Born_at { get; set; }
        public string Birthday { get; set; }
        public object Gender { get; set; }
        public DateTime Date_created { get; set; }
        public List<Document> Documents { get; set; }
    }

    public class Address
    {
        public string Object { get; set; }
        public string Street { get; set; }
        public object Complementary { get; set; }
        public string Street_number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public int Id { get; set; }
    }

    public class Billing
    {
        public string Object { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }

    public class Shipping
    {
        public string Object { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Fee { get; set; }
        public string Delivery_date { get; set; }
        public bool Expedited { get; set; }
        public Address Address { get; set; }
    }

    public class Item
    {
        public string Object { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public int Unit_price { get; set; }
        public int Quantity { get; set; }
        public object Category { get; set; }
        public bool Tangible { get; set; }
        public object Venue { get; set; }
        public object Date { get; set; }
    }

    public class Card
    {
        public string Object { get; set; }
        public string Id { get; set; }
        public DateTime Date_created { get; set; }
        public DateTime Date_updated { get; set; }
        public string Brand { get; set; }
        public string Holder_name { get; set; }
        public string First_digits { get; set; }
        public string Last_digits { get; set; }
        public string Country { get; set; }
        public string Fingerprint { get; set; }
        public bool Valid { get; set; }
        public string Expiration_date { get; set; }
    }    
}
