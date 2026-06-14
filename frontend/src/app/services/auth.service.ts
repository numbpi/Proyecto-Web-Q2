import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ILoginResponse, IRegisterResponse } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  protected readonly apiURL: string = 'http://localhost:5174/api/Auth';

  constructor(private http: HttpClient) {}

  /**
   *
   */
  register = (fullName: string, email: string, password: string): Observable<IRegisterResponse> =>
    this.http.post<IRegisterResponse>(`${this.apiURL}/register`, { fullName, email, password });

  forgotPassword = (email: string): Observable<{ message: string }> =>
    this.http.post<{ message: string }>(`${this.apiURL}/forgot-password`, { email });

  resetPassword = (token: string, newPassword: string): Observable<{ message: string }> =>
    this.http.post<{ message: string }>(`${this.apiURL}/reset-password`, { token, newPassword });

  /**
   * @param {string} email - correo electronico del usuario
   * @param {string} password - contraseña del usuario
   * @returns va retornar el JWT Token del usuario
   * @description Iniciar sesión con las credenciales del usuario
   */
  login = (email: string, password: string): Observable<ILoginResponse> =>
    this.http.post<ILoginResponse>(`${this.apiURL}/login`, { email, password });

  saveToken = (token: string): void => localStorage.setItem('token', token);

  getToken = (): string | null => localStorage.getItem('token');

  isLogged = (): boolean => this.getToken() !== null;

  logout = (): void => localStorage.removeItem('token');
}
