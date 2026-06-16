import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { ICurrentUser } from '../models/user.model';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class UserService {
  protected readonly apiURL: string = 'http://localhost:5174/api/User';

  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) {}

  getHeaders = (): HttpHeaders => {
    const token = this.authService.getToken();
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  };

  // Obtiene los datos del usuario que inició sesión
  getCurrentUser = (): Observable<ICurrentUser> =>
    this.http.get<ICurrentUser>(`${this.apiURL}/me`, {
      headers: this.getHeaders(),
    });

  // Obtiene todos los usuarios registrados para la vista del administrador
  getAllUsers = (): Observable<ICurrentUser[]> =>
    this.http.get<ICurrentUser[]>(`${this.apiURL}`, {
      headers: this.getHeaders(),
    });
}