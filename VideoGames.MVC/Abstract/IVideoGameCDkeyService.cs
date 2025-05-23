﻿using VideoGames.MVC.Models;

namespace VideoGames.MVC.Abstract
{
    public interface IVideoGameCDkeyService
    {
        Task<IEnumerable<VideoGameCDkeyModel>> GetCDKeysByGameIdAsync(int videoGameId);
        Task<VideoGameCDkeyModel> GetAsync(int id); 
        Task AddCDKeyAsync(VideoGameCDkeyModel model);
        Task UpdateCDKeyAsync(VideoGameCDkeyModel model);
        Task DeleteCDKeyAsync(int id);

    }
}
