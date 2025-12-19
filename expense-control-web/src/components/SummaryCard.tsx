import { formatCurrency } from '../utils/format';

interface SummaryCardProps {
  title: string;
  amount: number;
  variant: 'income' | 'expense' | 'balance';
}

export function SummaryCard({ title, amount, variant }: SummaryCardProps) {
  const styles = {
    income: { text: 'text-emerald-600', bg: 'group-hover:border-emerald-200' },
    expense: { text: 'text-rose-600', bg: 'group-hover:border-rose-200' },
    balance: { text: 'text-blue-600', bg: 'group-hover:border-blue-200' }
  };

  const currentStyle = styles[variant];

  return (
    <div className={`bg-white p-6 rounded-3xl border border-slate-200 shadow-sm group transition-all duration-300 ${currentStyle.bg}`}>
      <div className="flex justify-between items-start mb-2">
        <span className="text-[10px] font-black text-slate-400 uppercase tracking-widest">{title}</span>
        <span className="text-xs">{variant === 'income' ? '📈' : variant === 'expense' ? '📉' : '⚖️'}</span>
      </div>
      
      <div className="flex items-baseline gap-1">
        <span className={`text-3xl font-black tracking-tight ${currentStyle.text}`}>
          {formatCurrency(amount)}
        </span>
      </div>
      
      <div className="mt-4 w-full bg-slate-100 h-1 rounded-full overflow-hidden">
        <div 
          className={`h-full transition-all duration-1000 ${variant === 'income' ? 'bg-emerald-500' : variant === 'expense' ? 'bg-rose-500' : 'bg-blue-500'}`}
          style={{ width: '100%' }}
        />
      </div>
    </div>
  );
}