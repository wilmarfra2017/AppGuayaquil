namespace AppGuayaquil.Domain.Entities;

public class People : DomainEntity
{
    public Guid PeopleId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string IdentificationNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string IdentificationType { get; set; } = default!;
    public DateTime CreationDate { get; set; }    
    public string FullIdentification => $"{IdentificationType}-{IdentificationNumber}";
    public string FullName => $"{FirstName} {LastName}";
}
