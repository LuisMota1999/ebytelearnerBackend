namespace ebyteLearner.Models
{
    public class Category
    {
        public Guid Id { get; init; }
        public string CategoryName { get; set; }
        public Course[] Courses { get; set; }

    }
}
