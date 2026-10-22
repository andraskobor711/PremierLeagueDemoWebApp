import { CanActivateFn, Routes, Router } from '@angular/router';
import { inject } from '@angular/core';
import { TeamList } from './features/teams/pages/team-list/team-list';
import { NotFound } from './shared/components/not-found/not-found';
import { Home } from './shared/components/home/home';
import { TeamDetail } from './features/teams/pages/team-detail/team-detail';
import { GameWeek } from './features/matches/pages/game-week/game-week';
import { CreateMatch } from './features/matches/pages/create-match/create-match';
import { EditMatch } from './features/matches/pages/edit-match/edit-match';

const redirectMatchesWithoutQueryParams: CanActivateFn = route => {
    if (Object.keys(route.queryParams).length > 0) {
        return true;
    }

    return inject(Router).createUrlTree(['/matches/current-gameweek']);
};

export const routes: Routes = [
    { path: 'home', component: Home },
    { path: 'teams', component: TeamList },
    { path: 'teams/:id', component: TeamDetail },
    { path: 'matches/current-gameweek', component: GameWeek },
    { path: 'matches/new', component: CreateMatch },
    { path: 'matches/:id/edit', component: EditMatch },
    { path: 'matches', component: GameWeek, canActivate: [redirectMatchesWithoutQueryParams] },

    // default route
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    // wildcard ** route - always must be the last one
    { path: '**', component: NotFound }

];
