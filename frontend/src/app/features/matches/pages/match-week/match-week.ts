import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faChevronLeft, faChevronRight } from '@fortawesome/free-solid-svg-icons';
import { catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
import { Match, MatchService } from '../../services/match-service';
import { FALLBACK_TEAM_LOGO_PATH } from '../../../teams/services/teams-service';

interface MatchDayGroup {
  date: Date;
  matches: Match[];
}

type MatchWeekState =
  | { status: 'loading' }
  | {
      status: 'loaded';
      seasonId: number | null;
      seasonLabel: string | null;
      gameWeekNumber: number | null;
      days: MatchDayGroup[];
    }
  | { status: 'invalid-query' }
  | { status: 'load-error' };

@Component({
  imports: [AsyncPipe, DatePipe, FontAwesomeModule, RouterLink],
  selector: 'app-match-week',
  styleUrl: './match-week.css',
  templateUrl: './match-week.html',
})
export class MatchWeek {
  readonly previousGameWeekIcon = faChevronLeft;
  readonly nextGameWeekIcon = faChevronRight;
  readonly maxGameWeek = 38;
  private readonly route = inject(ActivatedRoute);
  private readonly matchService = inject(MatchService);
  private readonly fallbackTeamLogoPath = FALLBACK_TEAM_LOGO_PATH;
  readonly state$ = this.route.queryParamMap.pipe(
    switchMap(params => {
      if (this.route.snapshot.routeConfig?.path === 'matches/current-gameweek') {
        return this.loadMatches(this.matchService.getCurrentGameWeekMatches());
      }

      const seasonIdParam = params.get('seasonId');
      const gameWeekParam = params.get('gameWeek');

      if (seasonIdParam === null || gameWeekParam === null) {
        return of({ status: 'invalid-query' } satisfies MatchWeekState);
      }

      const seasonId = Number(seasonIdParam);
      const gameWeek = Number(gameWeekParam);

      if (
        !/^\d+$/.test(seasonIdParam) ||
        !/^\d+$/.test(gameWeekParam) ||
        !Number.isSafeInteger(seasonId) ||
        seasonId <= 0 ||
        !Number.isSafeInteger(gameWeek) ||
        gameWeek <= 0
      ) {
        return of({ status: 'invalid-query' } satisfies MatchWeekState);
      }

      return this.loadMatches(
        this.matchService.getMatchesByGameWeek(seasonId, gameWeek),
        { seasonId, gameWeek }
      );
    })
  );

  private loadMatches(
    matches$: Observable<Match[]>,
    context?: { seasonId: number; gameWeek: number }
  ): Observable<MatchWeekState> {
    return matches$.pipe(
      map(matches => this.toLoadedState(matches, context)),
      catchError((error: unknown) => {
        console.error('Failed to load matches.', error);
        return of({ status: 'load-error' } satisfies MatchWeekState);
      }),
      startWith({ status: 'loading' } satisfies MatchWeekState)
    );
  }

  onLogoError(event: Event): void {
    const image = event.currentTarget;

    if (image instanceof HTMLImageElement && !image.src.endsWith(this.fallbackTeamLogoPath)) {
      image.src = this.fallbackTeamLogoPath;
    }
  }

  private toLoadedState(
    matches: Match[],
    context?: { seasonId: number; gameWeek: number }
  ): MatchWeekState {
    const groups = new Map<string, MatchDayGroup>();

    for (const match of matches) {
      const kickoffAt = new Date(match.kickoffAt);
      const date = new Date(kickoffAt.getFullYear(), kickoffAt.getMonth(), kickoffAt.getDate());
      const key = `${date.getFullYear()}-${date.getMonth()}-${date.getDate()}`;
      const group = groups.get(key);

      if (group) {
        group.matches.push(match);
      } else {
        groups.set(key, { date, matches: [match] });
      }
    }

    return {
      status: 'loaded',
      seasonId: matches[0]?.seasonId ?? context?.seasonId ?? null,
      seasonLabel: matches[0]?.seasonLabel ?? null,
      gameWeekNumber: matches[0]?.gameWeekNumber ?? context?.gameWeek ?? null,
      days: [...groups.values()]
    };
  }
}
