import { Component, inject } from '@angular/core';
import { FALLBACK_TEAM_LOGO_PATH, TeamsService } from '../../services/teams-service';
import { AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  imports: [AsyncPipe, RouterLink],
  selector: 'app-team-list',
  styleUrl: './team-list.css',
  templateUrl: './team-list.html',
})
export class TeamList {
  private readonly teamsService = inject(TeamsService);
  readonly teams$ = this.teamsService.getTeams();

  onLogoError(event: Event): void {
    const image = event.target;
    if (image instanceof HTMLImageElement && !image.src.endsWith(FALLBACK_TEAM_LOGO_PATH)) {
      image.src = FALLBACK_TEAM_LOGO_PATH;
    }
  }
}
