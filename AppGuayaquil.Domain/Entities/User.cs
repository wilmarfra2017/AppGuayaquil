namespace AppGuayaquil.Domain.Entities
{
    public class User : DomainEntity
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public DateTime CreationDate { get; set; }

        public User()
        {
            UserId = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
        }
    }
}
