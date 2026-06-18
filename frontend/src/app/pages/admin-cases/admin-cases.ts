import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CaseService } from '../../services/case.service';
import { ICase } from '../../models/case.model';
import { MediatorService, IMediator } from '../../services/mediator.service';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-admin-cases',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatIconModule,
    MatChipsModule,
    MatButtonModule,
    MatSelectModule,
  ],
  templateUrl: './admin-cases.html',
  styleUrl: './admin-cases.css'
})
export class AdminCases implements OnInit {
  cases: ICase[] = [];
  mediators: IMediator[] = [];
  isLoading = true;
  errorMessage = '';

  // Controla qué caso está mostrando el selector de mediadores
  assigningCaseId: string | null = null;
  // Guarda el mediador seleccionado temporalmente por caso
  selectedMediatorId: string = '';

  constructor(
    private caseService: CaseService,
    private mediatorService: MediatorService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCases();
  }

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

  // Muestra el selector de mediadores para un caso
  showAssign(caseId: string): void {
    this.assigningCaseId = caseId;
    this.selectedMediatorId = '';

    // Carga los mediadores disponibles si no los tenemos
    if (this.mediators.length === 0) {
      this.mediatorService.getAllMediators().subscribe({
        next: (mediators: IMediator[]) => {
          this.mediators = mediators.filter(m => m.isAvailable);
          this.cdr.detectChanges();
        },
        error: (error: any) => {
          console.error(error);
          this.errorMessage = 'Error al cargar mediadores';
          this.cdr.detectChanges();
        }
      });
    }
  }

  // Asigna el mediador seleccionado al caso
  assignMediator(caseId: string): void {
    if (!this.selectedMediatorId) return;

    this.caseService.assignMediator(caseId, this.selectedMediatorId).subscribe({
      next: () => {
        this.assigningCaseId = null;
        this.selectedMediatorId = '';
        this.loadCases(); // Recarga para ver el cambio
      },
      error: (error: any) => {
        console.error(error);
        this.errorMessage = 'Error al asignar mediador';
        this.cdr.detectChanges();
      }
    });
  }

  // Cancela la asignación
  cancelAssign(): void {
    this.assigningCaseId = null;
    this.selectedMediatorId = '';
  }
}
