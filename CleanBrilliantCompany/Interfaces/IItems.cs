using CleanBrilliantCompany.DatabaseEntities;


namespace CleanBrilliantCompany.Interface
{
    public interface IItem
    {
        List<ItemTable> getItems();
    }

}