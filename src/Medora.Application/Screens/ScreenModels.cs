namespace Medora.Application.Screens;

public sealed record ScreenCatalogResponse(
    IReadOnlyList<NavigationGroup> Navigation,
    IReadOnlyList<ScreenDefinition> Screens);

public sealed record NavigationGroup(string Name, IReadOnlyList<NavigationItem> Items);

public sealed record NavigationItem(string Key, string Label, string Route, string Icon);

public sealed record ScreenDefinition(
    string Key,
    string Route,
    string Title,
    string Subtitle,
    string Group,
    IReadOnlyList<MetricCard> Metrics,
    IReadOnlyList<ActionButton> Actions,
    IReadOnlyList<ScreenSection> Sections);

public sealed record MetricCard(string Label, string Value, string? Hint, string Tone, string Icon);

public sealed record ActionButton(string Label, string Tone, string Icon, string? Route = null);

public sealed record ScreenSection(
    string Title,
    string Kind,
    IReadOnlyList<TableColumn>? Columns = null,
    IReadOnlyList<IReadOnlyDictionary<string, string>>? Rows = null,
    IReadOnlyList<CardItem>? Items = null,
    IReadOnlyList<BoardColumn>? ColumnsBoard = null,
    IReadOnlyList<WorkflowStep>? Steps = null,
    IReadOnlyList<string>? Tabs = null);

public sealed record TableColumn(string Key, string Label);

public sealed record CardItem(
    string Title,
    string? Subtitle = null,
    string? Value = null,
    string? Meta = null,
    string? Status = null,
    string? Tone = null,
    int? Progress = null,
    string? Detail = null,
    ActionButton? Action = null);

public sealed record BoardColumn(string Title, string Subtitle, string Tone, IReadOnlyList<CardItem> Cards);

public sealed record WorkflowStep(string Number, string Title, string Subtitle, string Icon);
