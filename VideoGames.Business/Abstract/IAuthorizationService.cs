using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Shared.DTOs.Auth;
using VideoGames.Shared.ResponseDTOs;

namespace VideoGames.Business.Abstract
{
    public interface IAuthorizationService
    {
        Task<ResponseDTO<TokenDTO>> LoginAsync(LoginDTO loginDTO);
        Task<ResponseDTO<NoContent>> RegisterAsync(RegisterDTO registerDTO);
    }
}
