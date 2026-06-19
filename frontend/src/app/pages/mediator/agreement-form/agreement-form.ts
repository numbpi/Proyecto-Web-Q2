import {
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';

import { ICase } from '../../../models/case.model';
import {
  IAgreement,
  ICreateAgreementPointDto,
} from '../../../models/agreement.model';

import { CaseService } from '../../../services/case.service';
import { AgreementService } from '../../../services/agreement.service';

interface AgreementPointForm {
  description: string;
  deadline: string;
}

@Component({
  selector: 'app-agreement-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatSelectModule,
  ],
  templateUrl: './agreement-form.html',
  styleUrl: './agreement-form.css',
})
export class AgreementForm implements OnInit {
  cases: ICase[] = [];

  caseId: string = '';
  agreementText: string = '';

  points: AgreementPointForm[] = [
    {
      description: '',
      deadline: '',
    },
  ];

  createdAgreement: IAgreement | null = null;
  minimumDate: string = new Date().toISOString().split('T')[0];

  isLoading: boolean = true;
  isSaving: boolean = false;

  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private caseService: CaseService,
    private agreementService: AgreementService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadCases();
  }

  loadCases(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.caseService.getMyCases().subscribe({
      next: (data) => {
        this.cases = data.filter(
          (item) => item.status === 'en mediacion',
        );

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message ||
          'No se pudieron cargar los casos';

        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  addPoint(): void {
    this.points.push({
      description: '',
      deadline: '',
    });
  }

  removePoint(index: number): void {
    if (this.points.length > 1) {
      this.points.splice(index, 1);
    }
  }

  createAgreement(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.createdAgreement = null;

    if (!this.caseId) {
      this.errorMessage = 'Selecciona un caso';
      return;
    }

    if (!this.agreementText.trim()) {
      this.errorMessage =
        'Escribe el resumen general del acuerdo';
      return;
    }

    const incompletePoint = this.points.some(
      (point) =>
        !point.description.trim() || !point.deadline,
    );

    if (incompletePoint) {
      this.errorMessage =
        'Completa la descripción y fecha de todos los puntos';
      return;
    }

    const agreementPoints: ICreateAgreementPointDto[] =
      this.points.map((point) => ({
        description: point.description.trim(),
        deadline: new Date(
          `${point.deadline}T00:00:00`,
        ).toISOString(),
      }));

    this.isSaving = true;

    this.agreementService
      .create({
        caseId: this.caseId,
        agreementText: this.agreementText.trim(),
        points: agreementPoints,
      })
      .subscribe({
        next: (agreement) => {
          this.createdAgreement = agreement;
          this.successMessage =
            'Acuerdo creado y enviado a las partes';

          this.cases = this.cases.filter(
            (item) => item.id !== this.caseId,
          );

          this.clearForm();
          this.isSaving = false;
          this.cdr.detectChanges();
        },
        error: (error) => {
          this.errorMessage =
            error.error?.message ||
            'No se pudo crear el acuerdo';

          this.isSaving = false;
          this.cdr.detectChanges();
        },
      });
  }

  clearForm(): void {
    this.caseId = '';
    this.agreementText = '';

    this.points = [
      {
        description: '',
        deadline: '',
      },
    ];
  }
}