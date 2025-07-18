using University.Data.Contexts;
using University.Data.Entities;

namespace University.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly UniversityDbContext _context;

        public CourseRepository(UniversityDbContext context)
        {
            _context = context;
        }

        public void AddCourse(Course course)
        {
            _context.Add(course);
        }

        public void DeleteCourse(Course course)
        {
            _context.Remove(course);
        }

        public List<Course> GetAllCourses()
        {
            var courses = _context.Courses.ToList();
            return courses;
        }

        public Course GetCourseById(int id)
        {
           return _context.Courses.Find(id);
            
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateCourse(Course course)
        {
            _context.Update(course);

        }
    }
    public interface ICourseRepository
    {
        List<Course> GetAllCourses();
        Course GetCourseById(int id);
        void AddCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
        void SaveChanges();
    }
}
