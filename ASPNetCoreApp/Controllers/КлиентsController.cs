using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASPNetCoreApp.DAL.Models;

using Microsoft.AspNetCore.Authorization;

using ASPNetCoreApp.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;

namespace ASPNetCoreApp.Controllers
{
    // Контроллер для работы с объектами Клиент
    [Route("api/[controller]")]
    [ApiController]
    public class КлиентsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IDbCrud _crud;

        // Конструктор контроллера
        public КлиентsController(IDbCrud newUnitOfWork, ILogger<КлиентsController> logger)
        {
            _crud = newUnitOfWork;
            _logger = logger;
        }

        // Получение всех объектов Клиент
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ASPNetCoreApp.DAL.Models.Клиент>>> GetAllКлиент()
        {
            try
            {
                _logger.LogInformation("Getting all Клиент");
                var Клиент = from s in _crud.GetAllКлиент() select s;
                return Клиент.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all Клиент: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Получение объекта Клиент по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ASPNetCoreApp.DAL.Models.Клиент>> GetКлиент(int id)
        {
            try
            {
                _logger.LogInformation($"Getting Клиент with ID {id}");
                var Клиент = _crud.GetКлиент(id);
                if (Клиент == null)
                {
                    _logger.LogWarning($"Клиент with ID {id} not found");
                    return NotFound();
                }
                return Клиент;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting Клиент with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Создание нового объекта Клиент
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<ASPNetCoreApp.DAL.Models.Клиент>> Createлиент(ASPNetCoreApp.DAL.Models.Клиент Клиент)
        {
            try
            {
                _logger.LogInformation($"Creating new Клиент with ID {Клиент.Номер_клиента}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _crud.CreateКлиент(Клиент);
                _crud.Save();
                _logger.LogInformation($"New Клиент with ID {Клиент.Номер_клиента} created");
                return CreatedAtAction("GetКлиент", new { id = Клиент.Номер_клиента }, Клиент);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating new Клиент with ID {Клиент.Номер_клиента}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Обновление объекта Клиент по ID
        [HttpPut("{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateКлиент(int id, ASPNetCoreApp.DAL.Models.Клиент Клиент)
        {
            try
            {
                _logger.LogInformation($"Updating Клиент with ID {id}");
                if (id != Клиент.Номер_клиента)
                {
                    return BadRequest();
                }
                _crud.UpdateКлиент(Клиент);
                try
                {
                    _crud.Save();
                    _logger.LogInformation($"Клиент with ID {id} updated");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!КлиентExists(id))
                    {
                        _logger.LogWarning($"Клиент with ID {id} not found");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Клиент with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Проверка существования объекта Клиент по ID
        private bool КлиентExists(int id)
        {
            return _crud.GetКлиент(id) != null;
        }




        // DELETE: api/Клиент/5 - удаление клиента по ID
        [HttpDelete("{id}")]
        [Authorize(Roles = "user")] // авторизация только для пользователей
        public async Task<IActionResult> DeleteКлиент(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting Клиент with ID {id}"); // логирование удаления клиента
                var cat = _crud.GetКлиент(id); // получение клиента по ID
                if (cat == null) // если клиент не найден
                {
                    _logger.LogWarning($"Клиент with ID {id} not found"); // логирование ошибки
                    return NotFound(); // возвращаем ошибку 404
                }
                _crud.DeleteКлиент(id); // удаление клиента
                _crud.Save(); // сохранение изменений
                _logger.LogInformation($"Клиент with ID {id} deleted"); // логирование успешного удаления клиента
                return NoContent(); // возвращаем пустой результат
            }
            catch (Exception ex) // обработка ошибок
            {
                _logger.LogError($"Error deleting Клиент with ID {id}: {ex.Message}"); // логирование ошибки
                return StatusCode(500, "Internal server error"); // возвращаем ошибку 500
            }
        }
    }
}