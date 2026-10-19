import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  imports: [],
  selector: 'app-home',
  styleUrl: './home.css',
  templateUrl: './home.html',
})
export class Home {
  private readonly router = inject(Router);

  goToTeams() {
    this.router.navigate(['/teams']);
  }

  goToMatches() {
    this.router.navigate(['/matches']);
  }
}
