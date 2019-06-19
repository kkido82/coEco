export interface Item {
  itemId: number;
  itemName: string;
  iconName: string;
}

export interface OrderOverview {
  id: number;
  itemId: number;
  itemName: string;
  fromUnit: string;
  fromUnitId: number;
  toUnitId: number;
  toUnit: string;
  statusId: number;
  statusName: string;
  orderDate: string;
}

export enum OrderStatusId {
  New = 1,
  Approved = 2,
  Confirmed = 3,
  Active = 4,
  Completed = 5,
  CanceledByRequestingUnit = 6,
  CanceledByLendingUnit = 7
}

export interface OrderDetails extends OrderOverview {
  cost: number;
  distance: number;
  itemDescription: string;
  contactPersonName: string;
  contactPersonPhone: string;
  remarks: string;
  actions: number[];
}

export interface ItemSearchResult {
  itemUnitId: number;
  itemId: number;
  unitName: string;
  distance: number;
  rating: number;
  description: string;
}

export interface UserPermissions {
  canOpenOrder: boolean;
  canUpdateInventory: boolean;
  canConfirmOrder: boolean;
}

export interface User {
  firstName: string;
  lastName: string;
  tz: string;
  unitId: number;
  requestToken: string;
  permissions: UserPermissions;
}

export interface Balance {
  currentBalance: number;
  originalBalance: number;
  incomingBalance: number;
  outgoingBalance: number;
}

export interface Message {
  id: number;
  orderId: number;
  date: string;
  title: string;
}

export interface CreateOrder {
  itemId: number;
  fromUnitId: number;
  remarks: string;
}

export interface UpdateOrderStatus {
  id: number;
  orderStatusId: OrderStatusId;
}

export interface ReportProblemRequest {
  orderId: number;
  description: string;
}

export interface UnitItemBase {
  itemId: number;
  qty: number;
  description: string;
}

export interface UnitItem extends UnitItemBase {
  itemName: string;
}
