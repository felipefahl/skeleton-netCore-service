namespace Skeleton.ServiceName.Utils.Interfaces
{
    public interface IEnumDropDownListable<TKey, TValue>
    {
        TKey Key { get; set; }
        TValue Value { get; set; }
    }
}
