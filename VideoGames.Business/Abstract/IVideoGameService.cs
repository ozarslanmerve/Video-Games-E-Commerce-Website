using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.ResponseDTOs;

namespace VideoGames.Business.Abstract
{
    public interface IVideoGameService
    {
        Task<ResponseDTO<VideoGameDTO>> GetAsync(int id);
        Task<ResponseDTO<VideoGameAdminDTO>> GetWithCategoriesAsync(int id);
        Task<ResponseDTO<IEnumerable<VideoGameDTO>>> GetAllAsync();
        Task<ResponseDTO<IEnumerable<VideoGameAdminDTO>>> GetAllWithCategoriesAsync();
        Task<ResponseDTO<IEnumerable<VideoGameDTO>>> GetByCategoryAsync(int categoryId);
        Task<ResponseDTO<VideoGameAdminDTO>> AddAsync(VideoGameCreateDTO videoGameCreateDTO);
        Task<ResponseDTO<VideoGameAdminDTO>> UpdateAsync(VideoGameUpdateDTO videoGameUpdateDTO);
        Task<ResponseDTO<NoContent>> DeleteAsync(int id);
        Task<ResponseDTO<int>> GetCountAsync();
        Task<ResponseDTO<int>> GetCountByCategoryAsync(int categoryId);
        Task<ResponseDTO<IEnumerable<VideoGameAdminDTO>>> GetAllDetailedAsync();


        // Yeni CDKey işlemleri
        Task<ResponseDTO<IEnumerable<VideoGameCDkeyDTO>>> GetCDkeysByGameIdAsync(int videoGameId);
        Task<ResponseDTO<VideoGameCDkeyDTO>> GetCDkeyByIdAsync(int id);
        Task<ResponseDTO<NoContent>> AddCDkeyAsync(VideoGameCDkeyAddDTO videoGameCDkeyAddDTO);
        Task<ResponseDTO<NoContent>> UpdateCDkeyAsync(VideoGameCDkeyUpdateDTO videoGameCDkeyUpdateDTO);
        Task<ResponseDTO<NoContent>> DeleteCDkeyAsync(int id);
    }
}
