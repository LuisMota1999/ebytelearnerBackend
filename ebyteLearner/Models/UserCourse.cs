using System.ComponentModel.DataAnnotations.Schema;

namespace ebyteLearner.Models
{
    public class UserCourse
    {

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        public DateTimeOffset CreatedDate { get; init; }
        public DateTimeOffset UpdatedDate { get; init; }
    }
}
