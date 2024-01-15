namespace Lab3_MinimalAPI.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? PhoneNum{ get; set; }

        public virtual ICollection<Interest> Interests { get; set; }
        public virtual ICollection<InterestLink> InterestLinks { get; set; } 

    }
}   
