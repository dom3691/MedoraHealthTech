import { Component, computed, input } from '@angular/core';

const ICONS: Record<string, string[]> = {
  ambulance: ['M4 17h2m12 0h2M6 17a2 2 0 1 0 4 0 2 2 0 0 0-4 0Zm8 0a2 2 0 1 0 4 0 2 2 0 0 0-4 0ZM5 17V7h9v10M14 9h3l3 4v4M7 10h4M9 8v4'],
  alert: ['M12 3 2 21h20L12 3Zm0 6v5m0 4h.01'],
  'arrow-right': ['M5 12h14M13 5l7 7-7 7'],
  audit: ['M9 3h6l1 2h3v16H5V5h3l1-2Zm0 6h6M9 13h6M9 17h3'],
  available: ['M16 21v-2a4 4 0 0 0-8 0v2M12 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8Zm5-1 2 2 4-4'],
  bed: ['M4 13V5m0 8h16m0 0v6M4 19v-6m0 0h7V8H7a3 3 0 0 0-3 3v2Zm16 0h-9V9h5a4 4 0 0 1 4 4Z'],
  bell: ['M18 8a6 6 0 1 0-12 0c0 7-3 7-3 9h18c0-2-3-2-3-9ZM10 21h4'],
  briefcase: ['M10 6V5a2 2 0 0 1 2-2h0a2 2 0 0 1 2 2v1M4 7h16v12H4V7Zm0 5h16'],
  building: ['M4 21V5a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v16M8 7h1m4 0h1M8 11h1m4 0h1M8 15h1m4 0h1M3 21h18'],
  calendar: ['M7 3v4m10-4v4M4 9h16M5 5h14a1 1 0 0 1 1 1v14H4V6a1 1 0 0 1 1-1Z'],
  chart: ['M4 19V5M4 19h16M8 16v-5m4 5V8m4 8v-9'],
  check: ['M20 6 9 17l-5-5'],
  clipboard: ['M9 4h6l1 2h3v15H5V6h3l1-2Zm0 6h6M9 14h6M9 18h4'],
  clock: ['M12 22a10 10 0 1 0 0-20 10 10 0 0 0 0 20Zm0-15v6l4 2'],
  dollar: ['M12 2v20m5-16H9a3 3 0 0 0 0 6h6a3 3 0 0 1 0 6H6'],
  exit: ['M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4M10 17l5-5-5-5M15 12H3'],
  flask: ['M9 3h6M10 3v6l-5 9a3 3 0 0 0 3 4h8a3 3 0 0 0 3-4l-5-9V3M8 15h8'],
  grid: ['M4 4h6v6H4V4Zm10 0h6v6h-6V4ZM4 14h6v6H4v-6Zm10 0h6v6h-6v-6Z'],
  hospital: ['M3 21h18M5 21V5a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2v16M9 9h6M12 6v6M8 21v-5h8v5'],
  invoice: ['M7 3h10v18H7V3Zm3 5h4M10 12h4M10 16h2'],
  leave: ['M16 21v-2a4 4 0 0 0-8 0v2M12 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8Zm6-5 4 4m0-4-4 4'],
  list: ['M8 6h13M8 12h13M8 18h13M3 6h.01M3 12h.01M3 18h.01'],
  'log-out': ['M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4M16 17l5-5-5-5M21 12H9'],
  pill: ['M10 21a6 6 0 0 1-4.24-10.24l5-5a6 6 0 0 1 8.48 8.48l-5 5A6 6 0 0 1 10 21Zm-1.5-8.5 6 6'],
  plus: ['M12 5v14M5 12h14'],
  pulse: ['M3 12h4l2-7 4 14 2-7h6'],
  report: ['M6 3h9l3 3v15H6V3Zm8 0v4h4M9 13h6M9 17h6'],
  reset: ['M3 12a9 9 0 1 0 3-6.7M3 4v6h6'],
  scan: ['M7 3H5a2 2 0 0 0-2 2v2m14-4h2a2 2 0 0 1 2 2v2M7 21H5a2 2 0 0 1-2-2v-2m14 4h2a2 2 0 0 0 2-2v-2M8 12h8'],
  scissors: ['M4 7a3 3 0 1 0 6 0 3 3 0 0 0-6 0Zm0 10a3 3 0 1 0 6 0 3 3 0 0 0-6 0Zm5-8 11 8M9 15l11-8'],
  search: ['M11 19a8 8 0 1 1 0-16 8 8 0 0 1 0 16Zm6-2 4 4'],
  settings: ['M12 15a3 3 0 1 0 0-6 3 3 0 0 0 0 6Zm0-13v3m0 14v3M4.93 4.93l2.12 2.12m9.9 9.9 2.12 2.12M2 12h3m14 0h3M4.93 19.07l2.12-2.12m9.9-9.9 2.12-2.12'],
  shield: ['M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10Zm-3-10 2 2 4-4'],
  staff: ['M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8Zm13 10v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75'],
  stethoscope: ['M6 3v5a4 4 0 0 0 8 0V3M4 3h4m4 0h4m-6 9v3a5 5 0 0 0 10 0v-1m0 0a2 2 0 1 0 0-4 2 2 0 0 0 0 4Z'],
  syringe: ['M18 2 22 6M17 7l-8 8M7 13l4 4M5 19l3-3M14 4l6 6M4 20l6-2 9-9-4-4-9 9-2 6Z'],
  trend: ['M3 17l6-6 4 4 7-7M14 8h6v6'],
  truck: ['M3 6h12v10H3V6Zm12 4h4l2 3v3h-6v-6ZM7 20a2 2 0 1 0 0-4 2 2 0 0 0 0 4Zm10 0a2 2 0 1 0 0-4 2 2 0 0 0 0 4Z'],
  'user-circle': ['M12 22a10 10 0 1 0 0-20 10 10 0 0 0 0 20Zm0-11a3 3 0 1 0 0-6 3 3 0 0 0 0 6Zm-6 8a6 6 0 0 1 12 0'],
  users: ['M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8Zm13 10v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75'],
  video: ['M4 5h11a2 2 0 0 1 2 2v10a2 2 0 0 1-2 2H4V5Zm13 5 4-3v10l-4-3'],
  workflow: ['M6 4v6h6V4H6Zm0 10v6h6v-6H6Zm10-5h2a2 2 0 0 1 2 2v1M12 7h2a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2h-2'],
  x: ['M18 6 6 18M6 6l12 12']
};

@Component({
  selector: 'app-icon',
  standalone: true,
  template: `
    <svg
      aria-hidden="true"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      stroke-width="2"
      stroke-linecap="round"
      stroke-linejoin="round">
      @for (path of paths(); track path) {
        <path [attr.d]="path" />
      }
    </svg>
  `,
  styles: [`
    :host {
      display: inline-grid;
      place-items: center;
      width: 1.15em;
      height: 1.15em;
      line-height: 1;
    }

    svg {
      display: block;
      width: 100%;
      height: 100%;
    }
  `]
})
export class IconComponent {
  readonly name = input<string | null | undefined>('grid');
  readonly paths = computed(() => ICONS[this.name() || ''] ?? ICONS['grid']);
}
