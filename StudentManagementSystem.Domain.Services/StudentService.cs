using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Domain.Common.Dtos;
using StudentManagementSystem.Domain.Common.Entities;
using StudentManagementSystem.Domain.Common.Response;
using StudentManagementSystem.Domain.EntityFramework;
using StudentManagementSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Domain.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public StudentService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<StudentDto>> GetAllStudents()
        {
            try
            {
                var studentsDto = new List<StudentDto>();
                var students = await _applicationDbContext.Student.Select(s => new {s.Id, s.FirstName, s.LastName, s.Mobile, s.Email, s.NIC }).ToListAsync();
                foreach (var student in students)
                {
                    var studentDto = new StudentDto();
                    studentDto.Id = student.Id;
                    studentDto.FirstName = student.FirstName;
                    studentDto.LastName = student.LastName;
                    studentDto.Mobile = student.Mobile;
                    studentDto.Email = student.Email;
                    studentDto.NIC = student.NIC;

                    studentsDto.Add(studentDto);
                }
                return studentsDto;
            }
            catch
            {

                return null;
            }
        }

        public async Task<Student> GetStudent(int id)
        {
            try
            {
                var student = await _applicationDbContext.Student.FindAsync(id);

                if (student == null)
                {
                    // Student not found
                    throw new NotFoundException($"Student with ID {id} not found.");
                }

                return student;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during database operation
                throw new Exception("An error occurred while retrieving the student.", ex);
            }
        }


        public async Task<Student> CreateStudent(Student student)
        {
            try
            {
                if (student == null)
                {
                    throw new ArgumentNullException(nameof(student), "Student object is null.");
                }
                var createdStudent = await _applicationDbContext.Student.AddAsync(student);
                await _applicationDbContext.SaveChangesAsync();
                return createdStudent.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the student.", ex);
            }
        }
        public async Task<Response> UpdateStudent(Student student)
        {
            try
            {
                if (student == null)
                {
                    throw new ArgumentNullException(nameof(student), "Student object is null.");
                }

                var existingStudent = await _applicationDbContext.Student.FindAsync(student.Id);
                if (existingStudent == null)
                {
                    return new Response { Success = false, Message = "Student not found." };
                }

                // Update the properties of the existing student
                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.Mobile = student.Mobile;
                existingStudent.Email = student.Email;
                existingStudent.NIC = student.NIC;
                existingStudent.DateOfBirth = student.DateOfBirth;
                existingStudent.Address = student.Address;

                await _applicationDbContext.SaveChangesAsync();

                return new Response { Success = true, Message = "Student updated successfully." };
            }
            catch (Exception ex)
            {
                // Handle the exception (logging, etc.) and rethrow if needed
                throw new Exception("An error occurred while updating the student.", ex);
            }
        }


        public async Task<Response> DeleteStudent(int id)
        {
            var student = await _applicationDbContext.Student.FindAsync(id);

            if (student == null)
            {
                return new Response { Success = false, Message = "Student not found." };
            }

            _applicationDbContext.Student.Remove(student);
            var dBSave = await _applicationDbContext.SaveChangesAsync();

            if (dBSave > 0)
            {
                return new Response { Success = true, Message = "Student deleted successfully." };
            }

            return new Response { Success = false, Message = "An error occurred while deleting the student." };
        }

    }
}
