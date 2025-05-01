using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using VideoGames.Business.Abstract;
using VideoGames.Data.Abstract;
using VideoGames.Entity.Concrete;
using VideoGames.Shared.ComplexTypes;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.ResponseDTOs;

public class OrderService : IOrderService
{
    private readonly IGenericRepository<Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;
    private readonly IGenericRepository<VideoGameCDkey> _cdKeyRepository;
    private readonly IGenericRepository<OrderItemCDKey> _orderItemCdKeyRepository;

    public OrderService(IGenericRepository<Order> orderRepository, IUnitOfWork unitOfWork, IMapper mapper, ICartService cartService, IGenericRepository<VideoGameCDkey> cdKeyRepository, IGenericRepository<OrderItemCDKey> orderItemCdKeyRepository)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cartService = cartService;
        _cdKeyRepository = cdKeyRepository;
        _orderItemCdKeyRepository = orderItemCdKeyRepository;
    }
    public async Task<ResponseDTO<OrderDTO>> CreateOrderAsync(OrderCreateDTO dto)
    {
        // 1. Her oyun için yeterli CD key var mı kontrolü
        foreach (var item in dto.OrderItems)
        {
            var game = await _unitOfWork.GetRepository<VideoGame>()
                .GetByIdAsync(g => g.ID == item.VideoGameId, query => query.Include(g => g.CDkeys));

            if (game == null)
                return ResponseDTO<OrderDTO>.Fail($"{item.VideoGameId} ID'li oyun bulunamadı", StatusCodes.Status404NotFound);

            var availableKeys = game.CDkeys.Where(k => !k.IsUsed).Take(item.Quantity).ToList();

            if (availableKeys.Count < item.Quantity)
                return ResponseDTO<OrderDTO>.Fail($"{game.Name} için yeterli CD Key yok", StatusCodes.Status400BadRequest);
        }

        // 2. Siparişi oluştur (mapper ile dönüştür)
        var order = _mapper.Map<Order>(dto);

        // 3. Fiyatları belirle ve toplam tutarı hesapla
        decimal totalAmount = 0;
        foreach (var orderItem in order.OrderItems)
        {
            var game = await _unitOfWork.GetRepository<VideoGame>().GetByIdAsync(orderItem.VideoGameId);
            orderItem.UnitPrice = game.Price;
            totalAmount += orderItem.Quantity * orderItem.UnitPrice;
        }
        order.TotalAmount = totalAmount;

        // 4. Order kaydı (ID'ler oluşması için)
        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(); // ❗️OrderItem.ID’ler burada oluşur

        // 5. Mevcut atanmış CD Key ID’lerini al
        var allOrderItemCdKeys = await _orderItemCdKeyRepository.GetAllAsync();
        var assignedKeyIds = new HashSet<int>(allOrderItemCdKeys.Select(c => c.VideoGameCDkeyId));

        // 6. CD Key ataması yap
        foreach (var orderItem in order.OrderItems)
        {
            var availableKeys = await _cdKeyRepository.GetAllAsync(
                k => k.VideoGameId == orderItem.VideoGameId &&
                     !k.IsDeleted && !k.IsUsed &&
                     !assignedKeyIds.Contains(k.ID)
            );

            var selectedKeys = availableKeys.Take(orderItem.Quantity).ToList();

            if (selectedKeys.Count < orderItem.Quantity)
            {
                return ResponseDTO<OrderDTO>.Fail("CD Key ataması sırasında beklenmeyen bir hata oluştu", StatusCodes.Status500InternalServerError);
            }

            foreach (var key in selectedKeys)
            {
                key.IsUsed = true;
                _cdKeyRepository.Update(key);

                await _orderItemCdKeyRepository.AddAsync(new OrderItemCDKey
                {
                    OrderItemId = orderItem.ID,
                    VideoGameCDkeyId = key.ID
                });
            }
        }

        // 7. CD Key güncellemelerini ve eşleşmeleri kaydet
        await _unitOfWork.SaveChangesAsync();

        // 8. Sepeti temizle
        await _cartService.ClearCartAsync(order.ApplicationUserId);

        // 9. DTO’ya dönüştür ve dön
        var resultDto = _mapper.Map<OrderDTO>(order);
        return ResponseDTO<OrderDTO>.Success(resultDto, StatusCodes.Status201Created);
    }



    public async Task<ResponseDTO<NoContent>> DeleteOrderAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            return ResponseDTO<NoContent>.Fail("İlgili sipariş bulunamadı", StatusCodes.Status404NotFound);

        _orderRepository.Delete(order);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDTO<NoContent>.Success(StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<OrderDTO>> GetOrderAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(
            x => x.ID == id,
            query => query.Include(o => o.OrderItems)
                          .ThenInclude(oi => oi.VideoGame));

        if (order == null)
            return ResponseDTO<OrderDTO>.Fail("İlgili sipariş bulunamadı", StatusCodes.Status404NotFound);

        var orderDto = _mapper.Map<OrderDTO>(order);

        // CD Key'leri her OrderItem için getir ve DTO'ya ekle
        foreach (var itemDto in orderDto.OrderItems)
        {
            var keys = await _orderItemCdKeyRepository.GetAllAsync(
                k => k.OrderItemId == itemDto.Id,
                null,
                q => q.Include(x => x.VideoGameCDkey));

            itemDto.CDKeys = keys.Select(k => k.VideoGameCDkey.CDkey).ToList();
        }

        return ResponseDTO<OrderDTO>.Success(orderDto, StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<IEnumerable<OrderDTO>>> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        if (orders == null || !orders.Any())
            return ResponseDTO<IEnumerable<OrderDTO>>.Fail("Hiçbir Sipariş Bulunamadı", StatusCodes.Status404NotFound);

        var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(orders);
        return ResponseDTO<IEnumerable<OrderDTO>>.Success(orderDTOs, StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<IEnumerable<OrderDTO>>> GetOrdersAsync(OrderStatus orderStatus)
    {
        var orders = await _orderRepository.GetAllAsync(
            x => x.OrderStatus == orderStatus,
            x => x.OrderByDescending(o => o.CreatedDate),
            query => query.Include(o => o.OrderItems).ThenInclude(oi => oi.VideoGame));

        if (!orders.Any())
            return ResponseDTO<IEnumerable<OrderDTO>>.Fail("Belirtilen Duruma Ait Hiçbir Sipariş Bulunamadı", StatusCodes.Status404NotFound);

        var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(orders);
        return ResponseDTO<IEnumerable<OrderDTO>>.Success(orderDTOs, StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<IEnumerable<OrderDTO>>> GetOrdersAsync(string applicationUserId)
    {
        var orders = await _orderRepository.GetAllAsync(
            x => x.ApplicationUserId == applicationUserId,
            x => x.OrderByDescending(o => o.CreatedDate),
            query => query.Include(o => o.OrderItems).ThenInclude(oi => oi.VideoGame));

        if (!orders.Any())
            return ResponseDTO<IEnumerable<OrderDTO>>.Fail("Kullanıcıya Ait Sipariş Bulunamadı", StatusCodes.Status404NotFound);

        var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(orders);
        return ResponseDTO<IEnumerable<OrderDTO>>.Success(orderDTOs, StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<IEnumerable<OrderDTO>>> GetOrdersAsync(DateTime startDate, DateTime endDate)
    {
        var orders = await _orderRepository.GetAllAsync(
            x => x.CreatedDate >= startDate && x.CreatedDate <= endDate,
            x => x.OrderByDescending(o => o.CreatedDate),
            query => query.Include(o => o.OrderItems).ThenInclude(oi => oi.VideoGame));

        if (!orders.Any())
            return ResponseDTO<IEnumerable<OrderDTO>>.Fail("Belirtilen tarih aralığında sipariş bulunamadı", StatusCodes.Status404NotFound);

        var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(orders);
        return ResponseDTO<IEnumerable<OrderDTO>>.Success(orderDTOs, StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<string>> GetOrderStatusAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            return ResponseDTO<string>.Fail("Sipariş Bulunamadı", StatusCodes.Status404NotFound);

        return ResponseDTO<string>.Success(order.OrderStatus.ToString(), StatusCodes.Status200OK);
    }

    public async Task<ResponseDTO<NoContent>> UpdateOrderStatusAsync(int orderId, OrderStatus orderStatus)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            return ResponseDTO<NoContent>.Fail("Sipariş bulunamadı!", StatusCodes.Status404NotFound);

        order.OrderStatus = orderStatus;
        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDTO<NoContent>.Success(StatusCodes.Status200OK);
    }

    public Task<ResponseDTO<NoContent>> UpdateOrderAsync(OrderUpdateDTO dto)
    {
        // Opsiyonel olarak yazılabilir, CD key mantığına göre dikkatli ele alınmalı.
        throw new NotImplementedException("CD Key'li siparişlerde güncelleme önerilmez.");
    }
}
