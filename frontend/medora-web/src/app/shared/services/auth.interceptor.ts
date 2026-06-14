import { HttpInterceptorFn } from '@angular/common/http';

type StoredSession = {
  token?: string | null;
  tenant?: {
    id?: string | null;
    code?: string | null;
  } | null;
};

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const raw = localStorage.getItem('medora.session');
  if (!raw) {
    return next(request);
  }

  try {
    const session = JSON.parse(raw) as StoredSession;
    if (!session.token) {
      return next(request);
    }

    const headers: Record<string, string> = {
      Authorization: `Bearer ${session.token}`
    };

    if (session.tenant?.id) {
      headers['X-Tenant-Id'] = session.tenant.id;
    }

    if (session.tenant?.code) {
      headers['X-Tenant-Code'] = session.tenant.code;
    }

    return next(request.clone({ setHeaders: headers }));
  } catch {
    localStorage.removeItem('medora.session');
    return next(request);
  }
};
