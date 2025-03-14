using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace UnityPostreAPI.Controllers
{
    [Route("api/sqlquery")]
    [ApiController]
    public class SqlQueryController : ControllerBase
    {
        // Строка подключения к базе данных.
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор конфигурации для инициализации строки подключения.
        /// </summary>
        public SqlQueryController(IConfiguration configuration)
        {
            // Получение строки подключения.
            _connectionString = configuration.GetConnectionString("PostgreSqlConnection")
                ?? throw new InvalidOperationException("Строка подключения не найдена в конфигурации.");
        }

        /// <summary>
        /// Метод для отправки запроса в базу данных.
        /// </summary>
        /// <param name="query">SQL-запрос</param>
        /// <returns>Результат выполнения</returns>
        [HttpPost("execute")]
        public IActionResult SendQuery([FromForm] string query)
        {
            try
            {
                // Попытка выполнение команды.
                ExecuteNonQuery(query);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Получение данных из БД
        /// </summary>
        [HttpGet("getdata")]
        public IActionResult GetData([FromQuery] string query)
        {
            try
            {
                // Пробуем получить данные.
                var result = ExecuteQuery(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Выполняет запрос без возврата данных (INSERT, UPDATE, DELETE)
        /// </summary>
        private void ExecuteNonQuery(string query)
        {
            // Создаём подключение.
            using var connection = new NpgsqlConnection(_connectionString);
            // Синхронное подключение.
            connection.Open(); 

            // Инициализируем команду.
            using var command = new NpgsqlCommand(query, connection);
            // Синхронное выполнение команды.
            command.ExecuteNonQuery(); 
        }

        /// <summary>
        /// Выполняет запрос и возвращает результат (SELECT)
        /// </summary>
        private List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            // Создаём подключение.
            using var connection = new NpgsqlConnection(_connectionString);
            // Синхронное подключение.
            connection.Open();

            // Инициализируем команду.
            using var command = new NpgsqlCommand(query, connection);
            // Синхронное выполнение запроса.
            using var reader = command.ExecuteReader(); 

            // Результат в виде списка словарей.
            var results = new List<Dictionary<string, object>>();
            // Пока есть результаты.
            while (reader.Read())
            {
                // Создаём ряд.
                var row = new Dictionary<string, object>();
                // Инициализируем ряд.
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                // Добавляем в общий список.
                results.Add(row);
            }

            return results;
        }
    }
}