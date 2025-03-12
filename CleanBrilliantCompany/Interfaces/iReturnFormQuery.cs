using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
	public interface iReturnFormQuery
	{
        public List<ReturnForm> displayReturnForms();

        public ReturnForm? getReturnFormById(int itemId);

        public bool deleteReturnForm(int returnId);

		public ReturnForm? insertReturnForm(ReturnForm model);


	}
}
