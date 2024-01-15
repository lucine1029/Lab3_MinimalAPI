namespace Lab3_MinimalAPI.Models
{
    public class Interest
    {
        public int Id { get; set; }
        public string? InterestName { get; set;}
        public string? InterestDescription { get; set;}

        //many to many: one interest can have many people, one people can have many interests
        //One to many: one interest can have many InterestLinks, one InterestLink can only belong to one interest
        public virtual ICollection<Person> People { get; set; }
        public virtual ICollection<InterestLink> InterestLinks { get; set; }

    }
}
