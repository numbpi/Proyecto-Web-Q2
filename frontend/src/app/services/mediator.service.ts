import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

// Representa un mediador que viene del backend
export interface IMediator {
  id: string;
  fullName: string;
  email: string;
  phone: string;
  specialty: string;
  isActive: boolean;
}

// Representa los datos que vamos a enviar al backend para crear un mediador
export interface ICreateMediator {
  fullName: string;
  email: string;
  phone: string;
  specialty: string;
}

@Injectable({
  providedIn: 'root'
})
export class MediatorService {

  // URL del endpoint del backend
  private readonly apiURL = 'http://localhost:5174/api/Mediator';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  // Crea los headers con el token del usuario logueado
  getHeaders(): HttpHeaders {
    const token = this.authService.getToken();

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  // Obtiene todos los mediadores registrados
  getAllMediators(): Observable<IMediator[]> {
    return this.http.get<IMediator[]>(this.apiURL, {
      headers: this.getHeaders()
    });
  }

  // Crea un nuevo mediador
  createMediator(data: ICreateMediator): Observable<IMediator> {
    return this.http.post<IMediator>(this.apiURL, data, {
      headers: this.getHeaders()
    });
  }
}