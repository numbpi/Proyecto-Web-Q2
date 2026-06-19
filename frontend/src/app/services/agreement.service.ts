import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthService } from './auth.service';

import {
  IAgreement,
  ICreateAgreementDto,
} from '../models/agreement.model';

@Injectable({
  providedIn: 'root',
})
export class AgreementService {
  private readonly apiURL: string =
    'http://localhost:5174/api/Agreement';

  constructor(
    private authService: AuthService,
    private http: HttpClient,
  ) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();

    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }

  create(
    dto: ICreateAgreementDto,
  ): Observable<IAgreement> {
    return this.http.post<IAgreement>(
      this.apiURL,
      dto,
      {
        headers: this.getHeaders(),
      },
    );
  }

  getByCaseId(
    caseId: string,
  ): Observable<IAgreement> {
    return this.http.get<IAgreement>(
      `${this.apiURL}/case/${caseId}`,
      {
        headers: this.getHeaders(),
      },
    );
  }

  confirm(
    agreementId: string,
    confirmed: boolean,
  ): Observable<IAgreement> {
    return this.http.put<IAgreement>(
      `${this.apiURL}/${agreementId}/confirm`,
      { confirmed },
      {
        headers: this.getHeaders(),
      },
    );
  }

  reportCompliance(
    agreementId: string,
    pointIndex: number,
    complianceStatus: string,
  ): Observable<IAgreement> {
    return this.http.put<IAgreement>(
      `${this.apiURL}/${agreementId}/compliance`,
      {
        pointIndex,
        complianceStatus,
      },
      {
        headers: this.getHeaders(),
      },
    );
  }
}
