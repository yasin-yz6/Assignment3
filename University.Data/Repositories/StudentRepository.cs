using University.Data.Contexts;
using University.Data.Entities;

namespace University.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UniversityDbContext _context;

        public StudentRepository(UniversityDbContext context)
        {
            _context = context;
        }

        public void Add(Student student)
        {
            _context.Add(student);
        }

        public void Delete(Student student)
        {
            _context.Remove(student);
        }

        public List<Student> GetAll()
        {
            var courses =  _context.Students.ToList();
            return courses;
        }

        public Student GetById(int id)
        {
            return _context.Students.Find(id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Student student)
        {
            _context.Update(student);
        }
    }

    public interface IStudentRepository
    {
        List<Student> GetAll();
        Student GetById (int id);
        void Add (Student student);
        void Update (Student student);
        void Delete (Student student);
        void SaveChanges();
    }
}
