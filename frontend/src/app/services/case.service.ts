import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


import { ICreateCaseDto } from '../models/case.model';

// Representa un caso que viene del backend
export interface ICase {
  id: string;
  reporterId: string;
  reporterName: string;
  respondentId: string;
  respondentName: string;
  conflictType: string;
  description: string;
  address: string;
  status: string;
  mediatorId: string | null;
  evidenceUrls: string[];
  createdAt: string;
  assignedAt: string | null;
closedAt: string | null;
}


@Injectable({ providedIn: 'root' })
export class CaseService {
  private readonly apiURL: string = 'http://localhost:5174/api/Case';

  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) {}

  // Sirve para armar el header con el token JWT
  private getHeaders = (): HttpHeaders => {
    const token = this.authService.getToken();
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  };

  // Trae todos los casos. Admin lo usa para ver todos los casos del sistema.
  getAllCases = (): Observable<ICase[]> =>
    this.http.get<ICase[]>(`${this.apiURL}`, {
      headers: this.getHeaders(),
    });

  // Trae los casos del usuario que inició sesión.
  getMyCases = (): Observable<ICase[]> =>
    this.http.get<ICase[]>(`${this.apiURL}`, {
      headers: this.getHeaders(),
    });

  // Envía los datos al backend para crear un caso nuevo.
  create = (dto: ICreateCaseDto): Observable<ICase> =>
    this.http.post<ICase>(`${this.apiURL}`, dto, {
      headers: this.getHeaders(),
    });

  // Cambia el estado de un caso (solo mediador)
  updateStatus = (caseId: string, status: string): Observable<ICase> =>
    this.http.put<ICase>(`${this.apiURL}/${caseId}/status`, { status }, {
      headers: this.getHeaders(),
    });

  // Asigna un mediador a un caso (solo admin)
  assignMediator = (caseId: string, mediatorId: string): Observable<ICase> =>
    this.http.put<ICase>(`${this.apiURL}/${caseId}/assign`, { mediatorId }, {
      headers: this.getHeaders(),
    });
}