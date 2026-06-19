import {
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';

import { ICase } from '../../../models/case.model';
import { IMediationSession } from '../../../models/session.model';
import { CaseService } from '../../../services/case.service';
import { SessionService } from '../../../services/session.service';

@Component({
  selector: 'app-session-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatSelectModule,
  ],
  templateUrl: './session-form.html',
  styleUrl: './session-form.css',
})
export class SessionForm implements OnInit {
  cases: ICase[] = [];
  sessions: IMediationSession[] = [];

  caseId: string = '';
  scheduledDate: string = '';
  scheduledTime: string = '';
  modality: string = 'presencial';
  meetingLink: string = '';
  sessionNotes: string = '';

  minimumDate: string = new Date().toISOString().split('T')[0];

  isLoading: boolean = true;
  isSaving: boolean = false;
  updatingSessionId: string = '';

  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private caseService: CaseService,
    private sessionService: SessionService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.errorMessage = '';

    forkJoin({
      cases: this.caseService.getMyCases(),
      sessions: this.sessionService.getMySessions(),
    }).subscribe({
      next: (response) => {
        this.cases = response.cases.filter((item) =>
          ['asignado', 'en mediacion'].includes(item.status),
        );

        this.sessions = response.sessions;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message ||
          'No se pudo cargar la información de las sesiones';

        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  createSession(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.caseId || !this.scheduledDate || !this.scheduledTime) {
      this.errorMessage =
        'Selecciona el caso, la fecha y la hora';
      return;
    }

    if (this.modality === 'virtual' && !this.meetingLink.trim()) {
      this.errorMessage =
        'Ingresa el enlace para la sesión virtual';
      return;
    }

    this.isSaving = true;

    this.sessionService
      .create({
        caseId: this.caseId,
        scheduledDate: new Date(
          `${this.scheduledDate}T00:00:00`,
        ).toISOString(),
        scheduledTime: this.scheduledTime,
        modality: this.modality,
        meetingLink:
          this.modality === 'virtual' ? this.meetingLink : '',
        sessionNotes: this.sessionNotes,
      })
      .subscribe({
        next: (session) => {
          this.sessions = [...this.sessions, session];
          this.successMessage =
            'Sesión programada correctamente';

          this.clearForm();
          this.isSaving = false;
          this.cdr.detectChanges();
        },
        error: (error) => {
          this.errorMessage =
            error.error?.message ||
            'No se pudo programar la sesión';

          this.isSaving = false;
          this.cdr.detectChanges();
        },
      });
  }

  completeSession(session: IMediationSession): void {
    this.updatingSessionId = session.id;
    this.errorMessage = '';
    this.successMessage = '';

    this.sessionService
      .updateStatus(
        session.id,
        'realizada',
        session.sessionNotes || '',
      )
      .subscribe({
        next: (updatedSession) => {
          this.sessions = this.sessions.map((item) =>
            item.id === updatedSession.id
              ? updatedSession
              : item,
          );

          this.successMessage =
            'Resultado de la sesión registrado';

          this.updatingSessionId = '';
          this.cdr.detectChanges();
        },
        error: (error) => {
          this.errorMessage =
            error.error?.message ||
            'No se pudo actualizar la sesión';

          this.updatingSessionId = '';
          this.cdr.detectChanges();
        },
      });
  }

  caseName(caseId: string): string {
    const selectedCase = this.cases.find(
      (item) => item.id === caseId,
    );

    if (!selectedCase) {
      return `Caso ${caseId.substring(0, 8)}`;
    }

    return `${selectedCase.conflictType}: ${selectedCase.reporterName} / ${selectedCase.respondentName}`;
  }

  clearForm(): void {
    this.caseId = '';
    this.scheduledDate = '';
    this.scheduledTime = '';
    this.modality = 'presencial';
    this.meetingLink = '';
    this.sessionNotes = '';
  }
}