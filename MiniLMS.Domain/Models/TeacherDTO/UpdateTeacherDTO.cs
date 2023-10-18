namespace MiniLMS.Domain.Models.TeacherDTO;

public class UpdateTeacherDTO : TeacherBaseDTO
{

    public int Id { get; set; }

    public string Password { get; set; }
}
