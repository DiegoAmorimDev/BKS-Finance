import axios from 'axios';
import type { Transaction, Category, Person, ReportTotals } from '../types';

const api = axios.create({
  baseURL: 'http://localhost:5034/api/v1',
});

// Interceptor para tratar erros de validação do FluentValidation (.NET)
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 400 && error.response.data.errors) {
      const messages = Object.values(error.response.data.errors).flat();
      alert(`Erro de Validação:\n${messages.join('\n')}`);
    } 
    return Promise.reject(error);
  }
);

export const apiService = {
  // --- PESSOAS ---
  getPersons: () => api.get<Person[]>('/persons').then(r => r.data),
  
  createPerson: (data: { name: string; age: number }) => 
    api.post('/persons', data).then(r => r.data),
  
  deletePerson: (id: string) => 
    api.delete(`/persons/${id}`).then(r => r.data),

  // --- CATEGORIAS ---
  getCategories: () => api.get<Category[]>('/categories').then(r => r.data),
  
  createCategory: (data: { description: string; purpose: number }) => 
    api.post('/categories', data).then(r => r.data),

  // --- TRANSAÇÕES ---
  getTransactions: () => api.get<Transaction[]>('/transactions').then(r => r.data),
  
  createTransaction: (data: Omit<Transaction, 'id'>) => 
    api.post('/transactions', data).then(r => r.data),

  // --- RELATÓRIOS (Com Normalização de PascalCase para camelCase) ---
getReports: () => api.get('/reports/totals').then(r => {
    const d = r.data;
    
    // Mapeamos o que vem do seu Backend para o que o Dashboard espera
    return {
      // O seu backend envia 'personTotals', convertemos para 'peopleTotals'
      peopleTotals: (d.personTotals || []).map((p: any) => ({
        personName: p.name, // O seu backend envia 'name'
        totalIncome: p.totalIncome,
        totalExpense: p.totalExpense,
        balance: p.balance
      })),
      // O seu backend envia 'generalTotalIncome', mapeamos para 'grandTotalIncome'
      grandTotalIncome: d.generalTotalIncome ?? 0,
      grandTotalExpense: d.generalTotalExpense ?? 0,
      grandBalance: d.generalBalance ?? 0
    } as ReportTotals;
  }),
};