using System.Collections.Generic;

namespace KSE.Payment.KlingerPag.Models
{
    public class TransactionViewModel
    {        
        public string api_key { get; set; }
        public bool capture { get; set; }
        public string amount { get; set; }
        public string card_number { get; set; }
        public string card_cvv { get; set; }
        public string card_expiration_date { get; set; }
        public string card_holder_name { get; set; }
        public CustomerViewModel customer { get; set; }
        public BillingViewModel billing { get; set; }
        public ShippingViewModel shipping { get; set; }
        public List<ItemViewModel> items { get; set; }
    }

    public class DocumentViewModel
    {
        public string type { get; set; }
        public string number { get; set; }
    }

    public class CustomerViewModel
    {
        public string external_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public List<DocumentViewModel> documents { get; set; }
        public List<string> phone_numbers { get; set; }
        public string birthday { get; set; }
    }

    public class AddressViewModel
    {
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string neighborhood { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
        public string zipcode { get; set; }
    }

    public class BillingViewModel
    {
        public string name { get; set; }
        public AddressViewModel address { get; set; }
    }

    public class ShippingViewModel
    {
        public string name { get; set; }
        public string fee { get; set; }
        public string delivery_date { get; set; }
        public bool expedited { get; set; }
        public AddressViewModel address { get; set; }
    }

    public class ItemViewModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string unit_price { get; set; }
        public int quantity { get; set; }
        public bool tangible { get; set; }
    }
}
