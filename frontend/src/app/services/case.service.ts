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
  mediatorId?: string;
  evidenceUrls: string[];
  createdAt: string;
  assignedAt?: string;
  closedAt?: string;
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
  // Lo usan las vistas del ciudadano.
  getMyCases = (): Observable<ICase[]> =>
    this.http.get<ICase[]>(`${this.apiURL}`, {
      headers: this.getHeaders(),
    });

  // Envía los datos al backend para crear un caso nuevo.
  // Lo usa la vista del ciudadano.
  create = (dto: ICreateCaseDto): Observable<ICase> =>
    this.http.post<ICase>(`${this.apiURL}`, dto, {
      headers: this.getHeaders(),
    });
}