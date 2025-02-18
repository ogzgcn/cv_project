using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logimat.API.Models;

public class Material
{
    [Key]
    public Guid Id { get; set; }=Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }  

    [MaxLength(50)]
    public string Code { get; set; } 

    [Required]
    public int Quantity { get; set; } 

    [MaxLength(200)]
    public string Description { get; set; }  

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  
}