using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Business.Abstract;
using VideoGames.Data.Abstract;
using VideoGames.Entity.Concrete;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.ResponseDTOs;

namespace VideoGames.Business.Concrete
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IGenericRepository<Cart> cartRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<CartItemDTO>> AddVideoGameToCartAsync(CartItemCreateDTO cartItemCreateDTO)
        {
            var cart = await _cartRepository.GetByIdAsync(
                x => x.ID == cartItemCreateDTO.CartId,
                query => query.Include(c => c.CartItems).ThenInclude(ci => ci.VideoGame));
            if (cart == null)
            {
                return ResponseDTO<CartItemDTO>.Fail("Sepet bulunamadı!", StatusCodes.Status404NotFound);

            }
            var videoGame = await _unitOfWork.GetRepository<VideoGame>().GetByIdAsync(cartItemCreateDTO.VideoGameId);
            if (videoGame == null)
            {
                return ResponseDTO<CartItemDTO>.Fail("Oyun bulunamadı!", StatusCodes.Status404NotFound);
            }
            var existingCartItem = cart.CartItems.FirstOrDefault(x => x.VideoGameId == videoGame.ID);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity = cartItemCreateDTO.Quantity;
                _cartRepository.Update(cart);
                await _unitOfWork.SaveChangesAsync();
                var cartItemUpdateDTO = _mapper.Map<CartItemDTO>(existingCartItem);
                return ResponseDTO<CartItemDTO>.Success(cartItemUpdateDTO, StatusCodes.Status200OK);
            }
            var cartItem = _mapper.Map<CartItem>(cartItemCreateDTO);
            cart.CartItems.Add(cartItem);
            _cartRepository.Update(cart);
            await _unitOfWork.SaveChangesAsync();
            var cartItemDTO = _mapper.Map<CartItemDTO>(cartItem);
            return ResponseDTO<CartItemDTO>.Success(cartItemDTO, StatusCodes.Status201Created);
        }

        public async Task<ResponseDTO<NoContent>> ChangeVideoGameQuantityAsync(CartItemChangeQuantityDTO cartItemChangeQuantityDTO)
        {
            var cartItem = await _unitOfWork.GetRepository<CartItem>().GetByIdAsync(cartItemChangeQuantityDTO.CartItemId);

            if (cartItem == null)
            {
                return ResponseDTO<NoContent>.Fail("Sepette böyle bir oyun bulunamadı!", StatusCodes.Status404NotFound);
            }

            cartItem.Quantity = cartItemChangeQuantityDTO.Quantity;
            _unitOfWork.GetRepository<CartItem>().Update(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return ResponseDTO<NoContent>.Success(StatusCodes.Status200OK);
        }


        public async Task<ResponseDTO<NoContent>> ClearCartAsync(string applicationUserId)
        {
            var cart = await _cartRepository.GetByIdAsync(
                x => x.ApplicationUserId == applicationUserId,
                query => query.Include(c => c.CartItems).ThenInclude(ci => ci.VideoGame));
            if (cart == null)
            {
                return ResponseDTO<NoContent>.Fail("Sepet bulunamadı!", StatusCodes.Status404NotFound);

            }
            cart.CartItems.Clear();
            _cartRepository.Update(cart);
            await _unitOfWork.SaveChangesAsync();
            return ResponseDTO<NoContent>.Success(StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<CartDTO>> CreateCartAsync(CartCreateDTO cartCreateDTO)
        {
            var cart = _mapper.Map<Cart>(cartCreateDTO);
            await _cartRepository.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            var cartDTO = _mapper.Map<CartDTO>(cartCreateDTO);
            return ResponseDTO<CartDTO>.Success(cartDTO, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<CartDTO>> GetCartAsync(string applicationUserId)
        {
            var cart = await _cartRepository.GetByIdAsync(
                c => c.ApplicationUserId == applicationUserId,
                query => query.Include(c => c.CartItems).ThenInclude(ci => ci.VideoGame));

            if (cart == null)
            {
                return ResponseDTO<CartDTO>.Fail("Sepet bulunamadı!", StatusCodes.Status404NotFound);
            }

            var cartDTO = _mapper.Map<CartDTO>(cart);
            return ResponseDTO<CartDTO>.Success(cartDTO, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<IEnumerable<CartDTO>>> GetCartAsync()
        {
            var carts = await _cartRepository.GetAllAsync();
            if (carts == null)
            {
                return
                    ResponseDTO<IEnumerable<CartDTO>>.Fail("Hiç sepet bulunamadı!", StatusCodes.Status400BadRequest);

            }
            var cartDTOs = _mapper.Map<IEnumerable<CartDTO>>(carts);
            return ResponseDTO<IEnumerable<CartDTO>>.Success(cartDTOs, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<NoContent>> RemoveVideoGameFromCartAsync(int cartItemId)
        {
            var cartItem = await _unitOfWork.GetRepository<CartItem>().GetByIdAsync(cartItemId);
            if (cartItem == null)
            {
                return ResponseDTO<NoContent>.Fail("Sepet öğesi bulunamadı!", StatusCodes.Status404NotFound);
            }

            _unitOfWork.GetRepository<CartItem>().Delete(cartItem);
            await _unitOfWork.SaveChangesAsync();
            return ResponseDTO<NoContent>.Success(StatusCodes.Status200OK);
        }
    }
}
