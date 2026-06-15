import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterLink],
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
  ) {}

  // Este metodo es cuando se renderiza las weas de esto, se podria decir que es como un main
  ngOnInit(): void {
    if (!this.authService.isLogged()) {
      this.onLogout();
      return;
    }

    this.userService.getCurrentUser().subscribe({
      next: (res) => {
        this.userName = res.fullName ?? 'User';
        this.userRole = res.role ?? 'ciudadano';
        this.isLoading = false;
      },
      error: () => this.onLogout(),
    });
  }

  onLogout = (): void => {
    this.authService.logout();
    this.router.navigate(['/login']);
  };
}
