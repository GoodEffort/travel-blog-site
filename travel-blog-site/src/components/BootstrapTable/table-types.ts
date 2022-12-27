export type TableColumn = {
  key: string;
  name?: string;
  width?: number;
  sortable?: boolean;
  filterable?: boolean;
};

export type TableUpdate = {
  sortField?: string,
  desc: boolean,
  page: number,
  filters: { key: string, value: string }[],
}
