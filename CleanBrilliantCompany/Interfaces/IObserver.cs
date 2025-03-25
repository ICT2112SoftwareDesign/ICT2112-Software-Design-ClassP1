using System.Runtime.InteropServices;
using CleanBrilliantCompany.Models.Entity;

public interface IObserver
{
    void Update(Dictionary<string, object> itemInfo);
}