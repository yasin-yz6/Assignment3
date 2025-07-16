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
            if (student == null) throw new ArgumentNullException(nameof(student));
            _context.Add(student);
            _context.SaveChanges();

        }

        public void Delete(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            _context.Remove(student);
            _context.SaveChanges();
        }

        public List<Student> GetAll()
        {
            return _context.Students.ToList();
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
            if (student == null) throw new ArgumentNullException(nameof(student));
            _context.Update(student);
            _context.SaveChanges();
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
