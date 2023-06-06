using Microsoft.AspNetCore.Mvc;
using ASPNetCoreApp.DAL.Models;
using ASPNetCoreApp.DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DAL;

using Microsoft.EntityFrameworkCore;
using ASPNetCoreApp.DAL.Models;
using BLL.Interfaces;

namespace ASPNetCoreApp.Controllers
{
    // Указываем маршрут для данного контроллера
    [Route("api/[controller]")]
    // Указываем, что данный класс является контроллером API
    [ApiController]
    public class DogovorsController : ControllerBase
    {
        // Интерфейс для работы с базой данных
        private readonly IDbCrud _crud;
        // Логгер для записи информации о работе приложения
        private readonly ILogger _logger;

        // Конструктор с параметрами
        public DogovorsController(IDbCrud newIDbCrud, ILogger<DogovorsController> logger)
        {
            _crud = newIDbCrud;
            _logger = logger;
        }

        // Метод для получения всех договоров
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ASPNetCoreApp.DAL.Models.Dogovor>>> GetAllDogovor()
        {
            try
            {
                // Записываем информацию о том, что был вызван данный метод
                _logger.LogInformation("You have moved to DogovorController, to the GetAllDogovor() method");
                // Получаем текущее время
                DateTime.UtcNow.ToLongTimeString();
                // Получаем все договоры из базы данных
                var dogovor = from s in _crud.GetAllDogovor() select s;
                // Возвращаем список договоров
                return dogovor.ToList();
            }
            catch (Exception ex)
            {
                // Записываем информацию об ошибке в лог
                _logger.LogError(ex, "An error occurred while getting all Dogovors");
                // Возвращаем ошибку сервера
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting all Dogovors");
            }

        }

        // Метод для получения договора по идентификатору
        [HttpGet("{id}")]
        public async Task<ActionResult<ASPNetCoreApp.DAL.Models.Dogovor>> GetDogovor(int id)
        {
            try
            {
                // Получаем договор из базы данных по идентификатору
                var dogovor = _crud.GetDogovor(id);
                // Если договор не найден, возвращаем ошибку
                if (dogovor == null)
                {
                    _logger.LogInformation($"Dogovor with id {id} not found");
                    return NotFound();
                }
                // Если договор найден, возвращаем его
                _logger.LogInformation($"Dogovor with id {id} found: {dogovor}");
                return dogovor;
            }
            catch (Exception ex)
            {
                // Записываем информацию об ошибке в лог
                _logger.LogError(ex, $"An error occurred while getting Dogovor with id {id}");
                // Возвращаем ошибку сервера
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting Dogovor with id {id}");
            }
        }
        // Контроллер для обновления договора с определенным id
        [HttpPut("{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateDogovor(int id, ASPNetCoreApp.DAL.Models.Dogovor Dogovor)
        {
            try
            {
                // Проверяем, что id договора совпадает с переданным в параметрах
                if (id != Dogovor.Номер_договора)
                {
                    return BadRequest();
                }
                // Вызываем метод для обновления договора в базе данных
                _crud.UpdateDogovor(Dogovor);
                // Логируем информацию об изменении договора
                _logger.LogInformation("Изменен договор с id " + Dogovor.Номер_договора);
                try
                {
                    // Сохраняем изменения в базе данных
                    _crud.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Если договор уже был изменен другим пользователем, возвращаем ошибку
                    if (!DogovorExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Возвращаем успешный результат без содержимого
                return NoContent();
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем ошибку сервера
                _logger.LogError(ex, $"An error occurred while updating Dogovor with id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating Dogovor with id {id}");
            }
        }

        // Контроллер для создания нового договора
        [HttpPost]
        [Authorize(Roles = "user")]
        public IActionResult CreateDogovor(ASPNetCoreApp.DAL.Models.Dogovor dogovor)
        {
            try
            {
                // Проверяем валидность модели
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Получаем тариф по его коду и проверяем, что он не заблокирован
                var tariff = dogovor.Тариф;
                if (tariff.Статус != "Unlocked")
                {
                    _logger.LogError("Неверный код тарифа или тариф заблокирован");
                    return BadRequest("Неверный код тарифа или тариф заблокирован");
                }

                // Получаем клиента по его номеру и проверяем, что у него достаточно средств на балансе
                var client = _crud.GetКлиент(dogovor.Номер_клиента_FK);
                if (client.Баланс < 0)
                {
                    _logger.LogError("Недостаточно средств на балансе клиента");
                    return BadRequest("Недостаточно средств на балансе клиента");
                }
                // Списание абонентской платы
                if (dogovor.Дата_заключения.AddMonths(1) <= DateTime.Now)
                    {
                    client.Баланс = -tariff.Стоимость_перехода;
                    _crud.UpdateКлиент(client);
                    }
                

                // Создаем новый договор в базе данных
                _crud.CreateDogovor(dogovor);
                // Сохраняем изменения в базе данных
                _crud.Save();

                // Возвращаем успешный результат с информацией о созданном договоре
                return CreatedAtAction("GetDogovor", new { id = dogovor.Номер_договора }, dogovor);
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем ошибку сервера
                _logger.LogError(ex, "An error occurred while creating a new Dogovor");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating a new Dogovor");
            }
        }

        // Этот метод проверяет ежемесячную абонентскую плату для договора с заданным id
        [HttpPost("{id}/check")]
        public async Task<IActionResult> CheckDogovor(int id)
        {
            try
            {
                // Получаем договор с заданным id из Unit of Work
                var dogovor = _crud.GetDogovor(id);
                if (dogovor == null)
                {
                    // Если договор не найден, возвращаем ошибку 404 Not Found
                    return NotFound();
                }

                // Получаем клиента для этого договора
                var client = dogovor.Клиент;
                if (client == null)
                {
                    // Если клиент не найден, возвращаем ошибку 404 Not Found
                    return NotFound();
                }

                // Получаем тариф для этого договора
                var tariff = dogovor.Тариф;
                if (tariff == null || tariff.Статус != "Unlocked")
                {
                    // Если тариф не найден или заблокирован, возвращаем ошибку 400 Bad Request
                    _logger.LogError("Неверный код тарифа или тариф неактивен");
                    return BadRequest("Неверный код тарифа или тариф неактивен");
                }

                // Вычисляем количество месяцев, прошедших с момента заключения договора
                var today = DateTime.Today;
                var openDate = dogovor.Дата_заключения;
                var monthsElapsed = (today.Year - openDate.Year) * 12 + today.Month - openDate.Month;
                if (monthsElapsed <= 0)
                {
                    // Если договор еще не начал действовать, возвращаем ошибку 400 Bad Request
                    _logger.LogError("Договор еще не начал действовать");
                    return BadRequest("Договор еще не начал действовать");
                }

                // Снимаем абонентскую плату за каждый прошедший месяц
                for (int i = 1; i <= monthsElapsed; i++)
                {
                    client.Баланс -= tariff.Стоимость_перехода;
                    if (client.Баланс < 0)
                    {
                        // Если на балансе клиента недостаточно средств, возвращаем ошибку 400 Bad Request
                        _logger.LogError("Недостаточно средств на балансе клиента");
                        return BadRequest("Недостаточно средств на балансе клиента");
                    }
                }

                // Обновляем договор в Unit of Work
                _crud.UpdateDogovor(dogovor);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, логируем ее и возвращаем ошибку 500 Internal Server Error
                _logger.LogError(ex, "Ошибка при проверке договора");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка при проверке договора");
            }
        }

        // Этот метод проверяет, существует ли договор с заданным id
        private bool DogovorExists(int id)
        {
            //return _context.CategoryTables.Any(e => e.CategoryId == id);
            return _crud.GetDogovor(id) != null;
        }

        // Этот метод удаляет договор с заданным id
        [HttpDelete("{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteDogovor(int id)
        {
            try
            {
                // Получаем договор с заданным id из Unit of Work
                var dogovor = _crud.GetDogovor(id);
                if (dogovor == null)
                {
                    // Если договор не найден, возвращаем ошибку 404 Not Found
                    return NotFound();
                }
                // Удаляем договор из Unit of Work
                _crud.DeleteDogovor(id);
                _crud.Save();
                _logger.LogInformation("Удален договор с id " + dogovor.Номер_договора);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, логируем ее и возвращаем ошибку 500 Internal Server Error
                _logger.LogError(ex, "Ошибка при удалении договора");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка при удалении договора");
            }
        }
    }
}