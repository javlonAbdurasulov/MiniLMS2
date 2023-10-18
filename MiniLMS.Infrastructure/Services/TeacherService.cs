using Microsoft.EntityFrameworkCore;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Infrastructure.DataAccess;

namespace MiniLMS.Infrastructure.Services;
public class TeacherService : ITeacherService
{
    private readonly MiniLMSDbContext _context;

    public TeacherService(MiniLMSDbContext context)
    {
        _context = context;
    }

    public async Task<Teacher> CreateAsync(Teacher entity)
    {
         _context.Attach(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int Id)
    {
        Teacher? entity = await _context.Teachers.FindAsync(Id);
        if (entity == null)
            return false;

        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public IEnumerable<Teacher> GetAll()
    {
        IEnumerable<Teacher> teachers = _context.Teachers.Include(x=>x.Students)
            .AsNoTracking().AsEnumerable().OrderBy(x=>x.Id);
        return teachers;
    }

    public Task<IEnumerable<Teacher>> GetAllAsync()
    {
        IEnumerable<Teacher> teachers = _context.Teachers.Include(x=>x.Students)
            .AsNoTracking().AsEnumerable().OrderBy(x=>x.Id);
        return Task.FromResult(teachers);
    }

    public async Task<Teacher?> GetByIdAsync(int id)
    {
        Teacher? teacherEntity = _context.Teachers.Include(x=>x.Students).FirstOrDefault(x=>x.Id==id);
        
        return teacherEntity;
    }

    public async Task<bool> UpdateAsync(Teacher entity)
    {
        _context.Teachers.Update(entity);
        var executedRows = await _context.SaveChangesAsync();

        return executedRows > 0;
    }
}
