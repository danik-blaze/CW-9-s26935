using Application.Services;
using Application.DTOs;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests;
public class PrescriptionServiceTests
{
    [Fact]
    public async Task AddPrescription_ValidRequest_Success()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var service = new PrescriptionService(context);
            var request = new PrescriptionRequestDto
            {
                Patient = new PatientDto { IdPatient = 1, FirstName = "Jan", LastName = "Kowalski", BirthDate = DateTime.Now },
                Medicaments = new List<MedicamentDto> { new() { IdMedicament = 1, Dose = 1, Description = "Test" } },
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                IdDoctor = 1
            };

            await service.AddPrescription(request);
            Assert.True(context.Prescriptions.Any());
        }
    }
}