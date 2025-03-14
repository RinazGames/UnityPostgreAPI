namespace UnityPostreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // �������� ������ ����������� �� ���������� ����� Render.
            var connectionString = builder.Configuration["ConnectionStrings__PostgreSqlConnection"]
                ?? throw new InvalidOperationException("������ ����������� �� ������� � ������������.");

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ������������� HTTP ��������.
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
