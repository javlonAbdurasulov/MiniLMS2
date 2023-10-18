using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Infrastructure.DataAccess;

namespace MiniLMS.Infrastructure.Services;
public class StudentService : IStudentService
{
    private readonly MiniLMSDbContext _context;

    public StudentService(MiniLMSDbContext context)
    {
        _context = context;
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
            return false;

        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<IEnumerable<Student>> GetAllAsync()
    {
        IEnumerable<Student> students = _context.Students.Include(x => x.Teachers)
            .AsNoTracking().AsEnumerable().OrderBy(x=>x.Id);
        
        return Task.FromResult(students);
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        Student? studentEntity = await _context.Students.Include(x=>x.Teachers)
            .FirstOrDefaultAsync(x=>x.Id==id);
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
