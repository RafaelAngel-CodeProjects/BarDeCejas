using BarCejas.Core.QueryFilters;

using System;

namespace BarCejas.Infrastructure.Interfaces
{
    public interface IUriService
    {
        Uri GetUserPaginationUri(QueryFilter filter, string actionUrl);
    }
}