using System.ComponentModel.DataAnnotations;
public class DashboardDTO
{
    public int DashboardId { get; set; }
    
    public string Name { get; set; } = string.Empty; 

    [Required]
    public DateTime RequestedStartDate { get; set; }
    [Required]
    public DateTime RequestedEndDate { get; set; }
    public DateTime? GeneratedDate { get; set; }

    [Required]
    public int ValidityDuration { get; set; }

    
    public int Type { get; set; } // Foreign Key
}
