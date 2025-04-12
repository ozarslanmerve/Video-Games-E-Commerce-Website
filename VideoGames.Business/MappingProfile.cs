using AutoMapper;
using VideoGames.Entity.Concrete;
using VideoGames.Shared.DTOs;

namespace VideoGames.Business
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Category
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();
            #endregion

            #region VideoGame

            // Normal kullanıcıya özel DTO
            CreateMap<VideoGame, VideoGameDTO>()
                .ForMember(dest => dest.Categories, opt =>
                    opt.MapFrom(src => src.VideoGameCategories.Select(vc => vc.Category)))
                .ReverseMap();

            CreateMap<VideoGame, VideoGameAdminDTO>()
             .ForMember(dest => dest.Categories, opt =>
                 opt.MapFrom(src => src.VideoGameCategories.Select(vc => vc.Category)))
             .ForMember(dest => dest.CDkeys, opt =>
                 opt.MapFrom(src => src.CDkeys)) // Tüm CDkey'leri detaylı şekilde DTO'ya geçir
             .ReverseMap();

            // Create DTO
            CreateMap<VideoGame, VideoGameCreateDTO>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())          // formdan gelir
                .ForMember(dest => dest.VideoGameCDkeys, opt => opt.Ignore())// elle ekleniyor
                .ReverseMap();

            // Update DTO
            CreateMap<VideoGame, VideoGameUpdateDTO>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())  // optional
                .ReverseMap();

            // CDKey mapping (DTO ↔ Entity)
            CreateMap<VideoGameCDkeyDTO, VideoGameCDkey>().ReverseMap();
            CreateMap<VideoGameCDkey, VideoGameCDkeyAddDTO>().ReverseMap();
            CreateMap<VideoGameCDkey, VideoGameCDkeyUpdateDTO>().ReverseMap();

            #endregion

            #region Cart

            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.VideoGame, opt => opt.MapFrom(src => src.VideoGame))
                .ReverseMap();

            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.ApplicationUser, opt => opt.MapFrom(src => src.ApplicationUser))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
                .ReverseMap();

            CreateMap<Cart, CartCreateDTO>().ReverseMap();
            CreateMap<CartItem, CartItemCreateDTO>().ReverseMap();
            CreateMap<CartItem, CartItemUpdateDTO>().ReverseMap();
            CreateMap<CartItemRemoveDTO, CartItem>().ReverseMap();

            #endregion

            #region Order
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.VideoGame, opt => opt.MapFrom(src => src.VideoGame))
                .ForMember(dest => dest.CDKeys, opt => opt.MapFrom(src => src.OrderItemCDKeys
                    .Select(oick => oick.VideoGameCDkey.CDkey)))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemCreateDTO>().ReverseMap();

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.ApplicationUser, opt => opt.MapFrom(src => src.ApplicationUser))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ReverseMap();

            CreateMap<Order, OrderCreateDTO>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ReverseMap();

            #endregion
        }
    }
}
