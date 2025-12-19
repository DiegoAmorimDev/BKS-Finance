import { useEffect, useState } from 'react';
import { apiService } from '../services/api';
import { TransactionType, Purpose } from '../types';
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
    try {
      const [t, p, c] = await Promise.all([
        apiService.getTransactions(),
        apiService.getPersons(),
        apiService.getCategories()
      ]);
      
      console.log("Categorias carregadas do banco:", c); // Log para debug
      setTransactions(t);
      setPeople(p);
      setCategories(c);
    } catch (e) {
      console.error("Erro ao carregar dados para transações:", e);
    }
  };

  useEffect(() => { loadData(); }, []);

  // REGRA DE NEGÓCIO: Menor de Idade
  const selectedPerson = people.find(p => p.id === formData.personId);
  const isMinor = !!(selectedPerson && selectedPerson.age < 18);

  useEffect(() => {
    if (isMinor) {
      setFormData(prev => ({ ...prev, type: TransactionType.Expense as TransactionType }));
    }
  }, [isMinor]);

  // REGRA DE NEGÓCIO: Filtro de Categorias por Finalidade
  // Usamos Number() para evitar erro de comparação entre string e number
  const filteredCategories = categories.filter(c => {
    const categoryPurpose = Number(c.purpose);
    const selectedType = Number(formData.type);
    
    return categoryPurpose === Purpose.Both || categoryPurpose === selectedType;
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.personId || !formData.categoryId || formData.value <= 0) {
      alert("Selecione a pessoa, a categoria e um valor válido.");
      return;
    }
    
    try {
      await apiService.createTransaction(formData);
      setFormData({ ...formData, description: '', value: 0, categoryId: '' });
      loadData();
    } catch (err) {
      console.error("Erro ao salvar transação", err);
    }
  };

  return (
    <div className="space-y-8">
      <h2 className="text-3xl font-black text-slate-900">Transações</h2>

      <form onSubmit={handleSubmit} className="bg-white p-8 rounded-3xl border border-slate-200 shadow-sm grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="md:col-span-2">
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Descrição</label>
          <input 
            className="w-full border-slate-200 border p-3 rounded-xl outline-none focus:ring-2 focus:ring-blue-500"
            value={formData.description} 
            onChange={e => setFormData({...formData, description: e.target.value})} 
            required
          />
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Pessoa</label>
          <select 
            className="w-full border-slate-200 border p-3 rounded-xl outline-none"
            value={formData.personId} 
            onChange={e => setFormData({...formData, personId: e.target.value})} 
            required
          >
            <option value="">Selecionar Pessoa...</option>
            {people.map(p => <option key={p.id} value={p.id}>{p.name} ({p.age} anos)</option>)}
          </select>
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Tipo</label>
          <select 
            className="w-full border-slate-200 border p-3 rounded-xl outline-none disabled:bg-slate-50 disabled:text-slate-400"
            value={formData.type} 
            onChange={e => setFormData({...formData, type: Number(e.target.value) as TransactionType})}
            disabled={isMinor}
          >
            <option value={TransactionType.Expense}>Despesa</option>
            {!isMinor && <option value={TransactionType.Income}>Receita</option>}
          </select>
          {isMinor && <p className="text-[10px] text-rose-500 mt-1 font-bold italic">* Menores apenas despesas.</p>}
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Valor (€)</label>
          <input 
            type="number" step="0.01" 
            className="w-full border-slate-200 border p-3 rounded-xl outline-none"
            value={formData.value || ''} 
            onChange={e => setFormData({...formData, value: Number(e.target.value)})} 
            required
          />
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Categoria</label>
          <select 
            className="w-full border-slate-200 border p-3 rounded-xl outline-none"
            value={formData.categoryId} 
            onChange={e => setFormData({...formData, categoryId: e.target.value})} 
            required
          >
            <option value="">Selecionar Categoria...</option>
            {filteredCategories.length > 0 ? (
              filteredCategories.map(c => (
                <option key={c.id} value={c.id}>{c.description}</option>
              ))
            ) : (
              <option disabled>Nenhuma categoria compatível encontrada</option>
            )}
          </select>
        </div>

        <button className="md:col-span-2 bg-blue-600 text-white p-4 rounded-2xl font-black text-lg hover:bg-blue-700 shadow-xl shadow-blue-100 transition-all">
          Registar Transação
        </button>
      </form>

      {/* Histórico Simples */}
      <div className="bg-white rounded-3xl border border-slate-200 shadow-sm overflow-hidden">
        <table className="w-full text-left">
          <thead>
            <tr className="bg-slate-50 text-slate-500 text-xs uppercase font-bold tracking-widest border-b border-slate-100">
              <th className="p-5">Descrição</th>
              <th className="p-5 text-right">Valor</th>
              <th className="p-5 text-center">Tipo</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-50">
            {transactions.map(t => (
              <tr key={t.id}>
                <td className="p-5 font-bold text-slate-800">{t.description}</td>
                <td className={`p-5 text-right font-black text-lg ${t.type === TransactionType.Income ? 'text-emerald-600' : 'text-rose-600'}`}>
                  € {t.value.toFixed(2)}
                </td>
                <td className="p-5 text-center">
                  <span className={`px-3 py-1 rounded-full text-[10px] font-black uppercase ${t.type === TransactionType.Income ? 'bg-emerald-100 text-emerald-700' : 'bg-rose-100 text-rose-700'}`}>
                    {t.type === TransactionType.Income ? 'Receita' : 'Despesa'}
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