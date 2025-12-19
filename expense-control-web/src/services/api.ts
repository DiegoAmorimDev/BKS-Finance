import axios from 'axios';
import type { Transaction, TransactionDTO, Category, Person } from '../types';

const api = axios.create({
  baseURL: 'http://localhost:5034/api',
});

export const apiService = {
  // Transações
  getTransactions: () => api.get<Transaction[]>('/transactions').then(r => r.data),
  createTransaction: (data: TransactionDTO) => api.post('/transactions', data),
  
  // Tabelas auxiliares
  getCategories: () => api.get<Category[]>('/categories').then(r => r.data),
  getPeople: () => api.get<Person[]>('/people').then(r => r.data),
};