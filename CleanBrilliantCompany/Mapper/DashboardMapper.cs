public static class DashboardMapper
{
    public static DashboardDTO ToDTO(DashboardTable table)
    {

        Console.WriteLine($"DEBUG - From DB: Name={table.Name}, GenDate={table.GeneratedDate}, Start={table.RequestedStartDate}");

        return new DashboardDTO
        {
            Name = table.Name,
            RequestedStartDate = table.RequestedStartDate,
            RequestedEndDate = table.RequestedEndDate,
            GeneratedDate = table.GeneratedDate,
            ValidityDuration = table.ValidityDuration,
            Type = table.TypeId
        };
    }

    public static DashboardTable ToEntity(DashboardDTO dto) => new DashboardTable
    {
        DashboardId = dto.DashboardId,
        Name = dto.Name,
        RequestedStartDate = dto.RequestedStartDate,
        RequestedEndDate = dto.RequestedEndDate,
        GeneratedDate = dto.GeneratedDate ?? DateTime.Now,
        ValidityDuration = dto.ValidityDuration,
        TypeId = dto.Type  
    };
}