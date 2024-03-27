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
    public class PatientSortRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task SortByLastName_ReturnsPatientsSortedByLastName()
        {
            // Arrange
            var options = GetDbContextOptions("SortByLastName_ReturnsPatientsSortedByLastName");
            using (var context = new AppDbContext(options))
            {
                Address address = new Address
                {
                    City = "Barcelona",
                    Street = "Camp Nou Street",
                    ZipCode = "12-345"
                };
                context.Patients.Add(new Patient { Id = 1, FirstName = "Leo", LastName = "Messi", Pesel = "12345678901", Address = address });
                context.Patients.Add(new Patient { Id = 2, FirstName = "Robert", LastName = "Lewandowski", Pesel = "98765432111", Address = address });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientSortRepository(context);

                // Act
                var result = await repository.SortByLastName();

                // Assert
                var sortedPatients = result.ToList();
                Assert.Equal(2, sortedPatients.Count);
                Assert.Equal("Lewandowski", sortedPatients[0].LastName);
                Assert.Equal("Messi", sortedPatients[1].LastName);
            }
        }

        [Fact]
        public async Task SortByPesel_ReturnsPatientsSortedByPesel()
        {
            // Arrange
            var options = GetDbContextOptions("SortByPesel_ReturnsPatientsSortedByPesel");
            using (var context = new AppDbContext(options))
            {
                Address address = new Address
                {
                    City = "Barcelona",
                    Street = "Camp Nou Street",
                    ZipCode = "12-345"
                };
                context.Patients.Add(new Patient { Id = 1, FirstName = "Leo", LastName = "Messi", Pesel = "12345678901", Address = address });
                context.Patients.Add(new Patient { Id = 2, FirstName = "Robert", LastName = "Lewandowski", Pesel = "98765432111", Address = address });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientSortRepository(context);

                // Act
                var result = await repository.SortByPesel();

                // Assert
                var sortedPatients = result.ToList();
                Assert.Equal(2, sortedPatients.Count);
                Assert.Equal("12345678901", sortedPatients[0].Pesel);
                Assert.Equal("98765432111", sortedPatients[1].Pesel);
            }
        }

        [Fact]
        public async Task SortByCity_ReturnsPatientsSortedByCity()
        {
            // Arrange
            var options = GetDbContextOptions("SortByCity_ReturnsPatientsSortedByCity");
            using (var context = new AppDbContext(options))
            {
                Address address = new Address
                {
                    City = "Miami",
                    Street = "Miami Beach",
                    ZipCode = "12-345"
                };
                context.Patients.Add(new Patient { Id = 1, FirstName = "Leo", LastName = "Messi", Pesel = "12345678901", Address = address });
                Address address2 = new Address
                {
                    City = "Barcelona",
                    Street = "Camp Nou Street",
                    ZipCode = "12-345"
                };
                context.Patients.Add(new Patient { Id = 2, FirstName = "Robert", LastName = "Lewandowski", Pesel = "98765432111", Address = address2 });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientSortRepository(context);

                // Act
                var result = await repository.SortByCity();

                // Assert
                var sortedPatients = result.ToList();
                Assert.Equal(2, sortedPatients.Count);
                Assert.Equal("Barcelona", sortedPatients[0].Address.City);
                Assert.Equal("Miami", sortedPatients[1].Address.City);
            }
        }

        [Fact]
        public async Task SortByZipCode_ReturnsPatientsSortedByZipCode()
        {
            // Arrange
            var options = GetDbContextOptions("SortByZipCode_ReturnsPatientsSortedByZipCode");
            using (var context = new AppDbContext(options))
            {
                Address address = new Address
                {
                    City = "Barcelona",
                    Street = "Camp Nou Street",
                    ZipCode = "12-345"
                };
                context.Patients.Add(new Patient { Id = 1, FirstName = "Leo", LastName = "Messi", Pesel = "12345678901", Address = address });
                Address address2 = new Address
                {
                    City = "Barcelona",
                    Street = "Camp Nou Street",
                    ZipCode = "12-346"
                };
                context.Patients.Add(new Patient { Id = 2, FirstName = "Robert", LastName = "Lewandowski", Pesel = "98765432111", Address = address2 });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repository = new PatientSortRepository(context);

                // Act
                var result = await repository.SortByZipCode();

                // Assert
                var sortedPatients = result.ToList();
                Assert.Equal(2, sortedPatients.Count);
                Assert.Equal("12-345", sortedPatients[0].Address.ZipCode);
                Assert.Equal("12-346", sortedPatients[1].Address.ZipCode);
            }
        }
    }
}