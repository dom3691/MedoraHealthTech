import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { combineLatest, filter, map, startWith } from 'rxjs';

import { AppContextService } from '../../../services/app-context.service';
import { ScreenDataService } from '../../../services/screen-data.service';
import { AppRole, navigationConfig } from '../../../config/navigation';
import { NavigationGroup, NavigationItem, ScreenCatalogResponse } from '../../../types/screen';
import { IconComponent } from '../../icons/icon.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [AsyncPipe, IconComponent, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app-layout.component.html',
  styleUrl: './app-layout.component.scss'
})
export class AppLayoutComponent {
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
      catalog: {
        ...catalog,
        navigation: this.buildNavigation(catalog)
      },
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

  private buildNavigation(catalog: ScreenCatalogResponse): NavigationGroup[] {
    const roleCodes = this.appContext.session()?.user.roleCodes as AppRole[] | undefined;
    const activeRoles = roleCodes?.length ? roleCodes : ['OperationsAdmin'];
    const configuredItems = navigationConfig
      .filter((item) => activeRoles.includes('SuperAdmin') || item.allowedRoles.some((role) => activeRoles.includes(role)))
      .map<NavigationItem & { section: string }>((item) => {
        const screen = catalog.screens.find((candidate) => candidate.route === item.path);
        return {
          key: screen?.key ?? item.path.replace('/', ''),
          label: item.label,
          route: item.path,
          icon: item.icon,
          section: item.section
        };
      });

    return configuredItems.reduce<NavigationGroup[]>((groups, item) => {
      let group = groups.find((candidate) => candidate.name === item.section);
      if (!group) {
        group = { name: item.section, items: [] };
        groups.push(group);
      }

      group.items.push({ key: item.key, label: item.label, route: item.route, icon: item.icon });
      return groups;
    }, []);
  }
}

