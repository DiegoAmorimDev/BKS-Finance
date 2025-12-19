import { useEffect, useState } from 'react';
import { apiService } from '../services/api';
import { Purpose } from '../types';
import type { Category } from '../types';

export function Categories() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [description, setDescription] = useState('');
  const [purpose, setPurpose] = useState<Purpose>(Purpose.Both);

  const loadCategories = async () => {
    try {
      const data = await apiService.getCategories();
      setCategories(data);
    } catch (e) { console.error(e); }
  };

  useEffect(() => { loadCategories(); }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!description) return;
    await apiService.createCategory({ description, purpose });
    setDescription('');
    setPurpose(Purpose.Both);
    loadCategories();
  };

  const getPurposeLabel = (p: Purpose) => {
    switch(p) {
      case Purpose.Expense: return { label: 'Despesa', color: 'bg-rose-100 text-rose-700' };
      case Purpose.Income: return { label: 'Receita', color: 'bg-emerald-100 text-emerald-700' };
      default: return { label: 'Ambas', color: 'bg-slate-100 text-slate-700' };
    }
  };

  return (
    <div className="space-y-8">
      <h2 className="text-3xl font-black text-slate-900">Categorias</h2>

      <form onSubmit={handleSubmit} className="bg-white p-6 rounded-3xl border border-slate-200 shadow-sm flex flex-wrap gap-4 items-end">
        <div className="flex-1 min-w-[250px]">
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2 ml-1">Descrição</label>
          <input 
            className="w-full border-slate-200 border p-3 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none transition-all"
            value={description} onChange={e => setDescription(e.target.value)} placeholder="Ex: Alimentação, Lazer..." required 
          />
        </div>
        <div className="w-48">
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2 ml-1">Finalidade</label>
          <select 
            className="w-full border-slate-200 border p-3 rounded-xl focus:ring-2 focus:ring-blue-500 outline-none"
            value={purpose} onChange={e => setPurpose(Number(e.target.value) as Purpose)}
          >
            <option value={Purpose.Expense}>Despesa</option>
            <option value={Purpose.Income}>Receita</option>
            <option value={Purpose.Both}>Ambas</option>
          </select>
        </div>
        <button className="bg-slate-900 text-white px-8 py-3.5 rounded-xl font-bold hover:bg-slate-800 transition-all">
          Criar
        </button>
      </form>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {categories.map(c => {
          const badge = getPurposeLabel(c.purpose);
          return (
            <div key={c.id} className="bg-white p-5 rounded-2xl border border-slate-200 hover:border-blue-300 transition-all">
              <p className="font-bold text-slate-800 mb-2">{c.description}</p>
              <span className={`text-[10px] uppercase font-black px-2 py-1 rounded-md ${badge.color}`}>
                {badge.label}
              </span>
            </div>
          );
        })}
      </div>
    </div>
  );
}