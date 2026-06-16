import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { ICurrentUser } from '../../models/user.model';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChip } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [
  CommonModule,
  MatCardModule,
  MatIconModule,
  MatChip,
  MatButtonModule
],
  templateUrl: './admin-users.html',
  styleUrl: './admin-users.css'
})
export class AdminUsers implements OnInit {
  // Lista donde se guardan los usuarios que vienen del backend
  users: ICurrentUser[] = [];

  // Sirve para mostrar "Cargando usuarios..." mientras se hace la petición
  isLoading = true;
  

  // Sirve para mostrar un mensaje si ocurre un error
  errorMessage = '';

  constructor(private userService: UserService, private cdr: ChangeDetectorRef ) {}

  // Se ejecuta automáticamente cuando se abre /admin/users
  ngOnInit(): void {
    this.loadUsers();
  }

  // Llama al backend usando UserService y carga todos los usuarios
  loadUsers(): void {
    console.log('Cargando usuarios...');

  this.userService.getAllUsers().subscribe({
    next: (users: ICurrentUser[]) => {
      console.log('Usuarios recibidos:', users);

      this.users = users;
      this.isLoading = false;
      this.cdr.detectChanges()
    },
    error: (error: any) => {
      console.error('Error cargando usuarios:', error);

      this.errorMessage = 'Error al cargar los usuarios';
      this.isLoading = false;
    }
    });
  }
}
