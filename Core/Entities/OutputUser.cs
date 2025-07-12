using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class OutputUser: BaseEntity
{
    public int OutputId { get; set; }
    [ForeignKey(nameof(OutputId))]
    public Output? Output { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
    
}