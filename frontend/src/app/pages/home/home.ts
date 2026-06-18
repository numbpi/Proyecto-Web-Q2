import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';

const Roles: Record<string, string> = {
  admin: 'ADMINISTRADOR',
  mediator: 'MEDIADOR',
};

@Component({
  selector: 'app-home',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    RouterLink,
  ],
  templateUrl: './home.html',
  styleUrl: './home.css',
  standalone: true,
})
export class Home implements OnInit {
  userName: string = '';
  userRole: string = '';
  isLoading: boolean = true;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {}

  // Este metodo es cuando se renderiza las weas de esto, se podria decir que es como un main
  ngOnInit(): void {
    if (!this.authService.isLogged()) {
      this.onLogout();
      return;
    }

    this.userService.getCurrentUser().subscribe({
      next: (res) => {
        this.userName = res.fullName.toLocaleUpperCase() ?? 'User';
        this.userRole = Roles[res.role] ?? 'CIUDADANO';
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => this.onLogout(),
    });
  }

  onLogout = (): void => {
    this.authService.logout();
    this.router.navigate(['/login']);
  };
}
