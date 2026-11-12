import { Component, ElementRef, HostListener, inject } from '@angular/core';
import { FALLBACK_TEAM_LOGO_PATH, Team, TeamsService } from '../../services/teams-service';
import { AsyncPipe } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { catchError, distinctUntilChanged, forkJoin, map, Observable, of, startWith, switchMap } from 'rxjs';
import { Season } from '../../../seasons/models/season';
import { SeasonsService } from '../../../seasons/services/season-service';

interface TeamListState {
  status: 'loading' | 'seasons-error' | 'teams-loading' | 'loaded' | 'teams-error';
  seasons: Season[];
  selectedSeasonId: number | null;
  teams: Team[];
}

@Component({
  imports: [AsyncPipe, RouterLink],
  selector: 'app-team-list',
  styleUrl: './team-list.css',
  templateUrl: './team-list.html',
})
export class TeamList {
  private readonly elementRef = inject(ElementRef<HTMLElement>);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly teamsService = inject(TeamsService);
  private readonly seasonsService = inject(SeasonsService);
  isSeasonDropdownOpen = false;
  activeSeasonId: number | null = null;
  isKeyboardNavigation = false;
  readonly state$: Observable<TeamListState> = forkJoin({
    seasons: this.seasonsService.getSeasons(),
    currentSeason: this.seasonsService.getCurrentSeason(),
  }).pipe(
    switchMap(({ seasons, currentSeason }) =>
      this.route.queryParamMap.pipe(
        map(params => this.getSelectedSeasonId(params.get('seasonId'), seasons, currentSeason.id)),
        distinctUntilChanged(),
        switchMap(selectedSeasonId =>
          this.teamsService.getTeamsBySeason(selectedSeasonId).pipe(
            map(teams => ({
              status: 'loaded',
              seasons,
              selectedSeasonId,
              teams,
            }) satisfies TeamListState),
            catchError((error: unknown) => {
              console.error(`Failed to load teams for season ${selectedSeasonId}.`, error);
              return of({
                status: 'teams-error',
                seasons,
                selectedSeasonId,
                teams: [],
              } satisfies TeamListState);
            }),
            startWith({
              status: 'teams-loading',
              seasons,
              selectedSeasonId,
              teams: [],
            } satisfies TeamListState)
          )
        )
      )
    ),
    catchError((error: unknown) => {
      console.error('Failed to load seasons.', error);
      return of({
        status: 'seasons-error',
        seasons: [],
        selectedSeasonId: null,
        teams: [],
      } satisfies TeamListState);
    }),
    startWith({
      status: 'loading',
      seasons: [],
      selectedSeasonId: null,
      teams: [],
    } satisfies TeamListState)
  );

  toggleSeasonDropdown(state: TeamListState): void {
    if (this.isSeasonDropdownOpen) {
      this.closeSeasonDropdown();
      return;
    }

    this.isSeasonDropdownOpen = true;
    this.activeSeasonId = state.selectedSeasonId;
    this.isKeyboardNavigation = false;
  }

  onSeasonDropdownKeydown(event: KeyboardEvent, state: TeamListState): void {
    switch (event.key) {
      case 'ArrowDown':
      case 'ArrowUp':
        event.preventDefault();
        if (!this.isSeasonDropdownOpen) {
          this.toggleSeasonDropdown(state);
          this.isKeyboardNavigation = true;
        } else {
          this.isKeyboardNavigation = true;
          this.moveActiveSeason(state.seasons, event.key === 'ArrowDown' ? 1 : -1);
        }
        break;
      case 'Home':
        event.preventDefault();
        this.isSeasonDropdownOpen = true;
        this.activeSeasonId = state.seasons[0]?.id ?? null;
        this.isKeyboardNavigation = true;
        break;
      case 'End':
        event.preventDefault();
        this.isSeasonDropdownOpen = true;
        this.activeSeasonId = state.seasons[state.seasons.length - 1]?.id ?? null;
        this.isKeyboardNavigation = true;
        break;
      case 'Enter':
      case ' ':
        event.preventDefault();
        if (!this.isSeasonDropdownOpen) {
          this.toggleSeasonDropdown(state);
        } else if (this.activeSeasonId !== null) {
          this.selectSeason(this.activeSeasonId);
        }
        break;
      case 'Escape':
        event.preventDefault();
        this.closeSeasonDropdown();
        break;
    }
  }

  setActiveSeason(seasonId: number): void {
    this.activeSeasonId = seasonId;
    this.isKeyboardNavigation = false;
  }

  onSeasonOptionKeydown(event: KeyboardEvent, seasonId: number): void {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.selectSeason(seasonId);
    }
  }

  selectSeason(seasonId: number): void {
    this.closeSeasonDropdown();
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { seasonId },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    }).catch((error: unknown) => {
      console.error('Failed to preserve the selected season in the URL.', error);
    });
  }

  getSelectedSeasonLabel(state: TeamListState): string {
    return state.seasons.find(season => season.id === state.selectedSeasonId)?.label ?? '';
  }

  @HostListener('document:click', ['$event.target'])
  closeSeasonDropdownOnOutsideClick(target: EventTarget | null): void {
    this.closeSeasonDropdownIfOutside(target);
  }

  @HostListener('document:focusin', ['$event.target'])
  closeSeasonDropdownOnOutsideFocus(target: EventTarget | null): void {
    this.closeSeasonDropdownIfOutside(target);
  }

  onLogoError(event: Event): void {
    const image = event.target;
    if (image instanceof HTMLImageElement && !image.src.endsWith(FALLBACK_TEAM_LOGO_PATH)) {
      image.src = FALLBACK_TEAM_LOGO_PATH;
    }
  }

  private closeSeasonDropdownIfOutside(target: EventTarget | null): void {
    if (
      this.isSeasonDropdownOpen &&
      target instanceof Node &&
      !this.elementRef.nativeElement.contains(target)
    ) {
      this.closeSeasonDropdown();
    }
  }

  private moveActiveSeason(seasons: Season[], direction: 1 | -1): void {
    if (seasons.length === 0) {
      return;
    }

    const activeIndex = seasons.findIndex(season => season.id === this.activeSeasonId);
    const nextIndex =
      activeIndex === -1
        ? direction === 1
          ? 0
          : seasons.length - 1
        : (activeIndex + direction + seasons.length) % seasons.length;
    this.activeSeasonId = seasons[nextIndex].id;
  }

  private getSelectedSeasonId(
    querySeasonId: string | null,
    seasons: Season[],
    currentSeasonId: number
  ): number {
    if (querySeasonId === null || !/^\d+$/.test(querySeasonId)) {
      return currentSeasonId;
    }

    const seasonId = Number(querySeasonId);
    return Number.isSafeInteger(seasonId) && seasons.some(season => season.id === seasonId)
      ? seasonId
      : currentSeasonId;
  }

  private closeSeasonDropdown(): void {
    this.isSeasonDropdownOpen = false;
    this.activeSeasonId = null;
    this.isKeyboardNavigation = false;
  }
}
