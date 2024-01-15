namespace Lab3_MinimalAPI.Models
{
    public class InterestLink
    {
        public int Id { get; set; }
        public string? URL { get; set; }

        public virtual ICollection<Interest> Interests {  get; set; }
        public virtual ICollection<Person> People { get; set; }
    }
}
