using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class InputUser: BaseEntity
{
    public required int InputId { get; set; }
    [ForeignKey(nameof(InputId))]
    public Input? Input { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}