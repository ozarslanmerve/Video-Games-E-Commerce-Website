namespace VideoGames.MVC.ComplexTypes
{
    public enum OrderStatus
    {
        Pending = 0, // Beklemede
        Processing = 1, // İşleniyor
        Delivered = 2, // Teslim Edildi
        Cancelled = 3, // İptal Edildi
        Returned = 4, // İade Edildi
        Failed = 5// Başarısız
    }
}
