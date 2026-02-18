namespace Shared.DTOs.ProductModuleDTOs;

public record PaginatedResult<TEntity>(int PageIndex, int PageSize, int TotalCount, IEnumerable<TEntity> Data);