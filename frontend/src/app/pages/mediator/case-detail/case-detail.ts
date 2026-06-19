import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { ICase } from '../../../models/case.model';
import { CaseService } from '../../../services/case.service';

@Component({
  selector: 'app-case-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDividerModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './case-detail.html',
  styleUrl: './case-detail.css',
})
export class CaseDetail implements OnInit {
  caseData: ICase | null = null;

  isLoading: boolean = true;
  isUpdating: boolean = false;

  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private caseService: CaseService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    const caseId = this.route.snapshot.paramMap.get('id');

    if (!caseId) {
      this.errorMessage = 'No se encontró el identificador del caso';
      this.isLoading = false;
      return;
    }

    this.loadCase(caseId);
  }

  loadCase(caseId: string): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.caseService.getById(caseId).subscribe({
      next: (data) => {
        this.caseData = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'No se pudo cargar el caso';
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  updateStatus(status: string): void {
    if (!this.caseData) {
      return;
    }

    this.isUpdating = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.caseService.updateStatus(this.caseData.id, status).subscribe({
      next: (updatedCase) => {
        this.caseData = updatedCase;
        this.successMessage = 'Estado actualizado correctamente';
        this.isUpdating = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'No se pudo actualizar el estado';
        this.isUpdating = false;
        this.cdr.detectChanges();
      },
    });
  }
}