namespace UnityPostreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Получаем строку подключения из переменных среды Render.
            var connectionString = builder.Configuration["ConnectionStrings__PostgreSqlConnection"]
                ?? throw new InvalidOperationException("Строка подключения не найдена в конфигурации.");

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
