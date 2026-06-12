namespace Medora.Application.Screens;

public interface IScreenCatalog
{
    Task<ScreenCatalogResponse> GetCatalogAsync(string? tenantCode = null, CancellationToken cancellationToken = default);

    Task<ScreenDefinition?> GetScreenAsync(string key, string? tenantCode = null, CancellationToken cancellationToken = default);
}
