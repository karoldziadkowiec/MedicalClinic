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
    public class PatientSearchRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task SearchPatients_ReturnsPatientsByExactFirstName()
        {
            // Arrange
            var options = GetDbContextOptions("SearchPatients_ReturnsPatientsByExactFirstName");
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
                var repository = new PatientSearchRepository(context);

                // Act
                var result = await repository.SearchPatients("Leo");

                // Assert
                var patients = result.ToList();
                Assert.Single(patients);
                Assert.Equal("Leo", patients[0].FirstName);
            }
        }

        [Fact]
        public async Task SearchPatients_ReturnsPatientsByExactLastName()
        {
            // Arrange
            var options = GetDbContextOptions("SearchPatients_ReturnsPatientsByExactLastName");
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
                var repository = new PatientSearchRepository(context);

                // Act
                var result = await repository.SearchPatients("Messi");

                // Assert
                var patients = result.ToList();
                Assert.Single(patients);
                Assert.Equal("Messi", patients[0].LastName);
            }
        }

        [Fact]
        public async Task SearchPatientsPartial_ReturnsPatientsByPartialFirstName()
        {
            // Arrange
            var options = GetDbContextOptions("SearchPatientsPartial_ReturnsPatientsByPartialFirstName");
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
                var repository = new PatientSearchRepository(context);

                // Act
                var result = await repository.SearchPatientsPartial("Le");

                // Assert
                var patients = result.ToList();
                Assert.Single(patients);
                Assert.Equal("Leo", patients[0].FirstName);
            }
        }

        [Fact]
        public async Task SearchPatientsPartial_ReturnsPatientsByPartialLastName()
        {
            // Arrange
            var options = GetDbContextOptions("SearchPatientsPartial_ReturnsPatientsByPartialLastName");
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
                var repository = new PatientSearchRepository(context);

                // Act
                var result = await repository.SearchPatientsPartial("Mes");

                // Assert
                var patients = result.ToList();
                Assert.Single(patients);
                Assert.Equal("Messi", patients[0].LastName);
            }
        }
    }
}