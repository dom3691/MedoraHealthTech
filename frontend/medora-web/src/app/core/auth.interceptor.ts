import { HttpInterceptorFn } from '@angular/common/http';

type StoredSession = {
  token?: string | null;
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

    return next(request.clone({
      setHeaders: {
        Authorization: `Bearer ${session.token}`
      }
    }));
  } catch {
    localStorage.removeItem('medora.session');
    return next(request);
  }
};
