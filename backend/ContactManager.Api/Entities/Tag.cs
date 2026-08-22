namespace ContactManager.Api.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Contact> Contacts { get; set; }
            = new List<Contact>();
    }
}
