using CleanBrilliantCompany.DatabaseEntities;


namespace CleanBrilliantCompany.Interface
{
    public interface CostIManufacturer
    {
        List<ManufacturerTable> GetAllManufacturers();

        //  List<ManufacturerTable> GetManufacturerDetails(int manufacturerId);
    }


}