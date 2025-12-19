export interface Category {
  id: string;
  name: string;
}

export interface Person {
  id: string;
  name: string;
}

export interface Transaction {
  id: string;
  description: string;
  amount: number;
  date: string;
  type: 'income' | 'expense';
  categoryId: string;
  personId: string;
}

export interface TransactionDTO {
  description: string;
  amount: number;
  date: string;
  type: 'income' | 'expense';
  categoryId: string;
  personId: string;
}