import { HttpErrorResponse } from '@angular/common/http';
import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import { catchError, map, of, switchMap } from 'rxjs';
import { FALLBACK_TEAM_LOGO_PATH, TeamsService } from '../../services/teams-service';

@Component({
  imports: [AsyncPipe, FontAwesomeModule, RouterLink],
  selector: 'app-team-detail',
  styleUrl: './team-detail.css',
  templateUrl: './team-detail.html',
})
export class TeamDetail {
  readonly notFoundIcon = faMagnifyingGlass;
  private readonly fallbackTeamLogoPath = FALLBACK_TEAM_LOGO_PATH;
  private readonly route = inject(ActivatedRoute);
  private readonly teamsService = inject(TeamsService);
  errorMessage = '';
  notFound = false;

  readonly team$ = this.route.paramMap.pipe(
    map(params => Number(params.get('id'))),
    switchMap(id => {
      this.errorMessage = '';
      this.notFound = false;
      if (!Number.isSafeInteger(id) || id <= 0) {
        this.errorMessage = 'Team details could not be loaded.';
        return of(null);
      }

      return this.teamsService.getTeamById(id).pipe(
        catchError((error: unknown) => {
          this.notFound = error instanceof HttpErrorResponse && error.status === 404;
          this.errorMessage = this.notFound
            ? 'Team could not be found.'
            : 'Team details could not be loaded.';
          return of(null);
        })
      );
    })
  );

  onLogoError(event: Event): void {
    const image = event.target;
    if (image instanceof HTMLImageElement && !image.src.endsWith(this.fallbackTeamLogoPath)) {
      image.src = this.fallbackTeamLogoPath;
    }
  }
}
