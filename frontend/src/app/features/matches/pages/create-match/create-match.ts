import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, forkJoin, map, Observable, of, startWith } from 'rxjs';
import { Team, TeamsService } from '../../../teams/services/teams-service';
import { MatchForm } from '../../components/match-form/match-form';
import { CreateMatchRequest, MatchService } from '../../services/match-service';
import { Season } from '../../models/season';
import { SeasonsService } from '../../services/season-service';

type CreateMatchPageState =
  | { status: 'loading' }
  | { status: 'loaded'; season: Season; teams: Team[] }
  | { status: 'load-error' };

@Component({
  imports: [AsyncPipe, MatchForm],
  selector: 'app-create-match',
  styleUrl: './create-match.css',
  templateUrl: './create-match.html',
})
export class CreateMatch {
  private readonly matchService = inject(MatchService);
  private readonly seasonsService = inject(SeasonsService);
  private readonly teamsService = inject(TeamsService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  isSubmitting = false;
  submitError: string | null = null;
  readonly initialGameWeek = this.getInitialGameWeek();

  readonly state$: Observable<CreateMatchPageState> = forkJoin({
    season: this.seasonsService.getCurrentSeason(),
    teams: this.teamsService.getTeams(),
  }).pipe(
    map(({ season, teams }) => ({ status: 'loaded', season, teams }) satisfies CreateMatchPageState),
    catchError((error: unknown) => {
      console.error('Failed to load data for match creation.', error);
      return of({ status: 'load-error' } satisfies CreateMatchPageState);
    }),
    startWith({ status: 'loading' } satisfies CreateMatchPageState)
  );

  onSubmit(request: CreateMatchRequest): void {
    if (this.isSubmitting) {
      return;
    }

    this.isSubmitting = true;
    this.submitError = null;
    this.matchService.createMatch(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        void this.router.navigate(['/matches'], {
          queryParams: {
            seasonId: request.seasonId,
            gameWeek: request.gameWeekNumber,
          },
        });
      },
      error: (error: unknown) => {
        console.error('Failed to create match.', error);
        this.submitError = 'The match could not be created. Please try again.';
        this.isSubmitting = false;
      },
    });
  }

  private getInitialGameWeek(): number {
    const value = this.route.snapshot.queryParamMap.get('gameWeek');

    if (value === null || !/^\d+$/.test(value)) {
      return 1;
    }

    const gameWeek = Number(value);
    return Number.isSafeInteger(gameWeek) && gameWeek >= 1 && gameWeek <= 38
      ? gameWeek
      : 1;
  }
}
