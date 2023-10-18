namespace MiniLMS.Domain.Entities;
public class Teacher : BaseEntity
{
    public virtual ICollection<Student?> Students { get; set; }
    public double? Salary { get; set; }
}
