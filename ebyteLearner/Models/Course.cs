using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public float CoursePrice { get; set; }
       
        [ForeignKey("CourseTeacherID")]
        public User User { get; set; }
        public Guid CourseTeacherID { get; set; }
        public List<Module> Modules { get; set; }
        public List<User> Users { get; set; }
        public DateTimeOffset CreatedDate { get; init; }
        public DateTimeOffset UpdatedDate { get; init; }
    }
}