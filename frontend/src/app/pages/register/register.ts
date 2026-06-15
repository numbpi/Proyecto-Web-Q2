import { ChangeDetectorRef, Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    RouterLink,
  ],
  templateUrl: './register.html',
  styleUrl: './register.css',
  standalone: true,
})
export class Register {
  fullName: string = '';
  email: string = '';
  password: string = '';
  isRegister: boolean = false;
  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {}

  onRegister = (): void => {
    this.errorMessage = '';
    this.isRegister = true;

    this.authService.register(this.fullName, this.email, this.password).subscribe({
      next: (): void => {
        console.log('Registro exitoso');
        this.router.navigate(['/home']);
      },

      error: (err: HttpErrorResponse): void => {
        this.errorMessage = err.error.message;
        console.error(`Ocurrio un Error con el Register: ${err.error.message}`);
        this.isRegister = false;
        this.cdr.detectChanges();
      },
    });
  };
}
