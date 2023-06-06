
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ASPNetCoreApp.DAL.Models;
using DAL.Models;

namespace ASPNetCoreApp.Controllers
{
    // Указываем тип данных, который контроллер будет возвращать в ответ на запрос
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;

        // Конструктор контроллера, который принимает UserManager, SignInManager и ILogger в качестве параметров
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // Метод для регистрации нового пользователя
        [HttpPost]
        [Route("api/account/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                // Проверяем, являются ли данные, полученные из запроса, валидными
                if (ModelState.IsValid)
                {
                    // Создаем нового пользователя
                    User user = new() { Email = model.Email, UserName = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    // Если пользователь успешно добавлен
                    if (result.Succeeded)
                    {
                        // Устанавливаем роль пользователя
                        await _userManager.AddToRoleAsync(user, "user");
                        // Устанавливаем куки
                        await _signInManager.SignInAsync(user, false);

                        // Логируем успешную регистрацию
                        _logger.LogInformation($"Добавлен новый пользователь: {user.UserName}");

                        // Возвращаем ответ с сообщением об успешной регистрации
                        return Ok(new { message = "Добавлен новый пользователь: " + user.UserName });
                    }
                    else
                    {
                        // Если возникли ошибки при добавлении пользователя, добавляем их в ModelState
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        // Формируем сообщение об ошибке
                        var errorMsg = new
                        {
                            message = "Пользователь не добавлен",
                            error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                        };

                        // Логируем неуспешную регистрацию
                        _logger.LogWarning($"Ошибка при добавлении нового пользователя: {errorMsg}");

                        // Возвращаем ответ с сообщением об ошибке
                        return Created("", errorMsg);
                    }
                }
                else
                {
                    // Если данные, полученные из запроса, не являются валидными, формируем сообщение об ошибке
                    var errorMsg = new
                    {
                        message = "Неверные входные данные",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };

                    // Логируем ошибку валидации модели
                    _logger.LogWarning($"Ошибка валидации модели при регистрации нового пользователя: {errorMsg}");

                    // Возвращаем ответ с сообщением об ошибке
                    return Created("", errorMsg);
                }
            }
            catch (Exception ex)
            {
                // Если возникла ошибка при регистрации нового пользователя, логируем ее и возвращаем ответ с сообщением об ошибке
                _logger.LogError(ex, "Ошибка при регистрации нового пользователя");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ошибка при регистрации нового пользователя" });
            }
        }
        // Обработчик POST-запроса на авторизацию пользователя
        [HttpPost]
        [Route("api/account/login")]
        //[AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                // Логирование попытки входа пользователя
                _logger.LogInformation("Login attempt: email - {email}", model.Email);

                if (ModelState.IsValid)
                {
                    // Авторизация пользователя с помощью ASP.NET Core Identity
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        // Получение текущего пользователя
                        User usr = await GetCurrentUserAsync();
                        if (usr == null)
                        {
                            // Логирование ошибки, если пользователь не найден после успешной авторизации
                            _logger.LogWarning("User not found after successful login: email - {email}", model.Email);
                            return Unauthorized(new
                            {
                                message = "Ошибка выполнения авторизации"
                            });
                        }

                        // Получение ролей пользователя
                        IList<string> roles = await _userManager.GetRolesAsync(usr);
                        string? userRole = roles.FirstOrDefault();

                        // Логирование успешной авторизации пользователя
                        _logger.LogInformation("User logged in: email - {email}, role - {role}", model.Email, userRole);

                        // Возвращение успешного ответа с данными пользователя
                        return Ok(new
                        {
                            message = "Выполнен вход",
                            userName = model.Email,
                            userRole
                        });
                    }
                    else
                    {
                        // Логирование ошибки при неверных учетных данных пользователя
                        _logger.LogWarning("Invalid login attempt: email - {email}", model.Email);
                        return Unauthorized(new
                        {
                            message = "Неверный email или пароль"
                        });
                    }
                }
                else
                {
                    // Логирование ошибки при неверных входных данных
                    var errorMsg = new
                    {
                        message = "Неверные входные данные",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };

                    _logger.LogWarning($"Ошибка валидации модели при попытке входа: {errorMsg}");

                    return Created("", errorMsg);
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки выполнения входа пользователя
                _logger.LogError(ex, "Ошибка при выполнении входа");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ошибка при выполнении входа" });
            }
        }

        // Обработчик POST-запроса для выхода пользователя из системы
        [HttpPost]
        [Route("api/account/logoff")]
        public async Task<IActionResult> LogOff()
        {
            // Получение текущего пользователя
            User usr = await GetCurrentUserAsync();
            if (usr == null)
            {
                // Если пользователь не авторизован, возвращаем ошибку и логируем предупреждение
                _logger.LogWarning("Выход: пользователь не авторизован");
                return Unauthorized(new { message = "Сначала выполните вход" });
            }
            // Удаление куки и логирование успешного выхода из системы
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Выход: пользователь {0} успешно вышел из системы", usr.UserName);
            return Ok(new { message = "Выполнен выход", userName = usr.UserName });
        }

        // Обработчик GET-запроса для проверки авторизации пользователя
        [HttpGet]
        [Route("api/account/isauthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            // Получение текущего пользователя
            User usr = await GetCurrentUserAsync();
            if (usr == null)
            {
                // Если пользователь не авторизован, возвращаем ошибку и логируем предупреждение
                _logger.LogWarning("Проверка авторизации: пользователь не авторизован");
                return Unauthorized(new { message = "Вы Гость. Пожалуйста, выполните вход" });
            }
            // Получение ролей пользователя и логирование успешной проверки авторизации
            IList<string> roles = await _userManager.GetRolesAsync(usr);
            string? userRole = roles.FirstOrDefault();
            _logger.LogInformation("Проверка авторизации: пользователь {0} авторизован, роль пользователя: {1}", usr.UserName, userRole);
            return Ok(new { message = "Сессия активна", userName = usr.UserName, userRole });
        }

        // Вспомогательный метод для получения текущего пользователя
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}