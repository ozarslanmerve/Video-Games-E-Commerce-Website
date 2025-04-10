using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGames.Shared.ComplexTypes
{
    public enum OrderStatus
    {
        Pending = 0, // Beklemede
		Processing = 1, // İşleniyor
		Delivered = 3, // Teslim Edildi
		Cancelled = 4, // İptal Edildi
		Returned = 5, // İade Edildi
		Failed = 6 // Başarısız
    }
}
