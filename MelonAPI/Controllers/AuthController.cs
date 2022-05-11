using MelonAPI.Model;
using MelonAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MelonAPI.Controllers
{
    [Route("/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository authRepository;

        private readonly IContextRepository contextRepository;

        public AuthController(IAuthRepository authRepository, IContextRepository contextRepository)
        {
            this.authRepository = authRepository;
            this.contextRepository = contextRepository;
        }

        [HttpPost("/auth/login")]
        public String Login([FromBody] LoginInfo loginInfo)
        {
            return authRepository.Login(loginInfo);
        }

        [HttpPost("/auth/register")]
        public string Register([FromBody] LoginInfo loginInfo)
        {
            return authRepository.Register(loginInfo);
        }

        [HttpPost("/auth/logout")]
        public void Logout()
        {
            Request.Headers.TryGetValue("token", out var token);
            
            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            authRepository.Logout(userId);
        }

    }
}
