export type InventoryItemDto = {
  id: string;
  name: string;
  category: string;
  quantityOnHand: number;
  reorderLevel: number;
  unit: string;
  status: string;
  expiryDate?: string | null;
};

export type CreateInventoryItemRequest = {
  name: string;
  category: string;
  quantityOnHand: number;
  reorderLevel: number;
  unit: string;
  expiryDate?: string | null;
};

export type ReorderInventoryItemRequest = {
  itemId: string;
  quantity: number;
  vendorId?: string | null;
};
