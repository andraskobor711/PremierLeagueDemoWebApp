import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faCalendarXmark,
  faChevronLeft,
  faChevronRight,
  faFileCircleExclamation,
  faPenToSquare,
  faPlus,
} from '@fortawesome/free-solid-svg-icons';
import { catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
import { Match, MatchService } from '../../services/match-service';
import { SeasonsService } from '../../services/season-service';
import { Season } from '../../models/season';
import { FALLBACK_TEAM_LOGO_PATH } from '../../../teams/services/teams-service';

interface MatchDayGroup {
  date: Date;
  matches: Match[];
}

type GameWeekState =
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
  selector: 'app-game-week',
  styleUrl: './game-week.css',
  templateUrl: './game-week.html',
})
export class GameWeek {
  readonly errorIcon = faFileCircleExclamation;
  readonly noMatchesIcon = faCalendarXmark;
  readonly createMatchIcon = faPlus;
  readonly editMatchIcon = faPenToSquare;
  readonly previousGameWeekIcon = faChevronLeft;
  readonly nextGameWeekIcon = faChevronRight;
  readonly maxGameWeek = 38;
  private readonly route = inject(ActivatedRoute);
  private readonly matchService = inject(MatchService);
  private readonly seasonsService = inject(SeasonsService);
  private readonly fallbackTeamLogoPath = FALLBACK_TEAM_LOGO_PATH;
  readonly state$ = this.route.queryParamMap.pipe(
    switchMap(params => {
      if (this.route.snapshot.routeConfig?.path === 'matches/current-gameweek') {
        return this.loadMatches(this.matchService.getCurrentGameWeekMatches());
      }

      const seasonIdParam = params.get('seasonId');
      const gameWeekParam = params.get('gameWeek');

      if (seasonIdParam === null || gameWeekParam === null) {
        return of({ status: 'invalid-query' } satisfies GameWeekState);
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
        return of({ status: 'invalid-query' } satisfies GameWeekState);
      }

      return this.loadMatches(
        this.matchService.getMatchesByGameWeek(seasonId, gameWeek),
        { seasonId, gameWeek },
        this.seasonsService.getSeasonById(seasonId)
      );
    })
  );

  private loadMatches(
    matches$: Observable<Match[]>,
    context?: { seasonId: number; gameWeek: number },
    season$?: Observable<Season>
  ): Observable<GameWeekState> {
    return matches$.pipe(
      switchMap(matches => {
        if (matches.length > 0 || !season$) {
          return of(this.toLoadedState(matches, context));
        }

        return season$.pipe(map(season => this.toLoadedState(matches, context, season.label)));
      }),
      catchError((error: unknown) => {
        console.error('Failed to load matches.', error);
        return of({ status: 'load-error' } satisfies GameWeekState);
      }),
      startWith({ status: 'loading' } satisfies GameWeekState)
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
    context?: { seasonId: number; gameWeek: number },
    seasonLabel?: string
  ): GameWeekState {
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
      seasonLabel: matches[0]?.seasonLabel ?? seasonLabel ?? null,
      gameWeekNumber: matches[0]?.gameWeekNumber ?? context?.gameWeek ?? null,
      days: [...groups.values()]
    };
  }
}
