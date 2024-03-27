using MedicalClinicApp.DatabaseHandler;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalClinicAppTests.Repositories
{
    public class PatientExportRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetPatientsCsvBytes_ReturnsCorrectByteArray()
        {
            // Arrange
            var options = GetDbContextOptions("GetPatientsCsvBytes_ReturnsCorrectByteArray");
            using (var context = new AppDbContext(options))
            {
                Address address = new Address
                {
                    City = "Barcelona",
                    Street = "Camp Nou Street",
                    ZipCode = "12-345"
                };
                context.Patients.Add(new Patient { Id = 1, FirstName = "Leo", LastName = "Messi", Pesel = "12345678901", Address = address });
                context.Patients.Add(new Patient { Id = 2, FirstName = "Cristiano", LastName = "Ronaldo", Pesel = "98765432109", Address = address });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientExportRepository(context);

                // Act
                var result = await repository.GetPatientsCsvBytes();

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Length > 0);
            }
        }
    }
}