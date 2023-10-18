namespace MiniLMS.Domain.Models.TeacherDTO;

public class TeacherCreateDTO:TeacherBaseDTO
{

    public string Login { get; set; }
    public string Password { get; set; }
}
