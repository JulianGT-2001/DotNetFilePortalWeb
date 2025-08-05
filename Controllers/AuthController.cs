using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using fileuploadweb.Models.Dto;
using fileuploadweb.Models.ViewModel;
using fileuploadweb.Negocio.Contrato;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fileuploadweb.Controllers
{
    public class AuthController : Controller
    {
        #region Atributos
        private readonly IAuth _auth;
        #endregion

        #region Constructor
        public AuthController(IAuth auth)
        {
            _auth = auth;
        }
        #endregion

        #region Metodos
        [HttpGet]
        public IActionResult Login()
        {
            LoginDto loginDto = new LoginDto();
            return View(loginDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterDto registerDto = new RegisterDto();
            return View(registerDto);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ActivateAuthenticator()
        {
            string? token = Request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            var usuario = await _auth.ObtenerUsuarioAsync<ApplicationUserDto?>(token);

            if (usuario == null)
                return NotFound();

            await _auth.ReiniciarClaveDeAutenticacionAsync<bool, string>(usuario.email!, token);
            string tokenAuth = await _auth.ObtenerClaveDeAutenticacionAsync<string, string>(usuario.email!, token);

            if (string.IsNullOrEmpty(tokenAuth))
                return Unauthorized();

            var adf = new AutenticacionDosFactoresViewModel
            {
                Token = tokenAuth
            };

            return View(adf);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }


            LoginResponseDto response = await _auth.LoginAsync<LoginResponseDto, LoginDto>(dto);

            if (response == null || string.IsNullOrEmpty(response.token))
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(dto);
            }

            Response.Cookies.Append(
                "jwt_token",
                response.token,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                }
            );
            // Response.Cookies.Append("jwt_token", response.token);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            bool response = await _auth.RegisterAsync<bool, RegisterDto>(dto);

            if (response == false)
            {
                ModelState.AddModelError(string.Empty, "Ha ocurrido un error, intentelo de nuevo.");
                return View(dto);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction(nameof(Login));
        }
        #endregion
    }
}