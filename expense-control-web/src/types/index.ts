// Padrão que substitui Enums para evitar o erro ts(1294)
export const Purpose = {
  Expense: 0,
  Income: 1,
  Both: 2,
} as const;

export type Purpose = typeof Purpose[keyof typeof Purpose];

export const TransactionType = {
  Expense: 0,
  Income: 1,
} as const;

export type TransactionType = typeof TransactionType[keyof typeof TransactionType];

export interface Category {
  id: string;
  description: string;
  purpose: Purpose;
}

export interface Person {
  id: string;
  name: string;
  age: number;
}

export interface Transaction {
  id: string;
  description: string;
  value: number;
  type: TransactionType;
  categoryId: string;
  personId: string;
}

export interface ReportTotals {
  peopleTotals: {
    personName: string;
    totalIncome: number;
    totalExpense: number;
    balance: number;
  }[];
  grandTotalIncome: number;
  grandTotalExpense: number;
  grandBalance: number;
}