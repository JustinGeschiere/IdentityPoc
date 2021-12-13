namespace IdentityPoc.Features.Models
{
	public struct PagingModel
	{
		public PagingModel(int currentPage, int pageSize, int totalCount)
		{
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalCount = totalCount;

			TotalPages = totalCount / pageSize;
			if (totalCount % pageSize > 0)
			{
				TotalPages++;
			}

			HasNextPage = CurrentPage < TotalPages;
			HasPreviousPage = CurrentPage > 1;
		}

		public int CurrentPage { get; }

		public int PageSize { get; }

		public int TotalCount { get; }

		public int TotalPages { get; }

		public bool HasNextPage { get; }
		public bool HasPreviousPage { get; }
	}
}
