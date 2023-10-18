namespace MiniLMS.Domain.Models.StudentDTO;
public class UpdateStudentDTO : StudentBaseDTO
{
    public int Id { get; set; }

    public string Password { get; set; }
}
