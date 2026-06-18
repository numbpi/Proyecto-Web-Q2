import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CaseService, ICase } from '../../services/case.service';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-admin-cases',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatChipsModule
  ],
  templateUrl: './admin-cases.html',
  styleUrl: './admin-cases.css'
})
export class AdminCases implements OnInit {
  cases: ICase[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private caseService: CaseService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCases();
  }

  // Carga todos los casos desde el backend
  loadCases(): void {
    this.caseService.getAllCases().subscribe({
      next: (cases: ICase[]) => {
        this.cases = [...cases];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error: any) => {
        console.error(error);
        this.errorMessage = 'Error al cargar los casos';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}