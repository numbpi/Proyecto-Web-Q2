import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';

import { ICase } from '../../../models/case.model';
import { CaseService } from '../../../services/case.service';

@Component({
  selector: 'app-assigned-cases',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTableModule,
  ],
  templateUrl: './assigned-cases.html',
  styleUrl: './assigned-cases.css',
})
export class AssignedCases implements OnInit {
  cases: ICase[] = [];
  displayedColumns: string[] = [
    'conflictType',
    'parties',
    'status',
    'actions',
  ];

  isLoading: boolean = true;
  errorMessage: string = '';

  constructor(
    private caseService: CaseService,
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
        this.cases = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'No se pudieron cargar los casos asignados';
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }
}