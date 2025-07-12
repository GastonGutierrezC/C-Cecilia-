using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Output: BaseEntity
{
    public DateTime OutputDate { get; set; }

}