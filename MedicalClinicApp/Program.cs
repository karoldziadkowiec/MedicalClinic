
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MedicalClinicApp.DatabaseHandler;
using MedicalClinicApp.Repositories.Classes;
using MedicalClinicApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database connection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:3000")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPatientSortRepository, PatientSortRepository>();
            builder.Services.AddScoped<IPatientSearchRepository, PatientSearchRepository>();
            builder.Services.AddScoped<IPatientExportRepository, PatientExportRepository>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowSpecificOrigin");

            app.MapControllers();

            app.Run();
        }
    }
}