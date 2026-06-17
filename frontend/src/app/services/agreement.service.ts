import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAgreement } from '../models/agreement.model';

@Injectable({ providedIn: 'root' })
export class AgreementService {
  // URL base del API de acuerdos
  protected readonly apiURL: string = 'http://localhost:5174/api/Agreement';

  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) {}

  // Sirve para armar el header con el token JWT
  private getHeaders = (): HttpHeaders => {
    const token = this.authService.getToken();
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  };

  // Trae el acuerdo de un caso específico
  getByCaseId = (caseId: string): Observable<IAgreement> =>
    this.http.get<IAgreement>(`${this.apiURL}/case/${caseId}`, {
      headers: this.getHeaders(),
    });

  // Confirma o rechaza un acuerdo (solo el que reportó o el denunciado)
  confirm = (agreementId: string, confirmed: boolean): Observable<IAgreement> =>
    this.http.put<IAgreement>(
      `${this.apiURL}/${agreementId}/confirm`,
      { confirmed },
      { headers: this.getHeaders() },
    );

  // Reporta si un punto del acuerdo se cumplió o no
  reportCompliance = (
    agreementId: string,
    pointIndex: number,
    complianceStatus: string,
  ): Observable<IAgreement> =>
    this.http.put<IAgreement>(
      `${this.apiURL}/${agreementId}/compliance`,
      { pointIndex, complianceStatus },
      { headers: this.getHeaders() },
    );
}
