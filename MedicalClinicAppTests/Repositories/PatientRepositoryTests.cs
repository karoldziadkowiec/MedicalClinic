using DocumentFormat.OpenXml.InkML;
using MedicalClinicApp.DatabaseHandler;
using MedicalClinicApp.Models;
using MedicalClinicApp.Repositories.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalClinicAppTests.Repositories
{
    public class PatientRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetAllPatients_ReturnsCorrectAmountOfPatients()
        {
            // Arrange
            var options = GetDbContextOptions("GetAllPatients_ReturnsPatientsOrderedByPesel");
            using (var context = new AppDbContext(options))
            {
                context.Patients.Add(new Patient { Id = 1, FirstName = "Leo", LastName = "Messi", Pesel = "12345678901" });
                context.Patients.Add(new Patient { Id = 2, FirstName = "Cristiano", LastName = "Ronaldo", Pesel = "98765432109" });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientRepository(context);

                // Act
                var result = await repository.GetAllPatients();

                // Assert
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetPatientsByPagination_ReturnsCorrectAmountOfPatients()
        {
            // Arrange
            var options = GetDbContextOptions("GetPatientsByPagination_ReturnsCorrectAmountOfPatients");
            using (var context = new AppDbContext(options))
            {
                for (int i = 1; i <= 10; i++)
                {
                    Address address = new Address
                    {
                        City = "Barcelona",
                        Street = "Camp Nou Street",
                        ZipCode = "12-345"
                    };
                    context.Patients.Add(new Patient { Id = i, FirstName = "Leo", LastName = "Messi", Pesel = $"1234567890{i}", Address = address });
                }
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientRepository(context);

                // Act
                var result = await repository.GetPatientsByPagination(1, 5);

                // Assert
                Assert.Equal(5, result.Count());
            }
        }

        [Fact]
        public async Task GetPatientById_ReturnsCorrectPatient()
        {
            // Arrange
            var options = GetDbContextOptions("GetPatientById_ReturnsCorrectPatient");
            using (var context = new AppDbContext(options))
            {
                Address address = new Address
                {
                    City = "Barcelona",
                    Street = "Camp Nou Street",
                    ZipCode = "12-345"
                };
                context.Patients.Add(new Patient { Id = 1, FirstName = "Leo", LastName = "Messi", Pesel = "12345678901", Address = address });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientRepository(context);

                // Act
                var result = await repository.GetPatientById(1);

                // Assert
                Assert.Equal("Leo", result.FirstName);
            }
        }

        [Fact]
        public async Task AddPatient_AddsPatientToDatabase()
        {
            // Arrange
            var options = GetDbContextOptions("AddPatient_AddsPatientToDatabase"); 
            Address address = new Address
            {
                City = "Barcelona",
                Street = "Camp Nou Street",
                ZipCode = "12-345"
            };
            var patient = new Patient { FirstName = "Leo", LastName = "Messi", Pesel = "12345678901", Address = address };

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientRepository(context);

                // Act
                await repository.AddPatient(patient);
            }

            using (var context = new AppDbContext(options))
            {
                // Assert
                var result = await context.Patients.FirstOrDefaultAsync(p => p.Pesel == "12345678901");
                Assert.NotNull(result);
                Assert.Equal("Leo", result.FirstName);
                Assert.Equal("Messi", result.LastName);
            }
        }

        [Fact]
        public async Task UpdatePatient_UpdatesExistingPatient()
        {
            // Arrange
            var options = GetDbContextOptions("UpdatePatient_UpdatesExistingPatient");
            using (var context = new AppDbContext(options))
            {
                Address address = new Address
                {
                    City = "Barcelona",
                    Street = "Camp Nou Street",
                    ZipCode = "12-345"
                };
                context.Patients.Add(new Patient { Id = 1, FirstName = "Leo", LastName = "Messi", Pesel = "12345678901", Address = address });
                await context.SaveChangesAsync();
            }

            Address address2 = new Address
            {
                City = "Barcelona",
                Street = "Camp Nou Street",
                ZipCode = "12-345"
            };
            var updatedPatient = new Patient { Id = 1, FirstName = "Lionel", LastName = "Messi", Pesel = "12345678901", Address = address2 };

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientRepository(context);

                // Act
                await repository.UpdatePatient(updatedPatient);
            }

            using (var context = new AppDbContext(options))
            {
                // Assert
                var result = await context.Patients.FindAsync(1);
                Assert.Equal("Lionel", result.FirstName);
            }
        }

        [Fact]
        public async Task DeletePatient_RemovesPatientFromDatabase()
        {
            // Arrange
            var options = GetDbContextOptions("DeletePatient_RemovesPatientFromDatabase");
            using (var context = new AppDbContext(options))
            {
                Address address = new Address
                {
                    City = "Barcelona",
                    Street = "Camp Nou Street",
                    ZipCode = "12-345"
                };
                context.Patients.Add(new Patient { Id = 1, FirstName = "Leo", LastName = "Messi", Pesel = "12345678901", Address = address });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientRepository(context);

                // Act
                await repository.DeletePatient(1);
            }

            using (var context = new AppDbContext(options))
            {
                // Assert
                var result = await context.Patients.FindAsync(1);
                Assert.Null(result);
            }
        }
    }
}