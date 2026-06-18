import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

import { UserService } from '../../services/user.service';
import { MediatorService } from '../../services/mediator.service';
import { CaseService } from '../../services/case.service';
import { ReportService } from '../../services/report.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboard implements OnInit {

  totalUsers = 0;
  totalMediators = 0;
  totalCases = 0;
  totalReports = 0;

  constructor(
    private userService: UserService,
    private mediatorService: MediatorService,
    private caseService: CaseService,
    private reportService: ReportService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    console.log('Cargando dashboard...');

    this.userService.getAllUsers().subscribe({
      next: (users) => {
        console.log('Usuarios dashboard:', users);
        this.totalUsers = users.length;
        this.cdr.detectChanges();
      },
      error: (error) => console.error('Error usuarios dashboard:', error)
    });

    this.mediatorService.getAllMediators().subscribe({
      next: (mediators) => {
        console.log('Mediadores dashboard:', mediators);
        this.totalMediators = mediators.length;
        this.cdr.detectChanges();
      },
      error: (error) => console.error('Error mediadores dashboard:', error)
    });

    this.caseService.getAllCases().subscribe({
      next: (cases) => {
        console.log('Casos dashboard:', cases);
        this.totalCases = cases.length;
        this.cdr.detectChanges();
      },
      error: (error) => console.error('Error casos dashboard:', error)
    });

    this.reportService.getAllReports().subscribe({
      next: (reports) => {
        console.log('Reportes dashboard:', reports);
        this.totalReports = reports.length;
        this.cdr.detectChanges();
      },
      error: (error) => console.error('Error reportes dashboard:', error)
    });
  }
}