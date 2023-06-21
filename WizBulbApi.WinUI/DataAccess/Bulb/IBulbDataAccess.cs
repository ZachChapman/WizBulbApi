namespace WizBulbApi.WinUI;

public interface IBulbDataAccess
{
	Task<BulbHandle?> GetBulbHandleAsync(MacAddress macAddress);
	Task<List<BulbHandle>> GetAllBulbHandlesAsync(int? homeId = null);
	Task<bool> AddBulbHandleAsync(BulbHandle handle);
	Task UpdateBulbHandleAsync(BulbHandle handle);
	Task UpdateBulbNameAsync(MacAddress macAddress, string newName);
	Task RemoveBulbHandleAsync(BulbHandle handle);
	Task<bool> ContainsAsync(MacAddress macAddress);
}