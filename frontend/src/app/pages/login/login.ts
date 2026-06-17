import { ChangeDetectorRef, Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { ILoginResponse } from '../../models/user.model';
import { MatCardModule } from '@angular/material/card';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-login',
  imports: [
    MatCardModule,
    FormsModule,
    MatFormFieldModule,
    CommonModule,
    MatInputModule,
    MatButtonModule,
    RouterLink,
  ],
  templateUrl: './login.html',
  styleUrl: './login.css',
  standalone: true,
})
export class Login {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  sucessMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {}

  onLogin = (): void => {
    this.errorMessage = '';
    this.isLoading = true;

    this.authService.login(this.email, this.password).subscribe({
      next: (res: ILoginResponse): void => {
        this.sucessMessage = 'Credenciales Validas..Ingresando a la App';
        this.cdr.detectChanges();
        this.authService.saveToken(res.token);
        console.log('Login funcionando Bien');
        setTimeout(() => this.router.navigate(['/home']), 2000);
      },

      error: (): void => {
        this.errorMessage = `Credenciales Invalidas`;
        console.error('Ocurrio un Error con el Login');
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  };
}
