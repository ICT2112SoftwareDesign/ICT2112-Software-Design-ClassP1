using System.ComponentModel.DataAnnotations;

public class AgingDashboardInputDTO
{
    [Required]
    public int ValidityDuration { get; set; } 

    [Required] 
    public DateTime RequestedStartDate { get; set; } 

    [Required] 
    public DateTime RequestedEndDate { get; set; } 
}