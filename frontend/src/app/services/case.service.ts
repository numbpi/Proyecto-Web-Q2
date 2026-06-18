import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

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

@Injectable({
  providedIn: 'root'
})
export class CaseService {
  private readonly apiURL = 'http://localhost:5174/api/Case';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  getHeaders(): HttpHeaders {
    const token = this.authService.getToken();

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  // Obtiene todos los casos registrados
  getAllCases(): Observable<ICase[]> {
    return this.http.get<ICase[]>(this.apiURL, {
      headers: this.getHeaders()
    });
  }
}