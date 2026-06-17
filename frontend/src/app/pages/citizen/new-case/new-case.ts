import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CaseService } from '../../../services/case.service';
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { IUserSearchResult } from '../../../models/case.model';

@Component({
  selector: 'app-new-case',
  imports: [
    CommonModule,
    MatCardModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    FormsModule,
    RouterLink,
  ],
  templateUrl: './new-case.html',
  styleUrl: './new-case.css',
  standalone: true,
})
export class NewCase implements OnInit {
  // Email que escribe el usuario para buscar al denunciado
  respondentEmail: string = '';

  // Datos de la persona denunciada cuando se encuentra
  respondent: IUserSearchResult | null = null;

  // Tipo de conflicto seleccionado
  conflictType: string = '';

  // Descripción del conflicto
  description: string = '';

  // Dirección del conflicto
  address: string = '';

  // Sirve para mostrar "Enviando..." mientras se crea el caso
  isLoading: boolean = false;

  // Sirve para mostrar "Buscando..." mientras se busca el usuario
  isSearching: boolean = false;

  // Sirve para mostrar mensajes de error
  errorMessage: string = '';
  searchErrorMessage: string = '';

  // Sirve para mostrar mensaje de éxito
  successMessage: string = '';

  // Lista de tipos de conflicto disponibles
  conflictTypes: string[] = [
    'Ruido',
    'Mascotas',
    'Basura',
    'Áreas comunes',
    'Límites de propiedad',
    'Otros',
  ];

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
    }
  }

  // Busca un usuario por email usando el servicio de usuarios
  onSearchRespondent = (): void => {
    this.searchErrorMessage = '';
    this.respondent = null;

    if (!this.respondentEmail.trim()) {
      this.searchErrorMessage = 'Ingresá un email para buscar';
      this.cdr.detectChanges();
      return;
    }

    this.isSearching = true;
    this.cdr.detectChanges();

    this.userService.getAllUsers().subscribe({
      next: (users) => {
        const found = users.find(
          (u) =>
            u.email.toLowerCase() === this.respondentEmail.toLowerCase(),
        );

        if (found) {
          this.respondent = found;
        } else {
          this.searchErrorMessage = 'No se encontró ningún usuario con ese email';
        }

        this.isSearching = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.searchErrorMessage = 'Error al buscar el usuario';
        this.isSearching = false;
        this.cdr.detectChanges();
      },
    });
  };

  // Envia los datos al backend para crear el caso
  onSubmit = (): void => {
    this.errorMessage = '';
    this.successMessage = '';

    // Validaciones antes de enviar
    if (!this.respondent) {
      this.errorMessage = 'Primero buscá y seleccioná el denunciado';
      this.cdr.detectChanges();
      return;
    }

    if (!this.conflictType) {
      this.errorMessage = 'Seleccioná el tipo de conflicto';
      this.cdr.detectChanges();
      return;
    }

    if (!this.description.trim()) {
      this.errorMessage = 'Escribí una descripción del conflicto';
      this.cdr.detectChanges();
      return;
    }

    if (!this.address.trim()) {
      this.errorMessage = 'Ingresá la dirección del conflicto';
      this.cdr.detectChanges();
      return;
    }

    this.isLoading = true;
    this.cdr.detectChanges();

    this.caseService
      .create({
        respondentId: this.respondent.id,
        conflictType: this.conflictType,
        description: this.description,
        address: this.address,
      })
      .subscribe({
        next: () => {
          this.successMessage = 'Caso reportado con éxito';
          this.isLoading = false;
          this.cdr.detectChanges();

          // Redirige a Mis Casos después de 1.5 segundos
          setTimeout(() => this.router.navigate(['/citizen/my-cases']), 1500);
        },
        error: (err) => {
          this.errorMessage =
            err.error?.message || 'Error al reportar el caso';
          this.isLoading = false;
          this.cdr.detectChanges();
        },
      });
  };
}
