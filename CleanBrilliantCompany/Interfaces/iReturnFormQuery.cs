using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
	public interface iReturnFormQuery
	{
        public List<ReturnForm> displayReturnForms();
        public List<Item> displayAllToReturnItems();
        public ReturnForm? getReturnFormById(int itemId);

        public bool deleteReturnForm(int productId, int itemId);
        public ReturnForm generateReturnForm(int productId, int itemId);
        public Task<ReturnForm?> sendReturnForm(ReturnForm model);

    }
}
