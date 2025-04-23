namespace MultiShop.RapidApiWebUI.Models
{
    public class ECommerceViewModel
    {

        public class Rootobject
        {
            public string status { get; set; }
            public string request_id { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public Product[] products { get; set; }
            public object[] sponsored_products { get; set; }
            public object[] filters { get; set; }
        }

        public class Product
        {
            public string product_id { get; set; }
            public string product_title { get; set; }
            public string product_description { get; set; }
            public string[] product_photos { get; set; }
            public Product_Videos[] product_videos { get; set; }
            public Product_Attributes product_attributes { get; set; }
            public float product_rating { get; set; }
            public string product_page_url { get; set; }
            public int product_num_reviews { get; set; }
            public int product_num_offers { get; set; }
            public string[] typical_price_range { get; set; }
            public Current_Product_Variant_Properties current_product_variant_properties { get; set; }
            public Product_Variants product_variants { get; set; }
            public Offer offer { get; set; }
        }

        public class Product_Attributes
        {
            public string Seri { get; set; }
            public string PaketBoyutları { get; set; }
            public string GüçKaynağı { get; set; }
            public string DonanımPlatformu { get; set; }
            public string İşletimSistemi { get; set; }
            public string OrtalamaPilÖmrüsaat { get; set; }
            public string SatışaSunulduğuİlkTarih { get; set; }
            public string ElYönü { get; set; }
            public string ModelAdı { get; set; }
            public string ÜrünİçinÖnerilenKullanımAlanları { get; set; }
            public string UyumluCihazlar { get; set; }
            public string Stil { get; set; }
            public string PilÖmrü { get; set; }
            public string Aralık { get; set; }
            public string PilSayısı { get; set; }
            public string BirlikteGelenBileşenler { get; set; }
            public string BirimSayısı { get; set; }
            public string Marka { get; set; }
            public string Programlanabilir { get; set; }
            public string Bağlantı { get; set; }
            public string TürYeni { get; set; }
            public string DüğmeSayısı { get; set; }
            public string Duyarlılık { get; set; }
            public string Renk { get; set; }
            public string TuşSayısı { get; set; }
            public string Hassasiyet { get; set; }
            public string BağlantıTürü { get; set; }
            public string MouseTipi { get; set; }
            public string Özellik { get; set; }
            public string Işıklı { get; set; }
            public string Görünüm { get; set; }
            public string Makrolu { get; set; }
            public string Elkullanımı { get; set; }
            public string MenşeÜlkesi { get; set; }
            public string ÜrünAğırlığı { get; set; }
            public string LityumBataryaPaketi { get; set; }
            public string KaplamaTürü { get; set; }
            public string Uyumluluk { get; set; }
            public string Hareketçözünürlüğü { get; set; }
            public string Dönenteker { get; set; }
            public string Hareketalgılamateknolojisi { get; set; }
            public string Tuşsayısı { get; set; }
            public string Araçarayüzü { get; set; }
            public string Ürünrengi { get; set; }
            public string Kablouzunluğu { get; set; }
            public string Desteklenendiğerçalışmasistemleri { get; set; }
            public string DesteklenenLinuxçalışmasistemleri { get; set; }
            public string DesteklenenMacçalışmasistemleri { get; set; }
            public string DesteklenenWindowsçalışmasistemleri { get; set; }
            public string Paketağırlığı { get; set; }
            public string Ambalajyüksekliği { get; set; }
            public string Ambalajderinliği { get; set; }
            public string Ambalajgenişliği { get; set; }
            public string Mastırkartonağırlığı { get; set; }
            public string Anakartonuzunluğu { get; set; }
            public string Mastırkartonuzunluğu { get; set; }
            public string Anakartongenişliği { get; set; }
            public string Uyumluişletimsistemi { get; set; }
            public string Donanımönkoşulları { get; set; }
            public string WattDeğeri { get; set; }
            public string ABYedekParçaBulunabilirlikSüresi { get; set; }
            public string Sensör { get; set; }
        }

        public class Current_Product_Variant_Properties
        {
            public string Renk { get; set; }
        }

        public class Product_Variants
        {
            public Renk[] Renk { get; set; }
        }

        public class Renk
        {
            public string name { get; set; }
            public string thumbnail { get; set; }
            public string product_id { get; set; }
        }

        public class Offer
        {
            public string offer_id { get; set; }
            public string offer_title { get; set; }
            public string offer_page_url { get; set; }
            public string price { get; set; }
            public string shipping { get; set; }
            public bool on_sale { get; set; }
            public object original_price { get; set; }
            public string product_condition { get; set; }
            public string store_name { get; set; }
            public object store_rating { get; set; }
            public int store_review_count { get; set; }
            public string store_reviews_page_url { get; set; }
            public string store_favicon { get; set; }
            public object payment_methods { get; set; }
            public string offer_badge { get; set; }
        }

        public class Product_Videos
        {
            public string title { get; set; }
            public string url { get; set; }
            public string source { get; set; }
            public string publisher { get; set; }
            public string thumbnail { get; set; }
            public string preview_url { get; set; }
            public int duration_ms { get; set; }
        }

    }
}
