namespace UnityPostreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Проверяем, если строка подключения есть в переменной окружения
            var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:PostgreSqlConnection");

            // Если переменная окружения не задана, то используем строку подключения из файла конфигурации
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
            }

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Конфигурируем HTTP пайплайн.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
