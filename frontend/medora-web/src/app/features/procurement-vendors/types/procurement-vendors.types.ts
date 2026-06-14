export type VendorDto = {
  id: string;
  name: string;
  category: string;
  openPurchaseOrders: number;
  onTimePercentage: number;
};

export type PurchaseOrderDto = {
  id: string;
  purchaseOrderNumber: string;
  itemName: string;
  vendorName: string;
  quantity: number;
  totalAmount: number;
  status: string;
  eta?: string | null;
};

export type CreatePurchaseOrderRequest = {
  vendorId: string;
  itemId: string;
  quantity: number;
  expectedDeliveryDate?: string | null;
};
