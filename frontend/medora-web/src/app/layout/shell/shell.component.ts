import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { combineLatest, filter, map, startWith } from 'rxjs';

import { AppContextService } from '../../core/app-context.service';
import { ScreenDataService } from '../../core/screen-data.service';
import { IconComponent } from '../../shared/icon/icon.component';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [AsyncPipe, IconComponent, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss'
})
export class ShellComponent {
  private readonly screenData = inject(ScreenDataService);
  private readonly router = inject(Router);
  readonly appContext = inject(AppContextService);

  private readonly currentPath$ = this.router.events.pipe(
    filter((event): event is NavigationEnd => event instanceof NavigationEnd),
    startWith(null),
    map(() => this.router.url.split('?')[0])
  );

  readonly viewModel$ = combineLatest([this.screenData.getCatalog(), this.currentPath$, this.appContext.loadApplication()]).pipe(
    map(([catalog, currentPath, application]) => ({
      catalog,
      application,
      currentScreen: catalog.screens.find((screen) => screen.route === currentPath) ?? catalog.screens[0],
      session: this.appContext.session()
    }))
  );

  initials(label?: string | null): string {
    return (label || '')
      .split(/[\s|-]+/)
      .filter(Boolean)
      .slice(0, 2)
      .map((word) => word.charAt(0))
      .join('')
      .toUpperCase();
  }

  signOut(): void {
    this.appContext.signOut();
    this.router.navigateByUrl('/login');
  }
}
