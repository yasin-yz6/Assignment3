using Microsoft.Extensions.Logging;
using University.Core.DTOs;
using University.Core.Exceptions;
using University.Core.Forms;
using University.Core.Validations;
using University.Data.Entities;
using University.Data.Repositories;

namespace University.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseService> _logger;
        public CourseService (ICourseRepository courseRepository, ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public void Create(CreateCourseForm form)
        {
            var Validation = FormValidator.Validate(form);
            if (!Validation.IsValid)
            {
                _logger.LogWarning("Validation faild");
                throw new BusinessException(Validation.Errors);
            }
            var course = new Course
            {
                CourseName = form.CourseName,
                TeacherName = form.TeacherName
            };
            _courseRepository.AddCourse(course);
            _courseRepository.SaveChanges();
            _logger.LogInformation("Course created");
        }

        public void Delete(int id)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null) throw new NotFoundException("Unable to find Course");
            _courseRepository.DeleteCourse(course);
            _courseRepository.SaveChanges();
            _logger.LogInformation("Course deleted");
        }

        public List<CourseDTO> GetAll()
        {
            var student = _courseRepository.GetAllCourses();
            var DTOs = student.Select(A => new CourseDTO
            {
                CourseName = A.CourseName,
                TeacherName = A.TeacherName
            }).ToList();
            _logger.LogInformation("All courses listed");
            return DTOs;
        }

        public CourseDTO GetById(int id)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null) throw new NotFoundException("Unable to find course");
            var DTO = new CourseDTO()
            {
                CourseName = course.CourseName,
                TeacherName = course.TeacherName
            };
            _logger.LogInformation($"Course with id; {id} listed");
            return DTO;
        }

        public void Update(int id, UpdateCourseForm form)
        {
            var Validation = FormValidator.Validate(form);
            if (!Validation.IsValid)
            {
                _logger.LogWarning("Validation faild");
                throw new BusinessException(Validation.Errors);
            }
            var course = _courseRepository.GetCourseById(id);
            if (course == null) throw new NotFoundException("Unable to find course");

            course.CourseName = form.CourseName;
            _courseRepository.UpdateCourse(course);
            _courseRepository.SaveChanges();
            _logger.LogInformation("Course updated");

        }
    }
    public interface ICourseService
    {
        List<CourseDTO> GetAll();
        CourseDTO GetById(int id);
        void Create(CreateCourseForm form);
        void Update(int id, UpdateCourseForm form);
        void Delete(int id);
    }
}
