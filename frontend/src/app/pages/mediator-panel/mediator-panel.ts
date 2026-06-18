import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface PanelCase {
  id: string;
  reporterName: string;
  respondentName: string;
  conflictType: string;
  description: string;
  address: string;
  status: string;
  mediatorId: string | null;
  createdAt: string;
}

interface PanelSession {
  id: string;
  caseId: string;
  mediatorId: string;
  scheduledDate: string;
  scheduledTime: string;
  modality: string;
  meetingLink: string;
  status: string;
  sessionNotes: string;
  createdAt: string;
}

interface SessionForm {
  caseId: string;
  scheduledDate: string;
  scheduledTime: string;
  modality: string;
  meetingLink: string;
  sessionNotes: string;
}

interface ResultForm {
  status: string;
  sessionNotes: string;
}

@Component({
  selector: 'app-mediator-panel',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './mediator-panel.html',
  styleUrl: './mediator-panel.css',
})
export class MediatorPanel implements OnInit {
  cases: PanelCase[] = [];
  sessions: PanelSession[] = [];

  selectedCase: PanelCase | null = null;
  selectedSession: PanelSession | null = null;

  isLoadingCases = false;
  isLoadingSessions = false;

  errorMessage = '';
  successMessage = '';

  sessionForm: SessionForm = {
    caseId: '',
    scheduledDate: '',
    scheduledTime: '',
    modality: 'presencial',
    meetingLink: '',
    sessionNotes: '',
  };

  resultForm: ResultForm = {
    status: 'realizada',
    sessionNotes: '',
  };

  agreementForm = {
    agreementText: '',
    deadline: '',
  };

  ngOnInit(): void {
    this.loadAssignedCases();
    this.loadMySessions();
  }

  loadAssignedCases(): void {
    this.errorMessage = '';
    this.isLoadingCases = true;

    this.cases = [
      {
        id: 'CASE-001',
        reporterName: 'Carlos Ramírez',
        respondentName: 'Ana López',
        conflictType: 'Ruido excesivo',
        description:
          'El vecino reporta música a alto volumen durante la noche y dificultad para descansar.',
        address: 'Colonia Los Álamos, bloque 4, casa 12',
        status: 'Asignado',
        mediatorId: 'MED-001',
        createdAt: '2026-06-18',
      },
      {
        id: 'CASE-002',
        reporterName: 'María Fernández',
        respondentName: 'José Martínez',
        conflictType: 'Mascotas',
        description:
          'Se reporta que una mascota permanece suelta y causa molestias a otros vecinos.',
        address: 'Residencial San Miguel, calle principal',
        status: 'Pendiente de aceptación',
        mediatorId: 'MED-001',
        createdAt: '2026-06-18',
      },
      {
        id: 'CASE-003',
        reporterName: 'Luis Hernández',
        respondentName: 'Pedro Castillo',
        conflictType: 'Uso de estacionamiento',
        description:
          'Existe desacuerdo por el uso frecuente de un espacio de estacionamiento compartido.',
        address: 'Condominios Vista Verde, torre B',
        status: 'En mediación',
        mediatorId: 'MED-001',
        createdAt: '2026-06-18',
      },
    ];

    this.isLoadingCases = false;
  }

  loadMySessions(): void {
    this.isLoadingSessions = true;

    this.sessions = [
      {
        id: 'SES-001',
        caseId: 'CASE-001',
        mediatorId: 'MED-001',
        scheduledDate: '2026-06-20',
        scheduledTime: '10:30',
        modality: 'virtual',
        meetingLink: 'https://meet.google.com/demo-session',
        status: 'programada',
        sessionNotes: 'Primera reunión para escuchar a ambas partes.',
        createdAt: '2026-06-18',
      },
      {
        id: 'SES-002',
        caseId: 'CASE-003',
        mediatorId: 'MED-001',
        scheduledDate: '2026-06-22',
        scheduledTime: '15:00',
        modality: 'presencial',
        meetingLink: '',
        status: 'pendiente',
        sessionNotes: 'Revisión de posibles acuerdos.',
        createdAt: '2026-06-18',
      },
    ];

    this.isLoadingSessions = false;
  }

  selectCase(caseItem: PanelCase): void {
    this.selectedCase = caseItem;
    this.sessionForm.caseId = caseItem.id;
    this.successMessage = '';
    this.errorMessage = '';
  }

  acceptCase(caseItem: PanelCase): void {
    caseItem.status = 'En mediación';
    this.successMessage = 'Caso aceptado correctamente.';
    this.errorMessage = '';
  }

  requestReassign(caseItem: PanelCase): void {
    caseItem.status = 'Reasignación solicitada';
    this.successMessage = 'Solicitud de reasignación registrada.';
    this.errorMessage = '';
  }

  createSession(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedCase) {
      this.errorMessage = 'Seleccione un caso antes de programar una sesión.';
      return;
    }

    if (!this.sessionForm.scheduledDate || !this.sessionForm.scheduledTime) {
      this.errorMessage = 'Debe ingresar fecha y hora para la sesión.';
      return;
    }

    if (
      this.sessionForm.modality === 'virtual' &&
      !this.sessionForm.meetingLink.trim()
    ) {
      this.errorMessage = 'Debe ingresar un enlace para sesiones virtuales.';
      return;
    }

    const newSession: PanelSession = {
      id: `SES-${this.sessions.length + 1}`,
      caseId: this.selectedCase.id,
      mediatorId: 'MED-001',
      scheduledDate: this.sessionForm.scheduledDate,
      scheduledTime: this.sessionForm.scheduledTime,
      modality: this.sessionForm.modality,
      meetingLink: this.sessionForm.meetingLink,
      status: 'programada',
      sessionNotes: this.sessionForm.sessionNotes,
      createdAt: new Date().toISOString(),
    };

    this.sessions.unshift(newSession);

    this.successMessage = 'Sesión programada correctamente.';

    this.sessionForm = {
      caseId: this.selectedCase.id,
      scheduledDate: '',
      scheduledTime: '',
      modality: 'presencial',
      meetingLink: '',
      sessionNotes: '',
    };
  }

  selectSession(session: PanelSession): void {
    this.selectedSession = session;

    this.resultForm = {
      status: session.status || 'realizada',
      sessionNotes: session.sessionNotes || '',
    };

    this.successMessage = '';
    this.errorMessage = '';
  }

  updateSessionResult(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedSession) {
      this.errorMessage = 'Seleccione una sesión antes de registrar resultado.';
      return;
    }

    this.selectedSession.status = this.resultForm.status;
    this.selectedSession.sessionNotes = this.resultForm.sessionNotes;

    this.successMessage = 'Resultado de sesión actualizado correctamente.';
  }

  saveAgreementDraft(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedCase) {
      this.errorMessage = 'Seleccione un caso antes de registrar un acuerdo.';
      return;
    }

    if (!this.agreementForm.agreementText.trim()) {
      this.errorMessage = 'Ingrese el texto del acuerdo.';
      return;
    }

    if (!this.agreementForm.deadline) {
      this.errorMessage = 'Ingrese el plazo de cumplimiento.';
      return;
    }

    this.successMessage =
      'Borrador de acuerdo registrado correctamente en el panel.';

    this.agreementForm = {
      agreementText: '',
      deadline: '',
    };
  }
}