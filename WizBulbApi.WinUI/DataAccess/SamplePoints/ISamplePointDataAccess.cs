namespace WizBulbApi.WinUI;

public interface ISamplePointDataAccess
{
    Task<IEnumerable<SamplePoint>> GetSamplePointsAsync();
    Task AddSamplePointsAsync(IEnumerable<SamplePoint> samplePoints);
    Task RemoveSamplePointsAsync(IEnumerable<SamplePoint> samplePoints);
}
