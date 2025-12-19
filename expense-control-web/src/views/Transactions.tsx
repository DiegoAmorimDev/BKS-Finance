import { useEffect, useState } from 'react';
import { apiService } from '../services/api';
import { TransactionType, Purpose } from '../types';
import { formatCurrency } from '../utils/format';
import type { Transaction, Person, Category } from '../types';

export function Transactions() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [people, setPeople] = useState<Person[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);

  const [formData, setFormData] = useState({
    description: '',
    value: 0,
    type: TransactionType.Expense as TransactionType,
    personId: '',
    categoryId: ''
  });

  const loadData = async () => {
    const [t, p, c] = await Promise.all([
      apiService.getTransactions(),
      apiService.getPersons(),
      apiService.getCategories()
    ]);
    setTransactions(t);
    setPeople(p);
    setCategories(c);
  };

  useEffect(() => { loadData(); }, []);

  const selectedPerson = people.find(p => p.id === formData.personId);
  const isMinor = !!(selectedPerson && selectedPerson.age < 18);

  useEffect(() => {
    if (isMinor) setFormData(prev => ({ ...prev, type: TransactionType.Expense as TransactionType }));
  }, [isMinor]);

  const filteredCategories = categories.filter(c => 
    Number(c.purpose) === 2 || Number(c.purpose) === Number(formData.type)
  );

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.personId || !formData.categoryId || formData.value <= 0) return;
    await apiService.createTransaction(formData);
    setFormData({ ...formData, description: '', value: 0 });
    loadData();
  };

  return (
    <div className="space-y-8">
      <h2 className="text-3xl font-black text-slate-900">Transações</h2>

      <form onSubmit={handleSubmit} className="bg-white p-8 rounded-3xl border border-slate-200 shadow-sm grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="md:col-span-2">
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Descrição</label>
          <input className="w-full border p-3 rounded-xl outline-none focus:ring-2 focus:ring-blue-500" value={formData.description} onChange={e => setFormData({...formData, description: e.target.value})} required />
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Pessoa</label>
          <select className="w-full border p-3 rounded-xl outline-none" value={formData.personId} onChange={e => setFormData({...formData, personId: e.target.value})} required>
            <option value="">Selecionar...</option>
            {people.map(p => <option key={p.id} value={p.id}>{p.name} ({p.age} anos)</option>)}
          </select>
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Tipo</label>
          <select className="w-full border p-3 rounded-xl outline-none disabled:bg-slate-50" value={formData.type} onChange={e => setFormData({...formData, type: Number(e.target.value) as TransactionType})} disabled={isMinor}>
            <option value={TransactionType.Expense}>Despesa</option>
            <option value={TransactionType.Income}>Receita</option>
          </select>
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Valor (R$)</label>
          <input type="number" step="0.01" className="w-full border p-3 rounded-xl outline-none" value={formData.value || ''} onChange={e => setFormData({...formData, value: Number(e.target.value)})} required />
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Categoria</label>
          <select className="w-full border p-3 rounded-xl outline-none" value={formData.categoryId} onChange={e => setFormData({...formData, categoryId: e.target.value})} required>
            <option value="">Selecionar...</option>
            {filteredCategories.map(c => <option key={c.id} value={c.id}>{c.description}</option>)}
          </select>
        </div>

        <button className="md:col-span-2 bg-blue-600 text-white p-4 rounded-2xl font-black text-lg hover:bg-blue-700 shadow-xl shadow-blue-100 transition-all">Registar Transação</button>
      </form>

      <div className="bg-white rounded-3xl border border-slate-200 overflow-hidden shadow-sm">
        <table className="w-full text-left">
          <thead className="bg-slate-50 text-[10px] font-black text-slate-400 uppercase tracking-widest border-b">
            <tr>
              <th className="p-5">Descrição</th>
              <th className="p-5 text-right">Valor</th>
              <th className="p-5 text-center">Tipo</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-50">
            {transactions.map(t => (
              <tr key={t.id} className="hover:bg-slate-50/50 transition-colors">
                <td className="p-5 font-bold text-slate-800">{t.description}</td>
                <td className={`p-5 text-right font-black text-lg ${Number(t.type) === 1 ? 'text-emerald-600' : 'text-rose-600'}`}>
                  {formatCurrency(t.value)}
                </td>
                <td className="p-5 text-center">
                  <span className={`px-3 py-1 rounded-full text-[10px] font-black uppercase ${Number(t.type) === 1 ? 'bg-emerald-100 text-emerald-700' : 'bg-rose-100 text-rose-700'}`}>
                    {Number(t.type) === 1 ? 'Receita' : 'Despesa'}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}