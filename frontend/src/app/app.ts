import { Component, inject } from '@angular/core';
import { Router, RouterLinkWithHref, RouterOutlet } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEnvelope, faPhone } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from './core/auth/auth.service';

@Component({
  imports: [RouterOutlet, RouterLinkWithHref, FontAwesomeModule],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  private readonly router = inject(Router);
  readonly auth = inject(AuthService);
  readonly faEnvelope = faEnvelope;
  readonly faPhone = faPhone;

  signOut(): void {
    this.auth.logout();
    void this.router.navigate(['/login']);
  }
}
