import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPerson } from '../_entities/person';
import { environment } from 'src/environments/environment.prod';
import { IPaginationResult } from '../_entities/pagination-result';

@Injectable({
  providedIn: 'root'
})
export class PersonsService {

  private url = environment.apiUrl + 'persons';
  constructor(private httpClient: HttpClient) { }

  loadAll(
    lastNameFilter?: string,
    pageIndex?: number,
    pageSize?: number,
    sortColumn?: string,
    sortDirection?: string): Observable<IPaginationResult<IPerson>> {
    const params = new HttpParams()
      .set('lastNameFilter', lastNameFilter == null ? '' : lastNameFilter)
      .set('pageIndex', pageIndex == null ? '0' : pageIndex.toString())
      .set('pageSize', pageSize == null ? '10' : pageSize.toString())
      .set('sortColumn', sortColumn == null ? 'lastName' : sortColumn)
      .set('sortDirection', sortDirection == null ? 'asc' : sortDirection);
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.httpClient.get<IPaginationResult<IPerson>>(this.url, { headers: headers, params: params });
  }

  create(person: IPerson): Observable<IPerson> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.httpClient.post<IPerson>(this.url + '/' + person.id, JSON.stringify(person), { headers: headers });
  }

  update(person: IPerson): Observable<IPerson> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.httpClient.put<IPerson>(this.url + '/' + person.id, JSON.stringify(person), { headers: headers });
  }

  delete(id: number): Observable<number> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.httpClient.delete<number>(this.url + '/' + id, { headers: headers });
  }
}
