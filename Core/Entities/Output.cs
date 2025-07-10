using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Output: BaseEntity
{
    public DateTime OutputDate { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}