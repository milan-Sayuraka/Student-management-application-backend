using StudentManagementSystem.Domain.Common.Dtos;
using StudentManagementSystem.Domain.Common.Entities;
using StudentManagementSystem.Domain.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Domain.Interfaces
{
    public interface IStudentService
    {
        public Task<List<StudentDto>> GetAllStudents();
        public Task<Student> GetStudent(int id);
        public Task<Student> CreateStudent(Student student);
        public Task<Response> UpdateStudent(Student student);
        public Task<Response> DeleteStudent(int id);
    }
}
