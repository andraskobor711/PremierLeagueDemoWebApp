import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export const FALLBACK_TEAM_LOGO_PATH = '/TeamLogos/PremierLeague.png';

interface TeamResponse {
    usedName: string;
    logoSrc?: string | null;
}

export interface Team extends TeamResponse {
    logoPath: string;
}

@Service()
export class TeamsService {
    private readonly plApiTeamsUrl = `${environment.plApiBaseUrl}/teams`;
    private readonly teamsClient = inject(HttpClient);

    getTeams(): Observable<Team[]> {
        return this.teamsClient.get<TeamResponse[]>(this.plApiTeamsUrl).pipe(
            map(teams => teams.map(team => ({
                ...team,
                logoPath: team.logoSrc
                    ? `/TeamLogos/${team.logoSrc}.png`
                    : FALLBACK_TEAM_LOGO_PATH
            })))
        );
    }
}
