using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Input: BaseEntity
{
    public DateTime InputDate { get; set; } 
}