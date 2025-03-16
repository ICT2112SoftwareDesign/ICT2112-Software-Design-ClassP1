using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
	public interface iReturnFormQuery
	{
        public Task<List<ReturnForm>> displayReturnForms();

        public Task<ReturnForm?> getReturnFormById(int itemId);

        public Task<bool> deleteReturnForm(int returnId);

		public Task<ReturnForm?> insertReturnForm(ReturnForm model);


	}
}
