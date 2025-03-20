using System.ComponentModel.DataAnnotations;

namespace CleanBrilliantCompany.Entities
{
    public class AlertTypeTable
    {
        public required string TypeCode { get; set; }
        public string Description { get; set; }
    }
}
