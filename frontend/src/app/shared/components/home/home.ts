import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCalendarCheck, faUsers } from '@fortawesome/free-solid-svg-icons';

@Component({
  imports: [FontAwesomeModule],
  selector: 'app-home',
  styleUrl: './home.css',
  templateUrl: './home.html',
})
export class Home {
  private readonly router = inject(Router);
  readonly faUsers = faUsers;
  readonly faCalendarCheck = faCalendarCheck;

  goToTeams() {
    this.router.navigate(['/teams']);
  }

  goToMatches() {
    this.router.navigate(['/matches/current-gameweek']);
  }
}
