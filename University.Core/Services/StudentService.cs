using System.Data;
using University.Core.DTOs;
using University.Core.Forms;
using University.Data.Entities;
using University.Data.Repositories;

namespace University.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Create(CreateStudentForm form)
        {
            if (string.IsNullOrEmpty(form.Name)) throw new Exception("Name is required");
            if (string.IsNullOrEmpty(form.Email)) throw new Exception("Email is required");
            var student = new Student
            {
                Name = form.Name,
                Email = form.Email
            };
            _studentRepository.Add(student);
            _studentRepository.SaveChanges();
        }


        public void Delete(int id)
        {
            var student = _studentRepository.GetById(id);
            if(student == null) throw new ArgumentNullException(nameof(student));
            _studentRepository.Delete(student);
            _studentRepository.SaveChanges();
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
            return DTOs;
        }

        public StudentDTO GetById(int id)
        {
            var student = GetById(id);
            if (student == null) throw new ArgumentNullException("not found");
            var DTO = new StudentDTO()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            };
            return DTO;
        }

        public void Update(int id, UpdateStudentForm form)
        {
            if (string.IsNullOrEmpty(form.Name)) throw new Exception("Name is required");
            var student = _studentRepository.GetById(id);
            if (student == null) throw new ArgumentNullException("Not Found");
            
            student.Name = form.Name;

            _studentRepository.Update(student);
            _studentRepository.SaveChanges();
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
