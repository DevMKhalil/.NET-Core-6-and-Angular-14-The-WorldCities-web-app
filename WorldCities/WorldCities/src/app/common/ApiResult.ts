export interface ApiResult<T> {
  data: T[];
  pageIndex: number;
  pageSize: string;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
