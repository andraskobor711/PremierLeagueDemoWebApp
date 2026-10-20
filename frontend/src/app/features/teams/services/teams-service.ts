import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export const FALLBACK_TEAM_LOGO_PATH = '/TeamLogos/PremierLeague.png';

export interface TeamResponse {
    id: number;
    fullName: string;
    usedName: string;
    abbreviation: string;
    logoSrc?: string | null;
    officialWebsite: string | null;
}

export interface Team extends TeamResponse {
    logoPath: string;
}

@Service()
export class TeamsService {
    private readonly teamsClient = inject(HttpClient);

    getTeams(): Observable<Team[]> {
        return this.teamsClient.get<TeamResponse[]>(`${environment.plApiBaseUrl}/teams`).pipe(
            map(teams => teams.map(team => this.toTeam(team)))
        );
    }

    getTeamById(id: number): Observable<Team> {
        return this.teamsClient.get<TeamResponse>(`${environment.plApiBaseUrl}/teams/${id}`).pipe(
            map(team => this.toTeam(team))
        );
    }

    private toTeam(team: TeamResponse): Team {
        return {
            ...team,
            logoPath: team.logoSrc
                ? `/TeamLogos/${team.logoSrc}.png`
                : FALLBACK_TEAM_LOGO_PATH
        };
    }
}
