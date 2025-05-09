using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Business.Abstract;
using VideoGames.Business.Helpers;
using VideoGames.Business.Helpers.FileManagement.Abstract;
using VideoGames.Data.Abstract;
using VideoGames.Entity.Concrete;
using VideoGames.Shared.DTOs;
using VideoGames.Shared.ResponseDTOs;

namespace VideoGames.Business.Concrete
{
    public class VideoGameService : IVideoGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<VideoGame> _videoGameRepository;
        private readonly IGenericRepository<VideoGameCDkey> _videoGameCDkeyRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IFileService _fileService;

        public VideoGameService(IUnitOfWork unitOfWork, IMapper mapper, IGenericRepository<VideoGame> videoGameRepository, IGenericRepository<VideoGameCDkey> videoGameCDkeyRepository, IGenericRepository<Category> categoryRepository, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _videoGameRepository = videoGameRepository;
            _videoGameCDkeyRepository = videoGameCDkeyRepository;
            _categoryRepository = categoryRepository;
            _fileService = fileService;
        }

        public async Task<ResponseDTO<VideoGameAdminDTO>> AddAsync(VideoGameCreateDTO videoGameCreateDTO)
        {
            var videoGame = _mapper.Map<VideoGame>(videoGameCreateDTO);

            if (videoGameCreateDTO.Image != null)
            {
                var imageUrl = await _fileService.SaveImageAsync(videoGameCreateDTO.Image);
                videoGame.ImageUrl = imageUrl;
            }

            await _videoGameRepository.AddAsync(videoGame);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
            {
                return ResponseDTO<VideoGameAdminDTO>.Fail("Bir Sorun Oluştu", StatusCodes.Status500InternalServerError);
            }

            // Kategorileri ekle
            videoGame.VideoGameCategories = videoGameCreateDTO.CategoryIds.Select(cId => new VideoGameCategory
            {
                VideoGameId = videoGame.ID,
                CategoryId = cId
            }).ToList();

            // CD Key listesi hazırla
            videoGame.CDkeys = new List<VideoGameCDkey>();

            // Elle girilen CD Key'leri ekle
            foreach (var cdKey in videoGameCreateDTO.VideoGameCDkeys)
            {
                videoGame.CDkeys.Add(new VideoGameCDkey
                {
                    VideoGameId = videoGame.ID,
                    CDkey = cdKey,
                    IsUsed = false
                });
            }

            // Otomatik CD Key üret
            for (int i = 0; i < videoGameCreateDTO.CDKeyCount; i++)
            {
                string newKey = CDKeyGenerator.Generate();

                // Benzersiz olsun diye kontrol et
                while (await _unitOfWork.GetRepository<VideoGameCDkey>().ExistsAsync(k => k.CDkey == newKey))
                {
                    newKey = CDKeyGenerator.Generate();
                }

                videoGame.CDkeys.Add(new VideoGameCDkey
                {
                    VideoGameId = videoGame.ID,
                    CDkey = newKey,
                    IsUsed = false
                });
            }

            // Güncelle ve kaydet
            _videoGameRepository.Update(videoGame);
            result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
            {
                return ResponseDTO<VideoGameAdminDTO>.Fail("Bir Sorun Oluştu", StatusCodes.Status500InternalServerError);
            }

            var videoGameAdminDTO = _mapper.Map<VideoGameAdminDTO>(videoGame);
            return ResponseDTO<VideoGameAdminDTO>.Success(videoGameAdminDTO, StatusCodes.Status201Created);
        }


        public async Task<ResponseDTO<IEnumerable<VideoGameAdminDTO>>> GetAllDetailedAsync()
        {
            var videoGames = await _videoGameRepository.GetAllAsync(
                null,
                null,
                query => query
                    .Include(v => v.VideoGameCategories)
                        .ThenInclude(vc => vc.Category)
                    .Include(v => v.CDkeys)
            );

            if (videoGames == null || !videoGames.Any())
            {
                return ResponseDTO<IEnumerable<VideoGameAdminDTO>>.Fail("Hiç oyun bulunamadı!", StatusCodes.Status404NotFound);
            }

            var videoGameDTOs = _mapper.Map<IEnumerable<VideoGameAdminDTO>>(videoGames);
            return ResponseDTO<IEnumerable<VideoGameAdminDTO>>.Success(videoGameDTOs, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<NoContent>> DeleteAsync(int id)
        {
            var videoGame = await _videoGameRepository.GetByIdAsync(id);
            if (videoGame == null)
            {
                return ResponseDTO<NoContent>.Fail("Oyun bulunamadı!", StatusCodes.Status404NotFound);
            }

            _videoGameRepository.Delete(videoGame);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                return ResponseDTO<NoContent>.Fail("Bir sorun oluştu", StatusCodes.Status500InternalServerError);
            }
            return ResponseDTO<NoContent>.Success(StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<IEnumerable<VideoGameDTO>>> GetAllAsync()
        {
            var videoGames = await _videoGameRepository.GetAllAsync();
            if (videoGames == null || !videoGames.Any())
            {
                return ResponseDTO<IEnumerable<VideoGameDTO>>.Fail("Hiç oyun bulunamadı!", StatusCodes.Status400BadRequest);
            }
            var videoGameDTOs = _mapper.Map<IEnumerable<VideoGameDTO>>(videoGames);
            return ResponseDTO<IEnumerable<VideoGameDTO>>.Success(videoGameDTOs, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<IEnumerable<VideoGameAdminDTO>>> GetAllWithCategoriesAsync()
        {
            var videoGames = await _videoGameRepository.GetAllAsync(null, null, query => query
                .Include(v => v.VideoGameCategories)
                .ThenInclude(vc => vc.Category)
                .Include(v => v.CDkeys));

            if (videoGames == null || !videoGames.Any())
            {
                return ResponseDTO<IEnumerable<VideoGameAdminDTO>>.Fail("Oyun bulunamadı", StatusCodes.Status404NotFound);
            }

            var videoGameDTOs = _mapper.Map<IEnumerable<VideoGameAdminDTO>>(videoGames);
            return ResponseDTO<IEnumerable<VideoGameAdminDTO>>.Success(videoGameDTOs, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<VideoGameDTO>> GetAsync(int id)
        {
            var videoGame = await _videoGameRepository.GetByIdAsync(
                v => v.ID == id,
                query => query
                    .Include(v => v.VideoGameCategories)
                        .ThenInclude(vc => vc.Category)
                    .Include(v => v.CDkeys));

            if (videoGame == null)
            {
                return ResponseDTO<VideoGameDTO>.Fail("Oyun bulunamadı", StatusCodes.Status404NotFound);
            }

            var videoGameDTO = _mapper.Map<VideoGameDTO>(videoGame);
            return ResponseDTO<VideoGameDTO>.Success(videoGameDTO, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<IEnumerable<VideoGameDTO>>> GetByCategoryAsync(int categoryId)
        {
            var videoGames = await _videoGameRepository.GetAllAsync(
                v => v.VideoGameCategories.Any(vc => vc.CategoryId == categoryId),
                null,
                query => query.Include(v => v.VideoGameCategories)
                              .ThenInclude(vc => vc.Category));

            if (videoGames == null || !videoGames.Any())
            {
                return ResponseDTO<IEnumerable<VideoGameDTO>>.Fail("Bu kategoriye ait oyun bulunamadı!", StatusCodes.Status404NotFound);
            }

            var videoGameDTOs = _mapper.Map<IEnumerable<VideoGameDTO>>(videoGames);
            return ResponseDTO<IEnumerable<VideoGameDTO>>.Success(videoGameDTOs, StatusCodes.Status200OK);
        }


        public async Task<ResponseDTO<int>> GetCountAsync()
        {
            var allGames = await _videoGameRepository.GetAllAsync();
            var count = allGames?.Count() ?? 0;
            return ResponseDTO<int>.Success(count, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<int>> GetCountByCategoryAsync(int categoryId)
        {
            var games = await _videoGameRepository.GetAllAsync(
                v => v.VideoGameCategories.Any(vc => vc.CategoryId == categoryId));

            var count = games?.Count() ?? 0;
            return ResponseDTO<int>.Success(count, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<VideoGameAdminDTO>> GetWithCategoriesAsync(int id)
        {
            var videoGame = await _videoGameRepository.GetByIdAsync(
                v => v.ID == id,
                query => query
                    .Include(v => v.VideoGameCategories)
                    .ThenInclude(vc => vc.Category));
          

            if (videoGame == null)
            {
                return ResponseDTO<VideoGameAdminDTO>.Fail("Oyun bulunamadı", StatusCodes.Status404NotFound);
            }

            var videoGameDTO = _mapper.Map<VideoGameAdminDTO>(videoGame);
            return ResponseDTO<VideoGameAdminDTO>.Success(videoGameDTO, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<VideoGameAdminDTO>> UpdateAsync(VideoGameUpdateDTO videoGameUpdateDTO)
        {
            var videoGame = await _videoGameRepository.GetByIdAsync(
                v => v.ID == videoGameUpdateDTO.Id,
                query => query
                    .Include(v => v.VideoGameCategories)
                    .ThenInclude(vc => vc.Category)
                    .Include(v => v.CDkeys));

            if (videoGame == null)
            {
                return ResponseDTO<VideoGameAdminDTO>.Fail("Oyun bulunamadı", StatusCodes.Status404NotFound);
            }

            videoGame.Name = videoGameUpdateDTO.Name;
            videoGame.Description = videoGameUpdateDTO.Description;
            videoGame.Price = videoGameUpdateDTO.Price;
            videoGame.HasAgeLimit = videoGameUpdateDTO.HasAgeLimit;

            videoGame.VideoGameCategories.Clear();
            videoGame.VideoGameCategories = videoGameUpdateDTO.CategoryIds.Select(cId => new VideoGameCategory
            {
                VideoGameId = videoGame.ID,
                CategoryId = cId
            }).ToList();

            if (videoGameUpdateDTO.Image != null)
            {
                var imageUrl = await _fileService.SaveImageAsync(videoGameUpdateDTO.Image, videoGame.ImageUrl);
                videoGame.ImageUrl = imageUrl;
            }

            _videoGameRepository.Update(videoGame);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
            {
                return ResponseDTO<VideoGameAdminDTO>.Fail("Bir hata oluştu", StatusCodes.Status500InternalServerError);
            }

            var videoGameAdminDTO = _mapper.Map<VideoGameAdminDTO>(videoGame);
            return ResponseDTO<VideoGameAdminDTO>.Success(videoGameAdminDTO, StatusCodes.Status200OK);
        }



        //---------------------------------------------------------------------------------------------------
        //                                  CDKEY İŞLEMLERİ



        public async Task<ResponseDTO<IEnumerable<VideoGameCDkeyDTO>>> GetCDkeysByGameIdAsync(int videoGameId)
        {
            var cdKeys = await _videoGameCDkeyRepository.GetAllAsync(k => k.VideoGameId == videoGameId);

            if (cdKeys == null || !cdKeys.Any())
            {
                return ResponseDTO<IEnumerable<VideoGameCDkeyDTO>>.Fail("Bu oyuna ait CD Key bulunamadı!", StatusCodes.Status404NotFound);
            }

            var cdKeyDTOs = _mapper.Map<IEnumerable<VideoGameCDkeyDTO>>(cdKeys);
            return ResponseDTO<IEnumerable<VideoGameCDkeyDTO>>.Success(cdKeyDTOs, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<NoContent>> AddCDkeyAsync(VideoGameCDkeyAddDTO videoGameCDkeyAddDTO)
        {
            var cdKey = new VideoGameCDkey
            {
                VideoGameId = videoGameCDkeyAddDTO.VideoGameId,
                CDkey = videoGameCDkeyAddDTO.CDkey,
                IsUsed = false
            };

            await _videoGameCDkeyRepository.AddAsync(cdKey);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
            {
                return ResponseDTO<NoContent>.Fail("CD Key eklenemedi", StatusCodes.Status500InternalServerError);
            }

            return ResponseDTO<NoContent>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<ResponseDTO<VideoGameCDkeyDTO>> GetCDkeyByIdAsync(int id)
        {
            var cdKey = await _videoGameCDkeyRepository.GetByIdAsync(id);
            if (cdKey == null)
            {
                return ResponseDTO<VideoGameCDkeyDTO>.Fail("CD Key bulunamadı", StatusCodes.Status404NotFound);
            }

            var dto = _mapper.Map<VideoGameCDkeyDTO>(cdKey);
            return ResponseDTO<VideoGameCDkeyDTO>.Success(dto, StatusCodes.Status200OK);
        }

        public async Task<ResponseDTO<NoContent>> UpdateCDkeyAsync(VideoGameCDkeyUpdateDTO videoGameCDkeyUpdateDTO)
        {
            var cdKey = await _videoGameCDkeyRepository.GetByIdAsync(videoGameCDkeyUpdateDTO.Id);
            if (cdKey == null)
            {
                return ResponseDTO<NoContent>.Fail("CD Key bulunamadı", StatusCodes.Status404NotFound);
            }

            cdKey.CDkey = videoGameCDkeyUpdateDTO.CDkey;
            cdKey.IsDeleted = videoGameCDkeyUpdateDTO.IsDeleted;
            cdKey.IsUsed = videoGameCDkeyUpdateDTO.IsUsed;
             _videoGameCDkeyRepository.Update(cdKey);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
            {
                return ResponseDTO<NoContent>.Fail("CD Key güncellenemedi", StatusCodes.Status500InternalServerError);
            }

            return ResponseDTO<NoContent>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<ResponseDTO<NoContent>> DeleteCDkeyAsync(int id)
        {
            var cdKey = await _videoGameCDkeyRepository.GetByIdAsync(id);
            if (cdKey == null)
            {
                return ResponseDTO<NoContent>.Fail("CD Key bulunamadı", StatusCodes.Status404NotFound);
            }

            _videoGameCDkeyRepository.Delete(cdKey);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
            {
                return ResponseDTO<NoContent>.Fail("CD Key silinemedi", StatusCodes.Status500InternalServerError);
            }

            return ResponseDTO<NoContent>.Success(StatusCodes.Status204NoContent);
        }
    }
}
