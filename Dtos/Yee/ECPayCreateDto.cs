namespace HotelFuen31.APIs.Dtos.Yee
{
    public class ECPayCreateDto
    {
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public string MerchantTradeDate { get; set; }
        public string PaymentType { get; set; }
        public string TotalAmount { get; set; }
        public string TradeDesc { get; set; }
        public string ItemName { get; set; }
        public string ReturnURL { get; set; }
        public string ChoosePayment { get; set; }
        public string CheckMacValue { get; set; }
        public string EncryptType { get; set; }
        public string ClientBackURL { get; set; }
        public string OrderResultURL { get; set; }
    }
}
