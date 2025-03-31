using System;
using System.ComponentModel.DataAnnotations;

public class ManufacturerTable
{
    [Key]
    public int ManufacturerId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ManufacturerAddress { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
