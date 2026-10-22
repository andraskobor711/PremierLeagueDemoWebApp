import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { of } from 'rxjs';
import { MatchService } from '../../services/match-service';
import { GameWeek } from './game-week';

describe('GameWeek', () => {
  let component: GameWeek;
  let fixture: ComponentFixture<GameWeek>;
  const matchService = {
    getCurrentGameWeekMatches: vi.fn(() => of([])),
    getMatchesByGameWeek: vi.fn(() => of([])),
  };

  beforeEach(async () => {
    matchService.getMatchesByGameWeek.mockClear();
    await TestBed.configureTestingModule({
      imports: [GameWeek],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            queryParamMap: of(convertToParamMap({ seasonId: '1', gameWeek: '5' })),
            snapshot: { routeConfig: { path: 'matches' } },
          },
        },
        { provide: MatchService, useValue: matchService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(GameWeek);
    component = fixture.componentInstance;
    fixture.detectChanges();
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('loads matches for the season and gameweek query parameters', () => {
    expect(matchService.getMatchesByGameWeek).toHaveBeenCalledWith(1, 5);
  });
});
