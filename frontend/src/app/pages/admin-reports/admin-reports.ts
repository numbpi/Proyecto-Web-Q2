import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportService, IReport } from '../../services/report.service';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-admin-reports',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatChipsModule
  ],
  templateUrl: './admin-reports.html',
  styleUrl: './admin-reports.css'
})
export class AdminReports implements OnInit {

  reports: IReport[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private reportService: ReportService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadReports();
  }

  // Obtiene todos los reportes del sistema
  loadReports(): void {

    this.reportService.getAllReports().subscribe({

      next: (reports: IReport[]) => {

        this.reports = [...reports];

        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: (error: any) => {

        console.error(error);

        this.errorMessage = 'Error al cargar los reportes';

        this.isLoading = false;

        this.cdr.detectChanges();
      }
    });
  }
}