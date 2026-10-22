import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { of } from 'rxjs';
import { MatchService } from '../../services/match-service';
import { MatchWeek } from './match-week';

describe('MatchWeek', () => {
  let component: MatchWeek;
  let fixture: ComponentFixture<MatchWeek>;
  const matchService = {
    getCurrentGameWeekMatches: vi.fn(() => of([])),
    getMatchesByGameWeek: vi.fn(() => of([])),
  };

  beforeEach(async () => {
    matchService.getMatchesByGameWeek.mockClear();
    await TestBed.configureTestingModule({
      imports: [MatchWeek],
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

    fixture = TestBed.createComponent(MatchWeek);
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
