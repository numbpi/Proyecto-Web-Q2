import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { Router, RouterLink } from '@angular/router';
import { CaseService } from '../../../services/case.service';
import { AgreementService } from '../../../services/agreement.service';
import { AuthService } from '../../../services/auth.service';
import { ICase } from '../../../models/case.model';
import { IAgreement } from '../../../models/agreement.model';

@Component({
  selector: 'app-compliance',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    RouterLink,
  ],
  templateUrl: './compliance.html',
  styleUrl: './compliance.css',
  standalone: true,
})
export class Compliance implements OnInit {
  // Lista de acuerdos formalizados con su caso correspondiente
  agreements: { caso: ICase; acuerdo: IAgreement }[] = [];

  // Sirve para mostrar "Cargando..." mientras se hace la petición
  isLoading: boolean = true;

  // Sirve para mostrar "Reportando..." mientras se envía
  reportingId: string | null = null;
  reportingPoint: number | null = null;

  // Sirve para mostrar mensajes de éxito o error
  successMessage: string = '';
  errorMessage: string = '';

  constructor(
    private caseService: CaseService,
    private agreementService: AgreementService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {}

  // Se ejecuta automáticamente cuando se abre la página
  ngOnInit(): void {
    if (!this.authService.isLogged()) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadCompliance();
  }

  // Trae los casos resueltos y busca sus acuerdos formalizados
  private loadCompliance = (): void => {
    this.caseService.getMyCases().subscribe({
      next: (casos) => {
        // Solo interesan los casos resueltos o cerrados
        const resueltos = casos.filter(
          (c) => c.status === 'resuelto' || c.status === 'cerrado sin acuerdo',
        );

        if (resueltos.length === 0) {
          this.isLoading = false;
          this.cdr.detectChanges();
          return;
        }

        // Por cada caso resuelto, busca si tiene acuerdo formalizado
        let completed = 0;
        const total = resueltos.length;

        resueltos.forEach((caso) => {
          this.agreementService.getByCaseId(caso.id).subscribe({
            next: (acuerdo) => {
              // Solo muestra acuerdos que ya están formalizados
              if (acuerdo.formalizedAt) {
                this.agreements.push({ caso, acuerdo });
              }
              completed++;
              if (completed === total) {
                this.isLoading = false;
                this.cdr.detectChanges();
              }
            },
            error: () => {
              completed++;
              if (completed === total) {
                this.isLoading = false;
                this.cdr.detectChanges();
              }
            },
          });
        });
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  };

  // Envia al backend el reporte de cumplimiento de un punto
  onReportCompliance = (
    acuerdo: IAgreement,
    pointIndex: number,
    status: string,
  ): void => {
    this.reportingId = acuerdo.id;
    this.reportingPoint = pointIndex;
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();

    this.agreementService
      .reportCompliance(acuerdo.id, pointIndex, status)
      .subscribe({
        next: () => {
          this.reportingId = null;
          this.reportingPoint = null;
          this.successMessage = 'Cumplimiento reportado con éxito';

          // Recarga para actualizar el estado de los puntos
          setTimeout(() => {
            this.successMessage = '';
            this.agreements = [];
            this.loadCompliance();
          }, 2000);

          this.cdr.detectChanges();
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Error al reportar';
          this.reportingId = null;
          this.reportingPoint = null;
          this.cdr.detectChanges();
        },
      });
  };
}
