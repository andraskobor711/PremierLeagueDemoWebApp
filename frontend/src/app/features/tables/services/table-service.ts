import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { Table } from '../models/table';

@Service()
export class TableService {
    private readonly tableClient = inject(HttpClient);

    getCurrentTable(): Observable<Table> {
        return this.tableClient.get<Table>(`${environment.plApiBaseUrl}/table/current`);
    }
}
