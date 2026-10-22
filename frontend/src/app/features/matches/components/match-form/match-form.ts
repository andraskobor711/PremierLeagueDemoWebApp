import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Season } from '../../models/season';
import { CreateMatchRequest, Match } from '../../services/match-service';
import { Team } from '../../../teams/services/teams-service';
import { TeamPicker } from '../team-picker/team-picker';

@Component({
  imports: [ReactiveFormsModule, TeamPicker],
  selector: 'app-match-form',
  styleUrl: './match-form.css',
  templateUrl: './match-form.html',
})
export class MatchForm implements OnChanges {
  @Input({ required: true }) teams: Team[] = [];
  @Input({ required: true }) season!: Season;
  @Input() initialMatch: Match | null = null;
  @Input() initialGameWeek = 1;
  @Input() editing = false;
  @Input() submitting = false;
  @Input() submitError: string | null = null;
  @Output() readonly matchSubmit = new EventEmitter<CreateMatchRequest>();

  readonly form = new FormGroup(
    {
      homeTeamId: new FormControl<number | null>(null, [
        Validators.required,
        Validators.min(1),
      ]),
      awayTeamId: new FormControl<number | null>(null, [
        Validators.required,
        Validators.min(1),
      ]),
      gameWeekNumber: new FormControl<number | null>(1, [
        Validators.required,
        Validators.min(1),
        Validators.max(38),
      ]),
      kickoffAt: new FormControl('', Validators.required),
      homeScore: new FormControl<number | null>(null, [
        Validators.min(0),
        Validators.pattern(/^\d+$/),
      ]),
      awayScore: new FormControl<number | null>(null, [
        Validators.min(0),
        Validators.pattern(/^\d+$/),
      ]),
    },
    {
      validators: control => {
        const homeTeamId = control.get('homeTeamId')?.value;
        const awayTeamId = control.get('awayTeamId')?.value;
        return homeTeamId !== null && homeTeamId === awayTeamId
          ? { sameTeams: true }
          : null;
      },
    }
  );

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialMatch'] && this.initialMatch) {
      this.form.patchValue({
        homeTeamId: this.initialMatch.homeTeamId,
        awayTeamId: this.initialMatch.awayTeamId,
        gameWeekNumber: this.initialMatch.gameWeekNumber,
        kickoffAt: this.toLocalDateTime(this.initialMatch.kickoffAt),
        homeScore: this.initialMatch.homeScore,
        awayScore: this.initialMatch.awayScore,
      });
    } else if (changes['initialGameWeek']) {
      this.form.controls.gameWeekNumber.setValue(this.initialGameWeek);
    }
  }

  submit(): void {
    this.form.markAllAsTouched();

    if (this.form.invalid || this.submitting) {
      return;
    }

    const { homeTeamId, awayTeamId, gameWeekNumber, kickoffAt, homeScore, awayScore } =
      this.form.getRawValue();

    if (
      homeTeamId === null ||
      awayTeamId === null ||
      gameWeekNumber === null ||
      !kickoffAt
    ) {
      return;
    }

    this.matchSubmit.emit({
      seasonId: this.season.id,
      homeTeamId,
      awayTeamId,
      gameWeekNumber,
      kickoffAt: new Date(kickoffAt).toISOString(),
      homeScore,
      awayScore,
    });
  }

  private toLocalDateTime(value: string): string {
    const date = new Date(value);
    const localDate = new Date(date.getTime() - date.getTimezoneOffset() * 60_000);
    return localDate.toISOString().slice(0, 16);
  }
}
