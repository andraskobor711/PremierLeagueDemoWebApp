import {
  Component,
  ElementRef,
  forwardRef,
  Input,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { FALLBACK_TEAM_LOGO_PATH, Team } from '../../../teams/services/teams-service';

@Component({
  selector: 'app-team-picker',
  styleUrl: './team-picker.css',
  templateUrl: './team-picker.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TeamPicker),
      multi: true,
    },
  ],
})
export class TeamPicker implements ControlValueAccessor, OnDestroy {
  @Input({ required: true }) teams: Team[] = [];
  @Input({ required: true }) label = '';
  @Input({ required: true }) name = '';
  @ViewChild('picker') private picker!: ElementRef<HTMLDetailsElement>;

  value: number | null = null;
  disabled = false;
  private onChange: (value: number | null) => void = () => {};
  private onTouched: () => void = () => {};
  private typeaheadBuffer = '';
  private typeaheadTimer: ReturnType<typeof setTimeout> | undefined;

  get selectedTeam(): Team | undefined {
    return this.teams.find(team => team.id === this.value);
  }

  writeValue(value: number | null): void {
    this.value = value;
  }

  registerOnChange(fn: (value: number | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(disabled: boolean): void {
    this.disabled = disabled;
  }

  onKeydown(event: KeyboardEvent): void {
    if (this.disabled) {
      return;
    }

    if (event.key === 'Escape' && this.picker.nativeElement.open) {
      event.preventDefault();
      this.picker.nativeElement.open = false;
      this.picker.nativeElement.querySelector('summary')?.focus();
      return;
    }

    if (event.key === 'Tab' && this.picker.nativeElement.open) {
      event.preventDefault();
      const activeOption = document.activeElement instanceof HTMLInputElement &&
        document.activeElement.type === 'radio'
        ? document.activeElement
        : undefined;
      const selectedOption = this.getTeamOptions().find(
        option => Number(option.value) === this.value
      );
      const teamId = Number((activeOption ?? selectedOption)?.value);

      if (Number.isSafeInteger(teamId) && teamId > 0) {
        this.selectTeam(teamId);
      } else {
        this.picker.nativeElement.open = false;
        this.onTouched();
        this.picker.nativeElement.querySelector('summary')?.focus();
      }

      this.focusAdjacentFormElement(event.shiftKey);
      return;
    }

    if (
      event.key === 'Enter' &&
      this.picker.nativeElement.open &&
      event.target instanceof HTMLInputElement &&
      event.target.type === 'radio'
    ) {
      event.preventDefault();
      this.selectTeam(Number(event.target.value));
      return;
    }

    if (event.key === 'ArrowDown' && !this.picker.nativeElement.open) {
      event.preventDefault();
      this.picker.nativeElement.open = true;
      this.focusTeamOption(event.key);
      return;
    }

    if (
      this.picker.nativeElement.open &&
      (event.key === 'ArrowDown' || event.key === 'ArrowUp')
    ) {
      event.preventDefault();
      this.focusTeamOption(event.key);
      return;
    }

    if (
      event.altKey ||
      event.ctrlKey ||
      event.metaKey ||
      event.key.length !== 1 ||
      event.key.trim() === ''
    ) {
      return;
    }

    event.preventDefault();

    const key = event.key.toLocaleLowerCase();
    const repeatedCharacter = this.typeaheadBuffer.length > 0 &&
      [...this.typeaheadBuffer].every(character => character === key);
    this.typeaheadBuffer = repeatedCharacter ? key : this.typeaheadBuffer + key;

    if (this.typeaheadTimer !== undefined) {
      clearTimeout(this.typeaheadTimer);
    }
    this.typeaheadTimer = setTimeout(() => {
      this.typeaheadBuffer = '';
      this.typeaheadTimer = undefined;
    }, 700);

    const radioButtons = this.getTeamOptions();
    const activeRadioIndex = radioButtons.findIndex(radio => radio === event.target);
    const selectedIndex = this.teams.findIndex(team => team.id === this.value);
    const startIndex = activeRadioIndex >= 0
      ? activeRadioIndex
      : selectedIndex;
    const searchFrom = repeatedCharacter || this.typeaheadBuffer.length === 1
      ? startIndex + 1
      : startIndex;

    for (let offset = 0; offset < this.teams.length; offset++) {
      const index = (searchFrom + offset + this.teams.length) % this.teams.length;
      const team = this.teams[index];

      if (team.usedName.toLocaleLowerCase().startsWith(this.typeaheadBuffer)) {
        this.picker.nativeElement.open = true;
        radioButtons.find(radio => Number(radio.value) === team.id)?.focus();
        return;
      }
    }
  }

  private focusTeamOption(direction: 'ArrowDown' | 'ArrowUp'): void {
    const radioButtons = this.getTeamOptions();

    if (radioButtons.length === 0) {
      return;
    }

    const activeIndex = radioButtons.findIndex(radio => radio === document.activeElement);
    const selectedIndex = radioButtons.findIndex(radio => Number(radio.value) === this.value);
    let nextIndex: number;

    if (activeIndex >= 0) {
      const step = direction === 'ArrowDown' ? 1 : -1;
      nextIndex = (activeIndex + step + radioButtons.length) % radioButtons.length;
    } else if (selectedIndex >= 0) {
      nextIndex = selectedIndex;
    } else {
      nextIndex = direction === 'ArrowDown' ? 0 : radioButtons.length - 1;
    }

    radioButtons[nextIndex].focus();
  }

  private getTeamOptions(): HTMLInputElement[] {
    return Array.from(
      this.picker.nativeElement.querySelectorAll<HTMLInputElement>('input[type="radio"]')
    );
  }

  private focusAdjacentFormElement(reverse: boolean): void {
    const picker = this.picker.nativeElement;
    const trigger = picker.querySelector('summary');
    const form = picker.closest('form');

    if (!trigger || !form) {
      return;
    }

    const focusableElements = Array.from(
      form.querySelectorAll<HTMLElement>(
        'a[href], button:not([disabled]), input:not([disabled]), select:not([disabled]), textarea:not([disabled]), summary, [tabindex]:not([tabindex="-1"])'
      )
    ).filter(element => {
      if (
        element.hidden ||
        element.getAttribute('aria-hidden') === 'true' ||
        element.matches(':disabled')
      ) {
        return false;
      }

      const parentDetails = element.closest('details');
      return !parentDetails || parentDetails.open || element === parentDetails.querySelector('summary');
    });
    const triggerIndex = focusableElements.indexOf(trigger);
    const nextElement = focusableElements[triggerIndex + (reverse ? -1 : 1)];

    nextElement?.focus();
  }

  selectTeam(teamId: number): void {
    this.value = teamId;
    this.onChange(teamId);
    this.onTouched();
    this.picker.nativeElement.open = false;
    this.picker.nativeElement.querySelector('summary')?.focus();
  }

  onLogoError(event: Event): void {
    const image = event.currentTarget;
    if (image instanceof HTMLImageElement && !image.src.endsWith(FALLBACK_TEAM_LOGO_PATH)) {
      image.src = FALLBACK_TEAM_LOGO_PATH;
    }
  }

  ngOnDestroy(): void {
    if (this.typeaheadTimer !== undefined) {
      clearTimeout(this.typeaheadTimer);
    }
  }
}
