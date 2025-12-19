import { useEffect, useState, useCallback } from 'react';
import { apiService } from '../services/api';
import type { ReportTotals, Transaction, Category } from '../types';
import { SummaryCard } from '../components/SummaryCard';
import { formatCurrency } from '../utils/format';
import { PieChart, Pie, Cell, ResponsiveContainer, Tooltip, Legend } from 'recharts';

export function Dashboard() {
  const [reports, setReports] = useState<ReportTotals | null>(null);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);

  const loadData = useCallback(async () => {
    try {
      setLoading(true);
      const [rData, tData, cData] = await Promise.all([
        apiService.getReports().catch(() => null),
        apiService.getTransactions().catch(() => []),
        apiService.getCategories().catch(() => [])
      ]);
      setReports(rData);
      setTransactions(Array.isArray(tData) ? tData : []);
      setCategories(Array.isArray(cData) ? cData : []);
    } catch (e) {
      console.error(e);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { loadData(); }, [loadData]);

  const chartData = categories.map(cat => {
    const total = transactions
      .filter(t => t.categoryId === cat.id && Number(t.type) === 0)
      .reduce((sum, t) => sum + t.value, 0);
    return { name: cat.description, value: total };
  }).filter(item => item.value > 0);

  const COLORS = ['#6366f1', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6'];

  if (loading) return <div className="p-20 text-center font-bold text-slate-400 animate-pulse">Sincronizando...</div>;

  return (
    <div className="space-y-8 pb-10">
      <header>
        <h2 className="text-4xl font-black text-slate-900 tracking-tight">Dashboard</h2>
        <p className="text-slate-500 font-medium text-sm">Controle Financeiro Residencial BKS</p>
      </header>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <SummaryCard title="Receitas" amount={reports?.grandTotalIncome ?? 0} variant="income" />
        <SummaryCard title="Despesas" amount={reports?.grandTotalExpense ?? 0} variant="expense" />
        <SummaryCard title="Saldo" amount={reports?.grandBalance ?? 0} variant="balance" />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        <section className="bg-white p-6 rounded-3xl border border-slate-200">
          <h3 className="font-bold text-slate-800 mb-6">📊 Gastos por Categoria</h3>
          <div className="h-64">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie data={chartData} innerRadius={60} outerRadius={80} dataKey="value">
                  {chartData.map((_, i) => <Cell key={i} fill={COLORS[i % COLORS.length]} />)}
                </Pie>
                <Tooltip formatter={(val: any) => formatCurrency(Number(val))} />
                <Legend />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </section>

        <section className="bg-slate-900 rounded-3xl p-6 text-white shadow-xl">
          <h3 className="text-[10px] font-black uppercase text-slate-500 tracking-widest mb-6">Atividade Recente</h3>
          <div className="space-y-4">
            {transactions.slice(0, 5).map(t => (
              <div key={t.id} className="flex justify-between items-center">
                <div>
                  <p className="font-bold text-sm">{t.description}</p>
                  <p className="text-[10px] text-slate-500 font-bold uppercase">
                    {categories.find(c => c.id === t.categoryId)?.description || 'Geral'}
                  </p>
                </div>
                <p className={`font-black ${Number(t.type) === 1 ? 'text-emerald-400' : 'text-rose-400'}`}>
                  {formatCurrency(t.value)}
                </p>
              </div>
            ))}
          </div>
        </section>
      </div>

      <section className="bg-white rounded-3xl border border-slate-200 overflow-hidden shadow-sm">
        <div className="p-6 border-b font-bold text-slate-800">Relatório Consolidado</div>
        <table className="w-full text-left">
          <thead className="text-[10px] font-black text-slate-400 uppercase bg-slate-50">
            <tr>
              <th className="p-5">Pessoa</th>
              <th className="p-5 text-right">Receitas</th>
              <th className="p-5 text-right">Despesas</th>
              <th className="p-5 text-right">Saldo</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100">
            {reports?.peopleTotals.map((p, i) => (
              <tr key={i} className="hover:bg-slate-50 transition-colors">
                <td className="p-5 font-bold text-slate-700">{p.personName}</td>
                <td className="p-5 text-right text-emerald-600 font-medium">{formatCurrency(p.totalIncome)}</td>
                <td className="p-5 text-right text-rose-500 font-medium">{formatCurrency(p.totalExpense)}</td>
                <td className={`p-5 text-right font-black ${p.balance >= 0 ? 'text-blue-600' : 'text-orange-500'}`}>
                  {formatCurrency(p.balance)}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </section>
    </div>
  );
}