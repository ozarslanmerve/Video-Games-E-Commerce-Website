using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<CategoryDTO>> AddAsync(CategoryCreateDTO categoryCreateDTO)
        {
            Category category = _mapper.Map<Category>(categoryCreateDTO);
            await _unitOfWork.GetRepository<Category>().AddAsync(category);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                return ResponseDTO<CategoryDTO>.Fail("Bir hata oluştu!", StatusCodes.Status500InternalServerError);
            }
            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);
            return ResponseDTO<CategoryDTO>.Success(categoryDTO, StatusCodes.Status201Created);
        }

        public async Task<ResponseDTO<IEnumerable<CategoryDTO>>> GetAllAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            if (categories == null)
            {
                return ResponseDTO<IEnumerable<CategoryDTO>>.Fail("Bir sorun oluştu, daha sonra tekrar deneyiniz", StatusCodes.Status500InternalServerError);
            }

            if (categories.Count() == 0)
            {
                return ResponseDTO<IEnumerable<CategoryDTO>>.Fail("Hiç kategori bulunamadı!", StatusCodes.Status404NotFound);
            }
            var categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return ResponseDTO<IEnumerable<CategoryDTO>>.Success(categoryDTOs, StatusCodes.Status200OK);
        }



        public async Task<ResponseDTO<int>> CountAsync()
        {
            var count = await _unitOfWork.GetRepository<Category>().CountAsync();
            return ResponseDTO<int>.Success(count, StatusCodes.Status200OK);
        }


        public async Task<ResponseDTO<NoContent>> DeleteAsync(int categoryId)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);
            if (category == null)
            {
                return ResponseDTO<NoContent>.Fail("Kategori Bulunamadı", StatusCodes.Status404NotFound);
            }
            _unitOfWork.GetRepository<Category>().Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return ResponseDTO<NoContent>.Success(StatusCodes.Status200OK);
        }



        public async Task<ResponseDTO<CategoryDTO>> GetAsync(int id)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                return ResponseDTO<CategoryDTO>.Fail("Kategori bulunamadı!", StatusCodes.Status404NotFound);
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return ResponseDTO<CategoryDTO>.Success(categoryDTO, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<NoContent>> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
            var existsCategory = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryUpdateDTO.Id);
            if (existsCategory == null)
            {
                return ResponseDTO<NoContent>.Fail("Kategori Bulunamadı", StatusCodes.Status404NotFound);
            }
            _mapper.Map(categoryUpdateDTO, existsCategory);
            _unitOfWork.GetRepository<Category>().Update(existsCategory);
            await _unitOfWork.SaveChangesAsync();
            return ResponseDTO<NoContent>.Success(StatusCodes.Status200OK);
        }

    }
}
