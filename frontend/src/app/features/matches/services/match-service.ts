import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Season } from '../models/season';

export interface GameWeek {
    season: Season,
    number: number
}

export interface Match {
    id: number,
    seasonId: number,
    seasonLabel: string,
    homeTeamId: number,
    homeTeamName: string,
    homeTeamLogoSrc: string | null,
    awayTeamId: number,
    awayTeamName: string,
    awayTeamLogoSrc: string | null,
    gameWeekNumber: number | null,
    kickoffAt: string,
    homeScore: number | null,
    awayScore: number | null
}

export interface CreateMatchRequest {
    seasonId: number,
    homeTeamId: number,
    awayTeamId: number,
    gameWeekNumber: number,
    kickoffAt: string,
    homeScore: number | null,
    awayScore: number | null
}

export type UpdateMatchRequest = CreateMatchRequest;

@Service()
export class MatchService {
    private readonly matchesClient = inject(HttpClient);

    getCurrentGameWeekMatches(): Observable<Match[]> {
        return this.matchesClient.get<Match[]>(`${environment.plApiBaseUrl}/matches/current-gameweek`);
    }

    getMatchesByGameWeek(seasonId: number, gameWeek: number): Observable<Match[]> {
        return this.matchesClient.get<Match[]>(`${environment.plApiBaseUrl}/matches`, {
            params: { seasonId, gameWeek }
        });
    }

    getMatchById(id: number): Observable<Match> {
        return this.matchesClient.get<Match>(`${environment.plApiBaseUrl}/matches/${id}`);
    }

    createMatch(request: CreateMatchRequest): Observable<Match> {
        return this.matchesClient.post<Match>(`${environment.plApiBaseUrl}/matches`, request);
    }

    updateMatch(id: number, request: UpdateMatchRequest): Observable<Match> {
        return this.matchesClient.put<Match>(`${environment.plApiBaseUrl}/matches/${id}`, request);
    }
}
