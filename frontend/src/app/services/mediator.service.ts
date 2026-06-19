import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

// Representa un mediador que viene del backend
export interface IMediator {
  id: string;
  fullName: string;
  email?: string;
  zone: string;
  specialty: string;
  isAvailable: boolean;
  isActive: boolean;
  activeCases: number;
  userId: string | null;
  createdAt: string;
}

// Representa los datos que vamos a enviar al backend para crear un mediador.
// El backend busca el usuario por email y completa fullName solo.
export interface ICreateMediator {
  email: string;
  zone: string;
  specialty: string;
  isAvailable: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MediatorService {

  private readonly apiURL = 'http://localhost:5174/api/Mediator';

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

  getAllMediators(): Observable<IMediator[]> {
    return this.http.get<IMediator[]>(this.apiURL, {
      headers: this.getHeaders()
    });
  }

  createMediator(data: ICreateMediator): Observable<IMediator> {
    return this.http.post<IMediator>(this.apiURL, data, {
      headers: this.getHeaders()
    });
  }
}