using Microsoft.Extensions.Logging;
using System.Data;
using University.Core.DTOs;
using University.Core.Exceptions;
using University.Core.Forms;
using University.Core.Validations;
using University.Data.Entities;
using University.Data.Repositories;

namespace University.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<StudentService> _logger;
        public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        public void Create(CreateStudentForm form)
        {
            var Validation = FormValidator.Validate(form);
            if (!Validation.IsValid)
            {
                _logger.LogWarning("Validation faild");
                throw new BusinessException(Validation.Errors);
            }
            var student = new Student
            {
                Name = form.Name,
                Email = form.Email
            };
            _studentRepository.Add(student);
            _studentRepository.SaveChanges();

            _logger.LogInformation("Student created");
        }


        public void Delete(int id)
        {
            var student = _studentRepository.GetById(id);
            if(student == null) throw new NotFoundException("Unable to find student");
            _studentRepository.Delete(student);
            _studentRepository.SaveChanges();
            _logger.LogInformation("Student Deleted");
        }

        public List<StudentDTO> GetAll()
        {
            var student = _studentRepository.GetAll();
            var DTOs = student.Select(A => new StudentDTO
            {
                Id = A.Id,
                Name = A.Name,
                Email = A.Email
            }).ToList();
            _logger.LogInformation("All students listed");
            return DTOs;
        }

        public StudentDTO GetById(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null) throw new NotFoundException("Unable to find student");
            var DTO = new StudentDTO()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            };
            _logger.LogInformation($"Student with id: {id} listed");
            return DTO;
        }

        public void Update(int id, UpdateStudentForm form)
        {
            var Validation = FormValidator.Validate(form);
            if (!Validation.IsValid)
            {
                _logger.LogWarning("Validation faild");
                throw new BusinessException(Validation.Errors);
            }
            var student = _studentRepository.GetById(id);
            if (student == null) throw new NotFoundException("Unable to find student");
            
            student.Name = form.Name;

            _studentRepository.Update(student);
            _studentRepository.SaveChanges();
            _logger.LogInformation("Student updated");
        }

    }

    public interface IStudentService
    {
        List<StudentDTO> GetAll();
        StudentDTO GetById(int id);
        void Create(CreateStudentForm form);
        void Update(int id, UpdateStudentForm form);
        void Delete(int id);
    }
}
