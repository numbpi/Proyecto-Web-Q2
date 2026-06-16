import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { Router, RouterLink } from '@angular/router';
import { CaseService } from '../../../services/case.service';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { ICase } from '../../../models/case.model';

@Component({
  selector: 'app-my-cases',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatChipsModule,
    RouterLink,
  ],
  templateUrl: './my-cases.html',
  styleUrl: './my-cases.css',
  standalone: true,
})
export class MyCases implements OnInit {
  // Lista de casos que vienen del backend
  casos: ICase[] = [];

  // Sirve para mostrar "Cargando casos..." mientras se hace la petición
  isLoading: boolean = true;

  // Sirve para guardar el ID del usuario que inició sesión
  currentUserId: string = '';

  // Sirve para mapear el estado del caso a un color del chip
  statusColor: Record<string, string> = {
    nuevo: 'accent',
    asignado: 'primary',
    'en mediacion': 'warn',
    resuelto: 'basic',
    'cerrado sin acuerdo': 'basic',
  };

  // Columnas que se muestran en la tabla
  displayedColumns: string[] = ['conflictType', 'reporterName', 'respondentName', 'status', 'createdAt'];

  constructor(
    private caseService: CaseService,
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
        this.loadCases();
      },
      error: () => {
        this.authService.logout();
        this.router.navigate(['/login']);
      },
    });
  }

  // Llama al backend usando CaseService y carga todos los casos del usuario
  private loadCases = (): void => {
    this.caseService.getMyCases().subscribe({
      next: (data) => {
        this.casos = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.authService.logout();
        this.router.navigate(['/login']);
      },
    });
  };

  // Sirve para mostrar "Yo" si el usuario es el que reportó o el denunciado
  mostrarNombre = (caso: ICase, tipo: 'reporter' | 'respondent'): string => {
    const esYo =
      tipo === 'reporter'
        ? caso.reporterId === this.currentUserId
        : caso.respondentId === this.currentUserId;

    return esYo ? 'Yo' : tipo === 'reporter' ? caso.reporterName : caso.respondentName;
  };
}
