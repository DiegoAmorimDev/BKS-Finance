import { useState, useEffect } from 'react';
import { apiService } from '../services/api';
import type { Category, Person } from '../types';

export function TransactionForm({ onSuccess }: { onSuccess: () => void }) {
  const [categories, setCategories] = useState<Category[]>([]);
  const [people, setPeople] = useState<Person[]>([]);
  
  // Estado simplificado para o formulário
  const [formData, setFormData] = useState({
    description: '',
    amount: 0,
    type: 'expense' as 'income' | 'expense',
    categoryId: '',
    personId: '',
    date: new Date().toISOString().split('T')[0]
  });

  useEffect(() => {
    // Carrega dados para os selects
    Promise.all([apiService.getCategories(), apiService.getPeople()])
      .then(([cats, peps]) => {
        setCategories(cats);
        setPeople(peps);
      });
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await apiService.createTransaction(formData);
      onSuccess(); // Recarrega a lista no Dashboard
      setFormData({ ...formData, description: '', amount: 0 }); // Limpa campos básicos
    } catch (err) {
      alert("Erro ao salvar transação");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white p-6 rounded-lg shadow mb-6 grid grid-cols-1 md:grid-cols-3 gap-4 border-t-4 border-blue-500">
      <input 
        type="text" placeholder="Descrição" required
        className="border p-2 rounded"
        value={formData.description}
        onChange={e => setFormData({...formData, description: e.target.value})}
      />
      <input 
        type="number" placeholder="Valor" required
        className="border p-2 rounded"
        value={formData.amount}
        onChange={e => setFormData({...formData, amount: Number(e.target.value)})}
      />
      <select 
        className="border p-2 rounded"
        value={formData.type}
        onChange={e => setFormData({...formData, type: e.target.value as any})}
      >
        <option value="expense">Despesa</option>
        <option value="income">Receita</option>
      </select>

      <select 
        required className="border p-2 rounded"
        value={formData.categoryId}
        onChange={e => setFormData({...formData, categoryId: e.target.value})}
      >
        <option value="">Selecione Categoria</option>
        {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
      </select>

      <select 
        required className="border p-2 rounded"
        value={formData.personId}
        onChange={e => setFormData({...formData, personId: e.target.value})}
      >
        <option value="">Selecione Pessoa</option>
        {people.map(p => <option key={p.id} value={p.id}>{p.name}</option>)}
      </select>

      <button type="submit" className="bg-blue-600 text-white font-bold py-2 rounded hover:bg-blue-700 transition-colors">
        Salvar Transação
      </button>
    </form>
  );
}