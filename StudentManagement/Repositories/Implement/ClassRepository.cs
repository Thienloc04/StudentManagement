using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Repositories.Interface;

namespace StudentManagement.Repositories.Implement
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;
        public ClassRepository (AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return await _context.Classes.Where(c => c.Status == 1).ToListAsync();
        }
    }
}
