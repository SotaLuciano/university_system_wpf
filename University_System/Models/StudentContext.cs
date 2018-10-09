using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_System.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext() : base()
        {
            Database.SetInitializer<StudentContext>(new DropCreateDatabaseIfModelChanges<StudentContext>());
        }

        public DbSet<Student> Students { get; set; }  
    }
}
