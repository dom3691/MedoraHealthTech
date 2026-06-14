export type QueryParameters = {
  pageNumber?: number;
  pageSize?: number;
  search?: string | null;
  sortBy?: string | null;
  sortDirection?: 'asc' | 'desc';
};

export type PagedResult<T> = {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};
