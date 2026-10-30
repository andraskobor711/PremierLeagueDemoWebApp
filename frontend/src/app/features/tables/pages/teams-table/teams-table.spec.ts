import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { TableService } from '../../services/table-service';
import { TeamsTable } from './teams-table';

describe('TeamsTable', () => {
  let component: TeamsTable;
  let fixture: ComponentFixture<TeamsTable>;
  const table = {
    seasonId: 1,
    seasonLabel: '2025/26',
    rows: [
      {
        position: 1,
        teamName: 'First FC',
        logoSrc: 'first-fc',
        gamesPlayed: 1,
        wins: 1,
        draws: 0,
        losses: 0,
        goalsFor: 2,
        goalsAgainst: 0,
        goalDifference: 2,
        points: 3,
      },
      {
        position: 2,
        teamName: 'Second FC',
        logoSrc: 'second-fc',
        gamesPlayed: 1,
        wins: 0,
        draws: 1,
        losses: 0,
        goalsFor: 1,
        goalsAgainst: 1,
        goalDifference: 0,
        points: 1,
      },
    ],
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamsTable],
      providers: [
        {
          provide: TableService,
          useValue: { getCurrentTable: () => of(table) },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(TeamsTable);
    component = fixture.componentInstance;
    fixture.detectChanges();
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display standings in the backend order', () => {
    const rows = fixture.nativeElement.querySelectorAll(
      'tbody tr'
    ) as NodeListOf<HTMLTableRowElement>;

    expect(Array.from(rows, row => row.cells[0].textContent?.trim())).toEqual(['1', '2']);
    expect(Array.from(rows, row => row.cells[1].textContent?.trim())).toEqual([
      'First FC',
      'Second FC',
    ]);
    expect(Array.from(rows, row => row.cells[1].querySelector('img')?.getAttribute('src')))
      .toEqual(['/TeamLogos/first-fc.png', '/TeamLogos/second-fc.png']);
  });

  it('should sort a new column descending, then reverse direction on repeated clicks', () => {
    const wonButton = Array.from(
      fixture.nativeElement.querySelectorAll('button.sort-button') as NodeListOf<HTMLButtonElement>
    ).find(button => button.textContent?.trim().startsWith('Won'));

    expect(wonButton).toBeTruthy();
    wonButton?.click();
    fixture.detectChanges();

    let rows = fixture.nativeElement.querySelectorAll(
      'tbody tr'
    ) as NodeListOf<HTMLTableRowElement>;
    expect(Array.from(rows, row => row.cells[1].textContent?.trim())).toEqual([
      'First FC',
      'Second FC',
    ]);

    wonButton?.click();
    fixture.detectChanges();

    rows = fixture.nativeElement.querySelectorAll('tbody tr') as NodeListOf<HTMLTableRowElement>;
    expect(Array.from(rows, row => row.cells[1].textContent?.trim())).toEqual([
      'Second FC',
      'First FC',
    ]);
  });

  it('should restore the backend order when reset is selected', () => {
    component.sortBy('draws');
    fixture.detectChanges();
    fixture.nativeElement.querySelector('.reset-sort-button').click();
    fixture.detectChanges();

    const rows = fixture.nativeElement.querySelectorAll(
      'tbody tr'
    ) as NodeListOf<HTMLTableRowElement>;
    expect(Array.from(rows, row => row.cells[1].textContent?.trim())).toEqual([
      'First FC',
      'Second FC',
    ]);
    expect(fixture.nativeElement.querySelector('.reset-sort-button').disabled).toBe(true);
  });
});
