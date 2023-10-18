namespace MiniLMS.Domain.Entities;
public class Student: BaseEntity
{
    public string Major { get; set; }
    public virtual ICollection<Teacher?> Teachers { get; set; }

}
