interface Props {
  title: string;
  amount: number;
  variant: 'income' | 'expense' | 'balance';
}

export function SummaryCard({ title, amount, variant }: Props) {
  const color = {
    income: 'text-green-600',
    expense: 'text-red-600',
    balance: 'text-blue-600'
  }[variant];

  return (
    <div className="p-4 bg-white rounded-lg shadow border border-gray-200">
      <p className="text-gray-500 text-sm font-medium uppercase">{title}</p>
      <p className={`text-2xl font-bold ${color}`}>
        € {amount.toFixed(2)}
      </p>
    </div>
  );
}