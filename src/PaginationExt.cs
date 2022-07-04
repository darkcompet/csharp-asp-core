namespace Tool.Compet.Core;

// Extension of IQueryable<T>
// Where T: item type
public static class PaginationExt {
	/// @param pagePos From 1 (NOT from 0).
	/// @param pageSize How many items in each page (for eg,. 10, 20, 50,...).
	/// Note: given `pageIndex * pageSize` must be in range of Int32.
	public static PagedResult<T> PaginateDk<T>(this IQueryable<T> queryable, int pagePos = 1, int pageSize = 50) where T : class {
		// Maybe zero-
		if (pagePos < 1) {
			pagePos = 1;
		}

		// Maybe overflow
		var offset = (pagePos - 1) * pageSize;
		if (offset < 0) {
			offset = 0;
		}

		var items = queryable.Skip(offset).Take(pageSize).ToArray();

		// This calculation is faster than `Math.Ceiling(items.Length / pageSize)`
		var pageCount = (items.Length + pageSize - 1) / pageSize;

		return new PagedResult<T>(
			items: items,
			pagePos: pagePos,
			pageCount: pageCount
		);
	}
}

public class PagedResult<T> where T : class {
	/// Item list in the page
	public readonly T[]? items;

	/// Position (1-index-based) of current page
	public readonly int pagePos;

	/// Total number of page
	public readonly int pageCount;

	public PagedResult(T[]? items, int pagePos, int pageCount) {
		this.items = items;
		this.pagePos = pagePos;
		this.pageCount = pageCount;
	}
}
