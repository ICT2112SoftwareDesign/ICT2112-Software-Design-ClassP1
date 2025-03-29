using CleanBrilliantCompany.DatabaseEntities;


namespace CleanBrilliantCompany.Interface
{
    public interface IManufacturer
    {
        List<ManufacturerTable> GetAllManufacturers();

        //  List<ManufacturerTable> GetManufacturerDetails(int manufacturerId);
    }


}