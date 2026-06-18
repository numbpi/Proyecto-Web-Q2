import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

// Representa un reporte del backend
export interface IReport {
  id: string;
  type: string;
  generatedBy: string;
  filters: { [key: string]: any };
  data: { [key: string]: any };
  createdAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private readonly apiURL = 'http://localhost:5174/api/Reports';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  // Crea los headers con el token
  getHeaders(): HttpHeaders {
    const token = this.authService.getToken();

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  // Obtiene todos los reportes
  getAllReports(): Observable<IReport[]> {
    return this.http.get<IReport[]>(this.apiURL, {
      headers: this.getHeaders()
    });
  }
}