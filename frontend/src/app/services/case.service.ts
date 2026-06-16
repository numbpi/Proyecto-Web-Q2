import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICase, ICreateCaseDto } from '../models/case.model';

@Injectable({ providedIn: 'root' })
export class CaseService {
  // URL base del API de casos
  protected readonly apiURL: string = 'http://localhost:5174/api/Case';

  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) {}

  // Sirve para armar el header con el token JWT
  private getHeaders = (): HttpHeaders => {
    const token = this.authService.getToken();
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  };

  // Trae todos los casos del usuario que inició sesión
  getMyCases = (): Observable<ICase[]> =>
    this.http.get<ICase[]>(`${this.apiURL}`, {
      headers: this.getHeaders(),
    });

  // Envia los datos al backend para crear un caso nuevo
  create = (dto: ICreateCaseDto): Observable<ICase> =>
    this.http.post<ICase>(`${this.apiURL}`, dto, {
      headers: this.getHeaders(),
    });
}
