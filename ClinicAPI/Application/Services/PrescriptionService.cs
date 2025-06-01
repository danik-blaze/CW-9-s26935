using Domain;
using Infrastructure;
using Application.DTOs;

namespace Application.Services;
public class PrescriptionService
{
    private readonly AppDbContext _context;

    public PrescriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddPrescription(PrescriptionRequestDto request)
    {
        if (request.Medicaments.Count > 10)
            throw new Exception("Recepta może zawierać maksymalnie 10 leków.");

        if (request.DueDate < request.Date)
            throw new Exception("DueDate musi być późniejsza niż Date.");

        var patient = await _context.Patients.FindAsync(request.Patient.IdPatient);
        if (patient == null)
        {
            patient = new Patient
            {
                IdPatient = request.Patient.IdPatient,
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                BirthDate = request.Patient.BirthDate
            };
            await _context.Patients.AddAsync(patient);
        }

        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = request.IdDoctor,
            PrescriptionMedicaments = request.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Description = m.Description
            }).ToList()
        };

        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
    }
}