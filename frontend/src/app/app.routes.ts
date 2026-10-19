import { Routes } from '@angular/router';
import { TeamList } from './features/teams/pages/team-list/team-list';
import { App } from './app';
import { NotFound } from './shared/components/not-found/not-found';
import { Home } from './shared/components/home/home';

export const routes: Routes = [
    { path: 'home', component: Home },
    { path: 'teams', component: TeamList },

    // default route
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    // wildcard ** route - always must be the last one
    { path: '**', component: NotFound }

];
