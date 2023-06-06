using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using ASPNetCoreApp.DAL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Data;

using ASPNetCoreApp.DAL.Repository;

using Microsoft.Extensions.Logging;
using BLL.Interfaces;

namespace ASPNetCoreApp.Controllers
{
    // Контроллер для управления тарифами
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class ТарифsController : ControllerBase
    {
        private readonly IDbCrud _crud; // Зависимость для работы с базой данных
        private readonly ILogger<ТарифsController> _logger; // Зависимость для логирования

        public ТарифsController(IDbCrud newUnitOfWork, ILogger<ТарифsController> logger)
        {
            _crud = newUnitOfWork;
            _logger = logger;
        }

        // Получить все тарифы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ASPNetCoreApp.DAL.Models.Тариф>>> GetAllТариф()
        {
            try
            {
                _logger.LogInformation("GetAllТариф method called"); // Запись в лог о вызове метода
                var Тариф = from s in _crud.GetAllТариф() select s; // Получение всех тарифов из базы данных

                // Добавляем проверку на прошедший месяц
                foreach (var item in Тариф)
                {
                    if (item.Дата_открытия.AddMonths(1) <= DateTime.Now && item.Статус != "Unlocked")
                    {
                        item.Статус = "Unlocked";
                        _crud.UpdateТариф(item);
                    }
                }

                return Тариф.ToList(); // Возвращаем список тарифов
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllТариф method"); // Запись в лог об ошибке
                return BadRequest(ex.Message); // Возвращаем ошибку в виде ответа на запрос
            }
        }

        // Получить тариф по id
        [HttpGet("{id}")]
        public async Task<ActionResult<ASPNetCoreApp.DAL.Models.Тариф>> GetТариф(int id)
        {
            try
            {
                _logger.LogInformation("GetТариф method called with id = {0}", id); // Запись в лог о вызове метода с переданным id
                var Тариф = _crud.GetТариф(id); // Получение тарифа из базы данных по id
                if (Тариф == null)
                {
                    _logger.LogWarning("Тариф with id = {0} not found", id); // Запись в лог об отсутствии тарифа с переданным id
                    return NotFound(); // Возвращаем ответ об отсутствии тарифа
                }
                return Тариф; // Возвращаем найденный тариф
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetТариф method with id = {0}", id); // Запись в лог об ошибке
                return BadRequest(ex.Message); // Возвращаем ошибку в виде ответа на запрос
            }
        }

        // Создать новый тариф
        [HttpPost]
        [Authorize(Roles = "admin")] // Атрибут для авторизации, только пользователи с ролью "admin" могут создавать новый тариф
        public async Task<ActionResult<ASPNetCoreApp.DAL.Models.Тариф>> CreateТариф(ASPNetCoreApp.DAL.Models.Тариф Тариф)
        {
            try
            {
                _logger.LogInformation("NewТариф method called with Тариф = {0}", Тариф); // Запись в лог о вызове метода с переданным тарифом
                if (!ModelState.IsValid) // Проверка валидности модели
                {
                    _logger.LogWarning("Invalid model state in NewТариф method with Тариф = {0}", Тариф); // Запись в лог об ошибке валидации
                    return BadRequest(ModelState); // Возвращаем ошибку в виде ответа на запрос
                }
                _crud.CreateТариф(Тариф); // Создание нового тарифа в базе данных
                _crud.Save(); // Сохранение изменений
                return CreatedAtAction("GetТариф", new { id = Тариф.Код_тарифа }, Тариф); // Возвращаем созданный тариф в виде ответа на запрос
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in NewТариф method with Тариф = {0}", Тариф); // Запись в лог об ошибке
                return BadRequest(ex.Message); // Возвращаем ошибку в виде ответа на запрос
            }
        }

        // PUT: api/Тариф/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateТариф(int id, ASPNetCoreApp.DAL.Models.Тариф Тариф)
        {
            try
            {
                // Проверяем, что id тарифа  совпадает с переданным в параметрах
                if (id != Тариф.Код_тарифа)
                {
                    return BadRequest();
                }
                // Вызываем метод для обновления тарифа в базе данных
                _crud.UpdateТариф(Тариф);
                // Логируем информацию об изменении тарифа
                _logger.LogInformation("Изменен тариф с id " + Тариф.Код_тарифа);
                try
                {
                    // Сохраняем изменения в базе данных
                    _crud.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Если тариф уже был изменен другим пользователем, возвращаем ошибку
                    if (!ТарифExists(id))
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
                _logger.LogError(ex, $"An error occurred while updating Тариф with id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating Тариф with id {id}");
            }
        }

        // Проверка наличия объекта Тариф с переданным id
        private bool ТарифExists(int id)
        {
            return _crud.GetТариф(id) != null;
        }


        // DELETE: api/Тариф/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteТариф(int id)
        {
            try
            {
                // Логирование вызова метода с переданным id
                _logger.LogInformation("DeleteТариф method called with id = {0}", id);

                // Получение объекта Тариф по переданному id
                var тариф = _crud.GetТариф(id);

                // Если объект не найден, логирование ошибки и возврат NotFound
                if (тариф == null)
                {
                    _logger.LogWarning("Тариф with id = {0} not found in DeleteТариф method", id);
                    return NotFound();
                }

                // Вызов метода репозитория для удаления объекта Тариф
                _crud.DeleteТариф(id);
                _crud.Save();

                // Возврат NoContent в случае успешного выполнения
                return NoContent();
            }
            catch (Exception ex)
            {
                // Логирование ошибки и возврат BadRequest с сообщением об ошибке
                _logger.LogError(ex, "Error in DeleteТариф method with id = {0}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}