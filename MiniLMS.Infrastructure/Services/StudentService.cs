using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Infrastructure.DataAccess;

namespace MiniLMS.Infrastructure.Services;
public class StudentService : IStudentService
{
    private readonly MiniLMSDbContext _context;
    private readonly Serilog.ILogger _serilog;

    public StudentService(MiniLMSDbContext context,Serilog.ILogger logger)
    {
        _context = context;
        _serilog = logger;
    }

    public async Task<Student> CreateAsync(Student entity)
    {
        _context.Attach(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int Id)
    {
        Student? entity = await _context.Students.FindAsync(Id);
        if (entity == null)
        {
            _serilog.Warning("Not found delete implementation!");
            return false;
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<IEnumerable<Student>> GetAllAsync()
    {
        _serilog.Debug("Get all async execute...");
        IEnumerable<Student> students = _context.Students.Include(x => x.Teachers)
            .AsNoTracking().OrderBy(x=>x.Id).AsEnumerable();
        
        return Task.FromResult(students);
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        Student? studentEntity = await _context.Students.Include(x=>x.Teachers)
            .FirstOrDefaultAsync(x=>x.Id==id);
        if (studentEntity == null)
        {
            _serilog.Warning($"Student implement send null object!");
        }
        return studentEntity;
    }

    public async Task<bool> UpdateAsync(Student entity)
    {
        Console.WriteLine(entity.Login);
        
        _context.Students.Update(entity);
        int executedRows = await _context.SaveChangesAsync();

        return executedRows > 0;
    }
}
