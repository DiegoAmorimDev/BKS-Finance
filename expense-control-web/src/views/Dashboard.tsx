import { useEffect, useState, useCallback } from 'react';
import { apiService } from '../services/api';
import type { ReportTotals, Transaction, Category } from '../types';
import { SummaryCard } from '../components/SummaryCard';
import { PieChart, Pie, Cell, ResponsiveContainer, Tooltip, Legend } from 'recharts';

export function Dashboard() {
  const [reports, setReports] = useState<ReportTotals | null>(null);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);

  const loadData = useCallback(async () => {
    try {
      setLoading(true);
      // Busca os dados em paralelo
      const [rData, tData, cData] = await Promise.all([
        apiService.getReports().catch(() => null),
        apiService.getTransactions().catch(() => []),
        apiService.getCategories().catch(() => [])
      ]);
      
      setReports(rData);
      setTransactions(Array.isArray(tData) ? tData : []);
      setCategories(Array.isArray(cData) ? cData : []);
    } catch (e) {
      console.error("Erro crítico no Dashboard:", e);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { loadData(); }, [loadData]);

  // Cálculo seguro do gráfico - evita crash se dados forem nulos
  const chartData = (categories ?? [])
    .map(cat => {
      const total = (transactions ?? [])
        .filter(t => t.categoryId === cat.id && Number(t.type) === 0)
        .reduce((sum, t) => sum + (t.value || 0), 0);
      return { name: cat.description, value: total };
    })
    .filter(item => item.value > 0);

  const COLORS = ['#6366f1', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', '#ec4899'];

  if (loading) return (
    <div className="h-96 flex items-center justify-center font-bold text-slate-400 animate-pulse">
      Sincronizando dados...
    </div>
  );

  return (
    <div className="space-y-8 pb-10">
      <header className="flex justify-between items-end">
        <div>
          <h2 className="text-4xl font-black text-slate-900 tracking-tight">Dashboard</h2>
          <p className="text-slate-500 font-medium">BKS Finance — Controle Residencial</p>
        </div>
      </header>

      {/* KPIs com fallback para 0 caso o backend falhe */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <SummaryCard title="Receitas" amount={reports?.grandTotalIncome ?? 0} variant="income" />
        <SummaryCard title="Despesas" amount={reports?.grandTotalExpense ?? 0} variant="expense" />
        <SummaryCard title="Saldo" amount={reports?.grandBalance ?? 0} variant="balance" />
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        {/* Gráfico de Pizza */}
        <section className="bg-white p-6 rounded-3xl border border-slate-200 shadow-sm">
          <h3 className="font-bold text-slate-800 mb-6">📊 Despesas por Categoria</h3>
          <div className="h-64 w-full">
            {chartData.length > 0 ? (
              <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                  <Pie data={chartData} innerRadius={60} outerRadius={80} paddingAngle={5} dataKey="value">
                    {chartData.map((_, index) => (
                      <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                  </Pie>
                  <Tooltip 
                    formatter={(val: any) => `€ ${Number(val).toFixed(2)}`}
                    contentStyle={{ borderRadius: '12px', border: 'none' }}
                  />
                  <Legend />
                </PieChart>
              </ResponsiveContainer>
            ) : (
              <div className="h-full flex items-center justify-center text-slate-400 italic text-sm">Sem dados para o gráfico.</div>
            )}
          </div>
        </section>

        {/* Últimas Transações */}
        <section className="bg-slate-900 rounded-3xl p-6 text-white shadow-xl">
          <h3 className="font-bold text-slate-400 mb-6 uppercase text-[10px] tracking-widest">Atividade</h3>
          <div className="space-y-4">
            {(transactions ?? []).slice(0, 5).map(t => (
              <div key={t.id} className="flex justify-between items-center">
                <div>
                  <p className="font-bold text-sm">{t.description}</p>
                  <p className="text-[10px] text-slate-500 uppercase font-black">
                    {categories.find(c => c.id === t.categoryId)?.description || 'Outros'}
                  </p>
                </div>
                <p className={`font-black ${Number(t.type) === 1 ? 'text-emerald-400' : 'text-rose-400'}`}>
                   € {t.value?.toFixed(2)}
                </p>
              </div>
            ))}
          </div>
        </section>
      </div>

      {/* Relatório por Pessoa */}
      <section className="bg-white rounded-3xl border border-slate-200 shadow-sm overflow-hidden">
        <div className="p-6 border-b border-slate-50 font-bold text-slate-800">Totais por Pessoa</div>
        <div className="overflow-x-auto">
          <table className="w-full text-left">
            <thead>
              <tr className="text-[10px] font-black text-slate-400 uppercase border-b">
                <th className="p-5">Nome</th>
                <th className="p-5 text-right">Ganhos</th>
                <th className="p-5 text-right">Gastos</th>
                <th className="p-5 text-right">Saldo</th>
              </tr>
            </thead>
            <tbody>
              {(reports?.peopleTotals ?? []).map((p, idx) => (
                <tr key={idx} className="hover:bg-slate-50 font-medium">
                  <td className="p-5 text-slate-900">{p.personName}</td>
                  <td className="p-5 text-right text-emerald-600">€ {p.totalIncome?.toFixed(2)}</td>
                  <td className="p-5 text-right text-rose-500">€ {p.totalExpense?.toFixed(2)}</td>
                  <td className={`p-5 text-right font-black ${p.balance >= 0 ? 'text-blue-600' : 'text-orange-500'}`}>
                    € {p.balance?.toFixed(2)}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </section>
    </div>
  );
}