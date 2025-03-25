public static class DashboardMapper
{
    public static DashboardDTO ToDTO(DashboardTable entity) => new DashboardDTO
    {
        DashboardId = entity.DashboardId,
        Name = entity.Name,
        RequestedStartDate = entity.RequestedStartDate,
        RequestedEndDate = entity.RequestedEndDate,
        GeneratedDate = entity.GeneratedDate,
        ValidityDuration = entity.ValidityDuration,
        TypeId = entity.TypeId
    };

    public static DashboardTable ToEntity(DashboardDTO dto) => new DashboardTable
    {
        DashboardId = dto.DashboardId,
        Name = dto.Name,
        RequestedStartDate = dto.RequestedStartDate,
        RequestedEndDate = dto.RequestedEndDate,
        GeneratedDate = dto.GeneratedDate,
        ValidityDuration = dto.ValidityDuration,
        TypeId = dto.TypeId
    };
}