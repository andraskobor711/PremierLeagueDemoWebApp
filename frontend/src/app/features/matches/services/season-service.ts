import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { Season } from '../models/season';

@Service()
export class SeasonsService {
    private readonly seasonsClient = inject(HttpClient);

    getCurrentSeason(): Observable<Season> {
        return this.seasonsClient.get<Season>(`${environment.plApiBaseUrl}/seasons/current`);
    }

    getSeasonById(id: number): Observable<Season> {
        return this.seasonsClient.get<Season>(`${environment.plApiBaseUrl}/seasons/${id}`);
    }
}
