import { Component, inject } from '@angular/core';
import { TeamsService } from '../../services/teams-service';
import { AsyncPipe, JsonPipe } from '@angular/common';

@Component({
  imports: [AsyncPipe, JsonPipe],
  selector: 'app-team-list',
  styleUrl: './team-list.css',
  templateUrl: './team-list.html',
})
export class TeamList {
  private readonly teamsService = inject(TeamsService);
  readonly teams$ = this.teamsService.getTeams();
}
