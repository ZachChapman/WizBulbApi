using System.Text.Json;

namespace WizBulbApi.WinUI;

public class SamplePointDataAccess : ISamplePointDataAccess
{
    private static readonly List<SamplePoint> _samplePoints = new();
    private readonly string _path;
    private bool _isLoaded;

    public SamplePointDataAccess(string path)
    {
        _path = path;
    }

    public async Task<IEnumerable<SamplePoint>> GetSamplePointsAsync()
    {
        await TryInitialiseHandlesAsync();

        return _samplePoints.ToList();
    }

    public async Task AddSamplePointsAsync(IEnumerable<SamplePoint> samplePoints)
    {
        _samplePoints.AddRange(samplePoints);

        await SaveAsync();
    }

    public async Task RemoveSamplePointsAsync(IEnumerable<SamplePoint> samplePoints)
    {
        foreach(var samplePoint in samplePoints)
        {
            var dbSamplePoint = _samplePoints.FirstOrDefault(point => point.Id == samplePoint.Id);
            if(dbSamplePoint is null)
            {
                continue;
            }

            _samplePoints.Remove(dbSamplePoint);
        }

        await SaveAsync();
    }

    private async Task TryInitialiseHandlesAsync()
    {
        if(!_isLoaded)
        {
            _isLoaded = true;

            // Initialise list from file
            if(File.Exists(_path))
            {
                await using var fileStream = File.OpenRead(_path);

                var samplePoints =
                    await JsonSerializer.DeserializeAsync<IEnumerable<SamplePoint>>(
                        fileStream,
                        GetJsonSerializerOptions());

                _samplePoints.AddRange(samplePoints);
            }
        }
    }

    private async Task SaveAsync()
    {
        await using var fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(fileStream, _samplePoints, GetJsonSerializerOptions());
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            AllowTrailingCommas = true,
        };
    }
}

public class SamplePoint
{
    public SamplePoint(int id, double x, double y)
    {
        Id = id;
        X = x;
        Y = y;
    }

    public int Id { get; set; }
    // Normalised X
    public double X { get; set; }
    // Normalised Y
    public double Y { get; set; }
}