namespace WizBulbApi;

public interface IConverter<TSource, TDestination>
{
    TDestination Convert(TSource source);
}
