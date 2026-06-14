import { AsyncPipe, NgClass } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { map, switchMap } from 'rxjs';

import { AppContextService, AssignableRole, TenantOnboardingResult, UserOnboardingResult } from '../../services/app-context.service';
import { CardItem, ScreenSection } from '../../types/screen';
import { ScreenDataService } from '../../services/screen-data.service';
import { IconComponent } from '../icons/icon.component';

@Component({
  selector: 'app-module-screen',
  standalone: true,
  imports: [AsyncPipe, FormsModule, IconComponent, NgClass, RouterLink],
  templateUrl: './module-screen.component.html',
  styleUrl: './module-screen.component.scss'
})
export class ModuleScreenComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly screenData = inject(ScreenDataService);
  private readonly appContext = inject(AppContextService);

  readonly tenantForm = {
    name: '',
    code: '',
    location: '',
    licensedBeds: 100,
    plan: 'Enterprise',
    adminFullName: '',
    adminEmail: '',
    includeDemoOperationalData: false
  };

  readonly userForm = {
    fullName: '',
    email: '',
    employeeNumber: '',
    department: '',
    jobTitle: '',
    employmentType: 'Employee',
    startDate: '',
    roleCodes: [] as string[]
  };

  onboardingBusy = false;
  onboardingError: string | null = null;
  onboardingResult: TenantOnboardingResult | null = null;
  userOnboardingBusy = false;
  userOnboardingError: string | null = null;
  userOnboardingResult: UserOnboardingResult | null = null;
  assignableRoles: AssignableRole[] = [];

  readonly screen$ = this.route.url.pipe(
    map((segments) => this.route.snapshot.data['screenKey'] ?? segments.at(0)?.path ?? 'dashboard'),
    switchMap((key) => this.screenData.getScreen(key))
  );

  ngOnInit(): void {
    if (this.canOnboardUsers()) {
      this.appContext.loadAssignableRoles().subscribe({
        next: (roles) => this.assignableRoles = roles,
        error: () => this.assignableRoles = []
      });
    }
  }

  isMedoraSuperAdmin(): boolean {
    return this.appContext.hasRole('MSA');
  }

  canOnboardUsers(): boolean {
    return this.appContext.hasRole('SA', 'HR');
  }

  isRoleSelected(roleCode: string): boolean {
    return this.userForm.roleCodes.includes(roleCode);
  }

  toggleUserRole(roleCode: string, checked: boolean): void {
    if (checked && !this.userForm.roleCodes.includes(roleCode)) {
      this.userForm.roleCodes = [...this.userForm.roleCodes, roleCode];
      return;
    }

    if (!checked) {
      this.userForm.roleCodes = this.userForm.roleCodes.filter((code) => code !== roleCode);
    }
  }

  toneClass(tone?: string | null): string {
    return `tone-${(tone || 'neutral').toLowerCase().replace(/\s+/g, '-')}`;
  }

  statusClass(value?: string | null): string {
    const normalized = (value || '').toLowerCase();
    if (/(critical|denied|overdue|urgent|resuscitation|emergent)/.test(normalized)) {
      return 'status-danger';
    }
    if (/(warning|pending|processing|due|waiting|low|cleaning|in progress|in theatre|acquired|under review)/.test(normalized)) {
      return 'status-warning';
    }
    if (/(success|done|completed|approved|paid|active|available|compliant|resulted|reported|received|in stock|on)/.test(normalized)) {
      return 'status-success';
    }
    if (/(confirmed|checked|scheduled|submitted|reserved|collected)/.test(normalized)) {
      return 'status-info';
    }
    return 'status-neutral';
  }

  initials(value: string): string {
    return value
      .split(/[\s|-]+/)
      .filter(Boolean)
      .slice(0, 2)
      .map((word) => word.charAt(0))
      .join('')
      .toUpperCase();
  }

  splitName(value: string): { name: string; role?: string } {
    const [name, role] = value.split('|').map((part) => part.trim());
    return { name, role };
  }

  isWide(section: ScreenSection): boolean {
    return ['table', 'feed', 'board', 'steps'].includes(section.kind);
  }

  section(screenSections: ScreenSection[], title: string): ScreenSection | undefined {
    return screenSections.find((section) => section.title === title);
  }

  criticalRows(section?: ScreenSection): Record<string, string>[] {
    return (section?.rows ?? [])
      .filter((row) => this.statusClass(row['status']) === 'status-danger')
      .slice(0, 2);
  }

  appointmentRows(section?: ScreenSection): Record<string, string>[] {
    return (section?.rows ?? []).slice(0, 5);
  }

  flowColumnCountTone(columnTitle: string): string {
    const normalized = columnTitle.toLowerCase();
    if (normalized.includes('doctor')) {
      return 'tone-warning';
    }
    return 'tone-success';
  }

  urgentRows(section?: ScreenSection): Record<string, string>[] {
    return (section?.rows ?? [])
      .filter((row) => this.statusClass(row['priority'] || row['status']) !== 'status-neutral')
      .slice(0, 4);
  }

  dashboardTrendPath(): string {
    return 'M20 164 C80 176 116 178 150 174 C186 170 198 130 236 110 C288 82 330 36 394 44 C462 52 494 106 560 92 C612 82 642 54 700 30';
  }

  dashboardTrendArea(): string {
    return `${this.dashboardTrendPath()} L700 198 L20 198 Z`;
  }

  progress(item: CardItem): number {
    const parsed = Number.parseInt(item.status || '0', 10) || 0;
    return Math.max(0, Math.min(100, item.progress ?? parsed));
  }

  onboardTenant(): void {
    this.onboardingBusy = true;
    this.onboardingError = null;
    this.onboardingResult = null;

    this.appContext.onboardTenant({
      ...this.tenantForm,
      licensedBeds: Number(this.tenantForm.licensedBeds)
    }).subscribe({
      next: (result) => {
        this.onboardingResult = result;
        this.onboardingBusy = false;
        this.tenantForm.name = '';
        this.tenantForm.code = '';
        this.tenantForm.location = '';
        this.tenantForm.licensedBeds = 100;
        this.tenantForm.plan = 'Enterprise';
        this.tenantForm.adminFullName = '';
        this.tenantForm.adminEmail = '';
        this.tenantForm.includeDemoOperationalData = false;
      },
      error: (error) => {
        this.onboardingError = error?.error?.message ?? 'Unable to onboard tenant.';
        this.onboardingBusy = false;
      }
    });
  }

  onboardUser(): void {
    this.userOnboardingBusy = true;
    this.userOnboardingError = null;
    this.userOnboardingResult = null;

    this.appContext.onboardUser({
      ...this.userForm,
      employeeNumber: this.userForm.employeeNumber || null,
      department: this.userForm.department || null,
      jobTitle: this.userForm.jobTitle || null,
      startDate: this.userForm.startDate || null
    }).subscribe({
      next: (result) => {
        this.userOnboardingResult = result;
        this.userOnboardingBusy = false;
        this.userForm.fullName = '';
        this.userForm.email = '';
        this.userForm.employeeNumber = '';
        this.userForm.department = '';
        this.userForm.jobTitle = '';
        this.userForm.employmentType = 'Employee';
        this.userForm.startDate = '';
        this.userForm.roleCodes = [];
      },
      error: (error) => {
        this.userOnboardingError = error?.error?.message ?? 'Unable to onboard employee.';
        this.userOnboardingBusy = false;
      }
    });
  }
}

