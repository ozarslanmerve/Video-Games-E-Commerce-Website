using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.ResponseDTOs;

namespace VideoGames.Business.Abstract
{
    public interface ICartService
    {

        Task<ResponseDTO<CartDTO>> GetCartAsync(string applicationUserId);
        Task<ResponseDTO<IEnumerable<CartDTO>>> GetCartAsync();
        Task<ResponseDTO<CartDTO>> CreateCartAsync(CartCreateDTO cartCreateDTO);

        Task<ResponseDTO<NoContent>> ClearCartAsync(string applicationUserId);

        Task<ResponseDTO<CartItemDTO>> AddVideoGameToCartAsync(CartItemCreateDTO cartItemCreateDTO);

        Task<ResponseDTO<NoContent>> RemoveVideoGameFromCartAsync(int cartItemId);

        Task<ResponseDTO<NoContent>> ChangeVideoGameQuantityAsync(CartItemChangeQuantityDTO cartItemChangeQuantityDTO);
    }
}
