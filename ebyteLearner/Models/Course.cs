using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ebyteLearner.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }

        public string CourseName { get; set; }

        public string? CourseDescription { get; set; }

        public float? CoursePrice { get; set; } = 0;

        public bool? CourseIsPublished { get; set; } = false;

        public string? CourseImageURL { get; set; }

        [ForeignKey("CourseTeacherID")]
        public User? CourseTeacher { get; set; }

        public string? CourseDirectory { get; set; }

        public Guid? CourseTeacherID { get; set; }

        [ForeignKey("CategoryID")]
        public Category? CourseCategory { get; set; }

        public Guid? CategoryID { get; set; }

        public List<Module> CourseModules { get; set; }

        public List<User> Users { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedDate { get; init; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset UpdatedDate { get; init; }
    }
}
