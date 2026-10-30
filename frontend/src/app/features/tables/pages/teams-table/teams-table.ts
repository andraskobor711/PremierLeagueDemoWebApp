import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BehaviorSubject, catchError, combineLatest, map, Observable, of, startWith } from 'rxjs';
import { TableRow } from '../../models/table-row';
import { FALLBACK_TEAM_LOGO_PATH } from '../../../teams/services/teams-service';
import { TableService } from '../../services/table-service';

type SortColumn = 'wins' | 'draws' | 'losses' | 'goalsFor' | 'goalsAgainst' | 'goalDifference';
type SortDirection = 'ascending' | 'descending';

interface SortState {
  column: SortColumn;
  direction: SortDirection;
}

type TableState =
  | { status: 'loading' }
  | {
      status: 'loaded';
      seasonLabel: string;
      rows: TableRow[];
      sortColumn: SortColumn | null;
      sortDirection: SortDirection | null;
    }
  | { status: 'load-error' };

@Component({
  imports: [AsyncPipe, RouterLink],
  selector: 'app-teams-table',
  styleUrl: './teams-table.css',
  templateUrl: './teams-table.html',
})
export class TeamsTable {
  private readonly tableService = inject(TableService);
  private readonly sortState$ = new BehaviorSubject<SortState | null>(null);
  readonly fallbackTeamLogoPath = FALLBACK_TEAM_LOGO_PATH;
  private readonly tableState$ = this.tableService.getCurrentTable().pipe(
    map(table => ({
      status: 'loaded' as const,
      seasonLabel: table.seasonLabel,
      rows: table.rows,
    })),
    catchError((error: unknown) => {
      console.error('Failed to load the current table.', error);
      return of({ status: 'load-error' } satisfies TableState);
    }),
    startWith({ status: 'loading' } satisfies TableState)
  );
  readonly state$: Observable<TableState> = combineLatest([this.tableState$, this.sortState$]).pipe(
    map(([state, sort]) => {
      if (state.status !== 'loaded') {
        return state;
      }

      const rows = sort
        ? [...state.rows].sort((first, second) => {
            const result = first[sort.column] - second[sort.column];
            return sort.direction === 'ascending' ? result : -result;
          })
        : state.rows;

      return {
        ...state,
        rows,
        sortColumn: sort?.column ?? null,
        sortDirection: sort?.direction ?? null,
      };
    })
  );

  sortBy(column: SortColumn): void {
    const currentSort = this.sortState$.value;
    const direction =
      currentSort?.column === column
        ? currentSort.direction === 'ascending'
          ? 'descending'
          : 'ascending'
        : 'descending';

    this.sortState$.next({ column, direction });
  }

  resetSort(): void {
    this.sortState$.next(null);
  }

  onLogoError(event: Event): void {
    const image = event.currentTarget;

    if (image instanceof HTMLImageElement && !image.src.endsWith(this.fallbackTeamLogoPath)) {
      image.src = this.fallbackTeamLogoPath;
    }
  }
}
