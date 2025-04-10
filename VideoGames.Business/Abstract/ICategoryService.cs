using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.ResponseDTOs;

namespace VideoGames.Business.Abstract
{
    public interface ICategoryService
    {
        Task<ResponseDTO<CategoryDTO>> GetAsync(int id);
        Task<ResponseDTO<IEnumerable<CategoryDTO>>> GetAllAsync();
        Task<ResponseDTO<CategoryDTO>> AddAsync(CategoryCreateDTO categoryCreateDTO);
        Task<ResponseDTO<NoContent>> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO);
        Task<ResponseDTO<NoContent>> DeleteAsync(int categoryId);
        Task<ResponseDTO<int>> CountAsync();        

    }
}
