namespace ContactManager.Api.Entities
{
    public class EmailAddress
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int ContactId { get; set; }
        public Contact Contact { get; set; } = null!;
    }
}
