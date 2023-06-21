namespace WizBulbApi;

public interface IDualConverter<TSource, TDestination>
{
    TDestination Convert(TSource source);
    TSource ConvertBack(TDestination source);
}