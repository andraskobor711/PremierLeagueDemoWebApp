import { TableRow } from './table-row';

export interface Table {
  seasonId: number;
  seasonLabel: string;
  rows: TableRow[];
}
