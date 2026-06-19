import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
  ICreateSessionDto,
  IMediationSession,
} from '../models/session.model';

import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class SessionService {
  private readonly apiURL: string =
    'http://localhost:5174/api/Session';

  constructor(
    private http: HttpClient,
    private authService: AuthService,
  ) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();

    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }

  getMySessions(): Observable<IMediationSession[]> {
    return this.http.get<IMediationSession[]>(
      `${this.apiURL}/my`,
      {
        headers: this.getHeaders(),
      },
    );
  }

  create(
    dto: ICreateSessionDto,
  ): Observable<IMediationSession> {
    return this.http.post<IMediationSession>(
      this.apiURL,
      dto,
      {
        headers: this.getHeaders(),
      },
    );
  }

  updateStatus(
    sessionId: string,
    status: string,
    sessionNotes: string,
  ): Observable<IMediationSession> {
    return this.http.put<IMediationSession>(
      `${this.apiURL}/${sessionId}/status`,
      {
        status,
        sessionNotes,
      },
      {
        headers: this.getHeaders(),
      },
    );
  }
}