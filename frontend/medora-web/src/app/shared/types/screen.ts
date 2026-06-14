export type ScreenCatalogResponse = {
  navigation: NavigationGroup[];
  screens: ScreenDefinition[];
};

export type NavigationGroup = {
  name: string;
  items: NavigationItem[];
};

export type NavigationItem = {
  key: string;
  label: string;
  route: string;
  icon: string;
};

export type ScreenDefinition = {
  key: string;
  route: string;
  title: string;
  subtitle: string;
  group: string;
  metrics: MetricCard[];
  actions: ActionButton[];
  sections: ScreenSection[];
};

export type MetricCard = {
  label: string;
  value: string;
  hint?: string | null;
  tone: string;
  icon: string;
};

export type ActionButton = {
  label: string;
  tone: string;
  icon: string;
  route?: string | null;
};

export type ScreenSection = {
  title: string;
  kind: string;
  columns?: TableColumn[] | null;
  rows?: Record<string, string>[] | null;
  items?: CardItem[] | null;
  columnsBoard?: BoardColumn[] | null;
  steps?: WorkflowStep[] | null;
  tabs?: string[] | null;
};

export type TableColumn = {
  key: string;
  label: string;
};

export type CardItem = {
  title: string;
  subtitle?: string | null;
  value?: string | null;
  meta?: string | null;
  status?: string | null;
  tone?: string | null;
  progress?: number | null;
  detail?: string | null;
  action?: ActionButton | null;
};

export type BoardColumn = {
  title: string;
  subtitle: string;
  tone: string;
  cards: CardItem[];
};

export type WorkflowStep = {
  number: string;
  title: string;
  subtitle: string;
  icon: string;
};
