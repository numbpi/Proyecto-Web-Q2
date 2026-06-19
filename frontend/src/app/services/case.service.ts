import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICase, ICreateCaseDto } from '../models/case.model';

@Injectable({ providedIn: 'root' })
export class CaseService {
  private readonly apiURL: string = 'http://localhost:5174/api/Case';

  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) {}

  private getHeaders = (): HttpHeaders => {
    const token = this.authService.getToken();

    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  };

  getAllCases = (): Observable<ICase[]> =>
    this.http.get<ICase[]>(this.apiURL, {
      headers: this.getHeaders(),
    });

  getMyCases = (): Observable<ICase[]> =>
    this.http.get<ICase[]>(this.apiURL, {
      headers: this.getHeaders(),
    });

  getById = (caseId: string): Observable<ICase> =>
    this.http.get<ICase>(`${this.apiURL}/${caseId}`, {
      headers: this.getHeaders(),
    });

  create = (dto: ICreateCaseDto): Observable<ICase> =>
    this.http.post<ICase>(this.apiURL, dto, {
      headers: this.getHeaders(),
    });

  updateStatus = (
    caseId: string,
    status: string,
  ): Observable<ICase> =>
    this.http.put<ICase>(
      `${this.apiURL}/${caseId}/status`,
      { status },
      { headers: this.getHeaders() },
    );

  assignMediator = (
    caseId: string,
    mediatorId: string,
  ): Observable<ICase> =>
    this.http.put<ICase>(
      `${this.apiURL}/${caseId}/assign`,
      { mediatorId },
      { headers: this.getHeaders() },
    );
}