import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatExpansionModule } from '@angular/material/expansion';
import { Router, RouterLink } from '@angular/router';
import { CaseService } from '../../../services/case.service';
import { AgreementService } from '../../../services/agreement.service';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { ICase } from '../../../models/case.model';
import { IAgreement } from '../../../models/agreement.model';

@Component({
  selector: 'app-agreements',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatExpansionModule,
    RouterLink,
  ],
  templateUrl: './agreements.html',
  styleUrl: './agreements.css',
  standalone: true,
})
export class Agreements implements OnInit {
  // Lista de acuerdos con su caso correspondiente
  agreements: { caso: ICase; acuerdo: IAgreement | null }[] = [];

  // Sirve para guardar el ID del usuario que inició sesión
  currentUserId: string = '';

  // Sirve para mostrar "Cargando acuerdos..." mientras se hace la petición
  isLoading: boolean = true;

  // Sirve para mostrar "Confirmando..." mientras se confirma
  confirmingId: string | null = null;

  // Sirve para mostrar mensajes de error
  errorMessage: string = '';

  constructor(
    private caseService: CaseService,
    private agreementService: AgreementService,
    private userService: UserService,
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

    // Primero obtiene los datos del usuario para saber quién es
    this.userService.getCurrentUser().subscribe({
      next: (user) => {
        this.currentUserId = user.id;
        this.loadAgreements();
      },
      error: () => {
        this.authService.logout();
        this.router.navigate(['/login']);
      },
    });
  }

  // Trae los casos del usuario y filtra los que están en mediación para mostrar sus acuerdos
  private loadAgreements = (): void => {
    this.caseService.getMyCases().subscribe({
      next: (casos) => {
        // Solo interesan los casos que están en mediación
        const enMediacion = casos.filter((c) => c.status === 'en mediacion');

        if (enMediacion.length === 0) {
          this.isLoading = false;
          this.cdr.detectChanges();
          return;
        }

        // Por cada caso en mediación, busca su acuerdo
        let completed = 0;
        enMediacion.forEach((caso) => {
          this.agreementService.getByCaseId(caso.id).subscribe({
            next: (acuerdo) => {
              this.agreements.push({ caso, acuerdo });
              completed++;
              if (completed === enMediacion.length) {
                this.isLoading = false;
                this.cdr.detectChanges();
              }
            },
            error: () => {
              this.agreements.push({ caso, acuerdo: null });
              completed++;
              if (completed === enMediacion.length) {
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

  // Sirve para saber si el usuario es el que reportó el caso
  isReporter = (caso: ICase): boolean => caso.reporterId === this.currentUserId;

  // Sirve para saber si el usuario es el denunciado
  isRespondent = (caso: ICase): boolean =>
    caso.respondentId === this.currentUserId;

  // Sirve para saber si el usuario ya confirmó el acuerdo
  hasConfirmed = (caso: ICase, acuerdo: IAgreement): boolean =>
    this.isReporter(caso)
      ? acuerdo.confirmedByReporter
      : acuerdo.confirmedByRespondent;

  // Sirve para saber si la otra persona ya confirmó
  otherConfirmed = (caso: ICase, acuerdo: IAgreement): boolean =>
    this.isReporter(caso)
      ? acuerdo.confirmedByRespondent
      : acuerdo.confirmedByReporter;

  // Confirma el acuerdo llamando al backend
  onConfirm = (acuerdo: IAgreement): void => {
    this.confirmingId = acuerdo.id;
    this.errorMessage = '';
    this.cdr.detectChanges();

    this.agreementService.confirm(acuerdo.id, true).subscribe({
      next: () => {
        this.confirmingId = null;
        // Recarga los acuerdos para actualizar el estado
        this.agreements = [];
        this.loadAgreements();
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Error al confirmar';
        this.confirmingId = null;
        this.cdr.detectChanges();
      },
    });
  };
}
