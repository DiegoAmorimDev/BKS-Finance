import { useEffect, useState, useCallback } from 'react';
import { apiService } from '../services/api';
import type { Transaction } from '../types';
import { SummaryCard } from '../components/ui/SumaryCard';
import { TransactionForm } from '../components/TransactionForm';

export function Dashboard() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  // Função reutilizável para carregar dados
  const loadData = useCallback(async () => {
    try {
      const data = await apiService.getTransactions();
      setTransactions(data);
    } catch (error) {
      console.error("Erro ao procurar transações:", error);
    } finally {
      setIsLoading(false);
    }
  }, []);

  // Carrega na montagem do componente
  useEffect(() => {
    loadData();
  }, [loadData]);

  // Cálculos rápidos
  const income = transactions
    .filter(t => t.type === 'income')
    .reduce((acc, t) => acc + t.amount, 0);
    
  const expense = transactions
    .filter(t => t.type === 'expense')
    .reduce((acc, t) => acc + t.amount, 0);

  return (
    <div className="max-w-6xl mx-auto space-y-8">
      {/* 1. Resumo Financeiro */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <SummaryCard title="Entradas" amount={income} variant="income" />
        <SummaryCard title="Saídas" amount={expense} variant="expense" />
        <SummaryCard title="Saldo" amount={income - expense} variant="balance" />
      </div>

      {/* 2. Formulário de Nova Transação */}
      <section>
        <h2 className="text-xl font-semibold mb-4 text-gray-700">Nova Transação</h2>
        <TransactionForm onSuccess={loadData} />
      </section>

      {/* 3. Tabela de Transações */}
      <section className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden">
        <div className="p-4 border-b border-gray-100 bg-gray-50/50">
          <h2 className="font-semibold text-gray-700">Histórico Recente</h2>
        </div>
        
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="text-sm text-gray-500 border-b">
                <th className="p-4 font-medium">Data</th>
                <th className="p-4 font-medium">Descrição</th>
                <th className="p-4 font-medium text-right">Valor</th>
                <th className="p-4 font-medium text-center">Tipo</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {isLoading ? (
                <tr><td colSpan={4} className="p-8 text-center text-gray-400">A carregar...</td></tr>
              ) : transactions.length === 0 ? (
                <tr><td colSpan={4} className="p-8 text-center text-gray-400">Nenhuma transação encontrada.</td></tr>
              ) : (
                transactions.map(t => (
                  <tr key={t.id} className="hover:bg-gray-50 transition-colors">
                    <td className="p-4 text-sm text-gray-600">
                      {new Date(t.date).toLocaleDateString('pt-PT')}
                    </td>
                    <td className="p-4 font-medium text-gray-800">{t.description}</td>
                    <td className={`p-4 text-right font-mono font-bold ${t.type === 'income' ? 'text-green-600' : 'text-red-600'}`}>
                      {t.type === 'expense' ? '-' : '+'} € {t.amount.toLocaleString('pt-PT', { minimumFractionDigits: 2 })}
                    </td>
                    <td className="p-4 text-center">
                      <span className={`text-[10px] uppercase px-2 py-1 rounded-full font-bold ${
                        t.type === 'income' ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                      }`}>
                        {t.type === 'income' ? 'Receita' : 'Despesa'}
                      </span>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </section>
    </div>
  );
}