
using Application.Applications;
using Application.Interfaces.Applications;
using Application.Interfaces.Repositorios;
using Infra.Repositories;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Registro dos serviços
            builder.Services.AddTransient<ICambioApplication, CambioApplication>();
            builder.Services.AddTransient<IMoedaRepositorio, MoedaRepositorio>();

            //Registro do HTTP
            builder.Services.AddHttpClient("moedaclient", x =>
            {
                x.BaseAddress = new Uri("https://economia.awesomeapi.com.br");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
