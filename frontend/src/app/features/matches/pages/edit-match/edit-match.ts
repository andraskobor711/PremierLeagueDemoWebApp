import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { catchError, forkJoin, map, Observable, of, startWith, switchMap } from 'rxjs';
import { Team, TeamsService } from '../../../teams/services/teams-service';
import { MatchForm } from '../../components/match-form/match-form';
import { Season } from '../../models/season';
import { Match, MatchService, UpdateMatchRequest } from '../../services/match-service';

type EditMatchPageState =
  | { status: 'loading' }
  | { status: 'loaded'; match: Match; season: Season; teams: Team[] }
  | { status: 'load-error' };

@Component({
  imports: [AsyncPipe, MatchForm, RouterLink],
  selector: 'app-edit-match',
  styleUrl: './edit-match.css',
  templateUrl: './edit-match.html',
})
export class EditMatch {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly matchService = inject(MatchService);
  private readonly teamsService = inject(TeamsService);
  isSubmitting = false;
  submitError: string | null = null;

  readonly state$: Observable<EditMatchPageState> = this.route.paramMap.pipe(
    switchMap(params => {
      const idParam = params.get('id');
      const id = Number(idParam);

      if (idParam === null || !/^\d+$/.test(idParam) || !Number.isSafeInteger(id) || id <= 0) {
        return of({ status: 'load-error' } satisfies EditMatchPageState);
      }

      return forkJoin({
        match: this.matchService.getMatchById(id),
        teams: this.teamsService.getTeams(),
      }).pipe(
        map(({ match, teams }) => ({
          status: 'loaded',
          match,
          season: { id: match.seasonId, label: match.seasonLabel },
          teams,
        }) satisfies EditMatchPageState),
        catchError((error: unknown) => {
          console.error('Failed to load match for editing.', error);
          return of({ status: 'load-error' } satisfies EditMatchPageState);
        })
      );
    }),
    startWith({ status: 'loading' } satisfies EditMatchPageState)
  );

  onSubmit(request: UpdateMatchRequest, matchId: number): void {
    if (this.isSubmitting) {
      return;
    }

    this.isSubmitting = true;
    this.submitError = null;
    this.matchService.updateMatch(matchId, request).subscribe({
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
        console.error('Failed to update match.', error);
        this.submitError = 'The match could not be updated. Please try again.';
        this.isSubmitting = false;
      },
    });
  }
}
