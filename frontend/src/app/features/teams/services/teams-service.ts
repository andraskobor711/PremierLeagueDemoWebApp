import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Service()
export class TeamsService {
    private readonly plApiTeamsUrl = `${environment.plApiBaseUrl}/teams`;
    private readonly teamsClient = inject(HttpClient);

    getTeams() {
        return this.teamsClient.get<any[]>(this.plApiTeamsUrl);
    }
}
