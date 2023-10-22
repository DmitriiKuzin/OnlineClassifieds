using DAL;
using Riok.Mapperly.Abstractions;

namespace ClassifiedsService;

[Mapper]
public static partial class Mapper
{
    public static partial Listing ToDbModel(this ListingDto dto);
    public static partial IQueryable<ListingDto> ToDtoList(this IQueryable<Listing> modelQueryable);
    
}