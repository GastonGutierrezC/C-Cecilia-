using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Input: BaseEntity
{
    public DateTime InputDate { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}