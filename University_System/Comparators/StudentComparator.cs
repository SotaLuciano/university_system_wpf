using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University_System.Models;

namespace University_System.Comparators
{
    public static class StudentComparator
    {
        public static bool CompareStudents(Student firstStudent, Student secondStudent)
        {
            return firstStudent.FirstName == secondStudent.FirstName 
                   && firstStudent.LastName == secondStudent.LastName 
                   && firstStudent.Age == secondStudent.Age 
                   && firstStudent.Gender == secondStudent.Gender 
                   && firstStudent.Address == secondStudent.Address
                   && firstStudent.Email == secondStudent.Email 
                   && firstStudent.PhoneNumber == secondStudent.PhoneNumber 
                   && firstStudent.BornDateTime == secondStudent.BornDateTime;
        }
    }
}
