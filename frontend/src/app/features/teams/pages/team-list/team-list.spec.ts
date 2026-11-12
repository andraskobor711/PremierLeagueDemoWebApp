import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { TeamList } from './team-list';

describe('TeamList', () => {
  let fixture: ComponentFixture<TeamList>;
  let httpTesting: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamList],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([{ path: '', component: TeamList }])
      ]
    }).compileComponents();

    httpTesting = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(TeamList);
    fixture.detectChanges();
    httpTesting.expectOne(`${environment.plApiBaseUrl}/seasons`).flush([
      { id: 1, label: '2024/25' },
      { id: 2, label: '2025/26' },
    ]);
    httpTesting.expectOne(`${environment.plApiBaseUrl}/seasons/current`).flush({
      id: 2,
      label: '2025/26',
    });
    httpTesting.expectOne(`${environment.plApiBaseUrl}/seasons/2/teams`).flush([
      {
        id: 10,
        fullName: 'Current Season Team',
        usedName: 'Current Team',
        abbreviation: 'CT',
        logoSrc: null,
        officialWebsite: null,
      },
    ]);
    fixture.detectChanges();
    await fixture.whenStable();
  });

  afterEach(() => httpTesting.verify());

  it('selects the current season by default and lists its teams', () => {
    const dropdown: HTMLButtonElement = fixture.nativeElement.querySelector('#season-select');
    const teamNames = [...fixture.nativeElement.querySelectorAll('.team-name')]
      .map((element: HTMLElement) => element.textContent?.trim());

    expect(fixture.nativeElement.querySelector('#season-value').textContent.trim()).toBe('2025/26');
    expect(teamNames).toEqual(['Current Team']);

    dropdown.click();
    fixture.detectChanges();

    const seasonLabels = [...fixture.nativeElement.querySelectorAll('[role="option"]')]
      .map((element: HTMLElement) => element.textContent?.trim());
    expect(seasonLabels).toEqual(['2024/25', '2025/26']);
  });

  it('restores the selected season after recreating the page from its URL', async () => {
    const dropdown: HTMLButtonElement = fixture.nativeElement.querySelector('#season-select');
    dropdown.click();
    fixture.detectChanges();

    const previousSeason: HTMLElement = fixture.nativeElement.querySelector('#season-option-1');
    previousSeason.dispatchEvent(new MouseEvent('mouseenter'));
    previousSeason.click();
    fixture.detectChanges();
    await fixture.whenStable();

    httpTesting.expectOne(`${environment.plApiBaseUrl}/seasons/1/teams`).flush([
      {
        id: 11,
        fullName: 'Previous Season Team',
        usedName: 'Previous Team',
        abbreviation: 'PT',
        logoSrc: null,
        officialWebsite: null,
      },
    ]);
    fixture.detectChanges();

    const teamNames = [...fixture.nativeElement.querySelectorAll('.team-name')]
      .map((element: HTMLElement) => element.textContent?.trim());
    expect(TestBed.inject(Router).url).toContain('seasonId=1');
    expect(fixture.nativeElement.querySelector('#season-value').textContent.trim()).toBe('2024/25');
    expect(teamNames).toEqual(['Previous Team']);

    fixture.destroy();
    fixture = TestBed.createComponent(TeamList);
    fixture.detectChanges();
    httpTesting.expectOne(`${environment.plApiBaseUrl}/seasons`).flush([
      { id: 1, label: '2024/25' },
      { id: 2, label: '2025/26' },
    ]);
    httpTesting.expectOne(`${environment.plApiBaseUrl}/seasons/current`).flush({
      id: 2,
      label: '2025/26',
    });
    httpTesting.expectOne(`${environment.plApiBaseUrl}/seasons/1/teams`).flush([
      {
        id: 11,
        fullName: 'Previous Season Team',
        usedName: 'Previous Team',
        abbreviation: 'PT',
        logoSrc: null,
        officialWebsite: null,
      },
    ]);
    fixture.detectChanges();
    await fixture.whenStable();

    expect(fixture.nativeElement.querySelector('#season-value').textContent.trim()).toBe('2024/25');
  });
});
