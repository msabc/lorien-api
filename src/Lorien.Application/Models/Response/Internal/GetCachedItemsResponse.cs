namespace Lorien.Application.Models.Response.Internal
{
    public record GetCachedItemsResponse
    {
        public required IEnumerable<CachedItem> CachedItems { get; set; }
    }

    public record CachedItem
    {
        public required string ItemName { get; set; }

        public required int NumberOfRecords { get; set; }
    }
}
