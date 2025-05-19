
using System.Text.Json;
using VideoGames.MVC.Abstract;
using VideoGames.MVC.Models;

namespace VideoGames.MVC.Services
{
    public class VideoGameService : BaseService, IVideoGameService
    {
        public VideoGameService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<VideoGameModel> AddAsync(VideoGameCreateModel model)
        {
            var client = GetHttpClient();

            using var form = new MultipartFormDataContent();

            
            form.Add(new StringContent(model.Name), "Name");
            form.Add(new StringContent(model.Description), "Description");
            form.Add(new StringContent(model.Price.ToString()), "Price");

           
            if (model.CategoryIds != null)
            {
                foreach (var categoryId in model.CategoryIds)
                {
                    form.Add(new StringContent(categoryId.ToString()), "CategoryIds");
                }
            }

           
            if (model.CDKeys != null)
            {
                foreach (var key in model.CDKeys)
                {
                    form.Add(new StringContent(key.CDkey), "VideoGameCDkeys");
                }
            }

            
            if (model.Image != null && model.Image.Length > 0)
            {
                var imageContent = new StreamContent(model.Image.OpenReadStream());
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Image.ContentType);
                form.Add(imageContent, "Image", model.Image.FileName);
            }

            
            var response = await client.PostAsync("videoGames/add", form);
            var jsonString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel<VideoGameModel>>(jsonString, _jsonSerializerOptions);

            if (result.Errors != null && result.Errors.Count > 0)
            {
                throw new Exception("Ekleme sırasında hata oluştu: " + string.Join(", ", result.Errors));
            }

            return result.Data;
        }


        public async Task<int> CountAsync()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync("videoGames/count");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Oyun sayısı alınamadı.");
                return 0;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<int>>(jsonString, _jsonSerializerOptions);

            return result?.Data ?? 0;
        }

        public async Task<int> CountByCategoryAsync(int categoryId)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"videoGames/countbycategory/{categoryId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Kategoriye göre oyun sayısı alınamadı.");
                return 0;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseModel<int>>(jsonString, _jsonSerializerOptions);

            return result?.Data ?? 0;
        }

        public async Task DeleteAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"videoGames/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Silme işlemi başarısız: {content}");
                throw new Exception("Oyun silinemedi.");
            }
        }


        public async Task<IEnumerable<VideoGameModel>> GetAllAsync()
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.GetAsync("videoGames");
                var jsonString = await response.Content.ReadAsStringAsync();
                ResponseModel<IEnumerable<VideoGameModel>> result;
                try
                {
                    result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<VideoGameModel>>>(jsonString, _jsonSerializerOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata var");
                    return new List<VideoGameModel>();
                }
                if (result != null && (result.Errors == null || result.Errors.Count == 0))
                {
                    return result.Data;
                }
                else
                {
                    Console.WriteLine("Bir hata var");
                    return new List<VideoGameModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new List<VideoGameModel>();
            }
        }

        public async Task<IEnumerable<VideoGameModel>> GetAllByCategoryAsync(int categoryId)
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.GetAsync($"videoGames/bycategory/{categoryId}");
                var jsonString = await response.Content.ReadAsStringAsync();
                ResponseModel<IEnumerable<VideoGameModel>> result;
                try
                {
                    result = JsonSerializer.Deserialize<ResponseModel<IEnumerable<VideoGameModel>>>(jsonString, _jsonSerializerOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<VideoGameModel>();
                }
                if (result != null && (result.Errors == null || result.Errors.Count == 0))
                {
                    return result.Data;
                }
                else
                {
                    Console.WriteLine("Bir hata var");
                    return new List<VideoGameModel>();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new List<VideoGameModel>();
            }
        }

        public async  Task<VideoGameModel> GetAsync(int id)
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.GetAsync($"videoGames/get/{id}");
                var jsonString = await response.Content.ReadAsStringAsync();
                ResponseModel<VideoGameModel> result;
                try
                {
                    result = JsonSerializer.Deserialize<ResponseModel<VideoGameModel>>(jsonString, _jsonSerializerOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new VideoGameModel();
                }
                if (result != null && (result.Errors == null || result.Errors.Count == 0))
                {
                    return result.Data;
                }
                else
                {
                    Console.WriteLine("Bir hata var");
                    return new VideoGameModel();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new VideoGameModel();
            }
        }

        public async Task UpdateAsync(VideoGameUpdateModel model)
        {
            var client = GetHttpClient();
            var form = new MultipartFormDataContent();

            form.Add(new StringContent(model.Id.ToString()), "Id");
            form.Add(new StringContent(model.Name), "Name");
            form.Add(new StringContent(model.Description), "Description");
            form.Add(new StringContent(model.Price.ToString()), "Price");
            form.Add(new StringContent(model.HasAgeLimit.ToString()), "HasAgeLimit");

            if (model.Image != null)
            {
                var imageContent = new StreamContent(model.Image.OpenReadStream());
                form.Add(imageContent, "Image", model.Image.FileName);
            }

            foreach (var categoryId in model.CategoryIds)
            {
                form.Add(new StringContent(categoryId.ToString()), "CategoryIds");
            }

            var response = await client.PutAsync("videogames", form);

            
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return;

            var jsonString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Sunucu hatası: {response.StatusCode} - {jsonString}");
            }

            var result = JsonSerializer.Deserialize<ResponseModel<VideoGameModel>>(jsonString, _jsonSerializerOptions);

            if (result?.Errors != null && result.Errors.Count > 0)
            {
                throw new Exception(string.Join(", ", result.Errors));
            }
        }



    }
}
