import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MediatorService, IMediator, ICreateMediator } from '../../services/mediator.service';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-admin-mediators',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatIconModule,
    MatChipsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './admin-mediators.html',
  styleUrl: './admin-mediators.css'
})
export class AdminMediators implements OnInit {
  mediators: IMediator[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';

  // Datos del formulario para crear mediador
  newMediator: ICreateMediator = {
    fullName: '',
    email: '',
    phone: '',
    specialty: ''
  };

  constructor(
    private mediatorService: MediatorService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadMediators();
  }

  loadMediators(): void {
    this.mediatorService.getAllMediators().subscribe({
      next: (mediators: IMediator[]) => {
        this.mediators = [...mediators];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error: any) => {
        console.error(error);
        this.errorMessage = 'Error al cargar los mediadores';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  // Envía los datos del formulario al backend
  createMediator(): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.mediatorService.createMediator(this.newMediator).subscribe({
      next: () => {
        this.successMessage = 'Mediador creado correctamente';

        // Limpiamos el formulario
        this.newMediator = {
          fullName: '',
          email: '',
          phone: '',
          specialty: ''
        };

        // Recargamos la lista para mostrar el nuevo mediador
        this.loadMediators();
        this.cdr.detectChanges();
      },
      error: (error: any) => {
        console.error(error);
        this.errorMessage = 'Error al crear el mediador';
        this.cdr.detectChanges();
      }
    });
  }
}