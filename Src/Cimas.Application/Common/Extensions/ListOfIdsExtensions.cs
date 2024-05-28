namespace Cimas.Application.Common.Extensions
{
    public static class ListOfIdsExtensions
    {
        public static Guid? GetSingleDistinctIdOrNull<T>(this List<T> items, Func<T, Guid> idSelector)
        {
            IEnumerable<Guid> distinctIds = items
                .Select(idSelector)
                .Distinct();

            return distinctIds.Count() == 1 ? distinctIds.First() : null;
        }
    }
}
