interface SidebarProps {
  currentView: string;
  setView: (view: string) => void;
}

export function Sidebar({ currentView, setView }: SidebarProps) {
  const menuItems = [
    { id: 'dashboard', label: 'Dashboard', icon: '📊' },
    { id: 'transactions', label: 'Transações', icon: '💸' },
    { id: 'persons', label: 'Pessoas', icon: '👥' },
    { id: 'categories', label: 'Categorias', icon: '🏷️' },
  ];

  return (
    <aside className="w-64 bg-slate-900 text-white h-screen fixed left-0 top-0 p-4 shadow-2xl">
      <div className="mb-10 p-2">
        <h1 className="text-xl font-black tracking-tighter text-blue-400">BKS FINANCE</h1>
        <p className="text-[10px] text-slate-500 uppercase tracking-widest font-bold">Expense Control</p>
      </div>
      
      <nav className="space-y-1">
        {menuItems.map((item) => (
          <button
            key={item.id}
            onClick={() => setView(item.id)}
            className={`w-full flex items-center gap-3 p-3 rounded-xl transition-all duration-200 ${
              currentView === item.id 
                ? 'bg-blue-600 text-white shadow-lg shadow-blue-900/20' 
                : 'text-slate-400 hover:bg-slate-800 hover:text-white'
            }`}
          >
            <span className="text-xl">{item.icon}</span>
            <span className="font-semibold">{item.label}</span>
          </button>
        ))}
      </nav>
      
      <div className="absolute bottom-8 left-4 right-4 p-4 bg-slate-800/50 rounded-2xl border border-slate-700/50">
        <p className="text-[10px] text-slate-500 uppercase font-bold mb-1">Status Backend</p>
        <div className="flex items-center gap-2">
          <div className="w-2 h-2 bg-emerald-500 rounded-full animate-pulse"></div>
          <span className="text-xs text-slate-300 font-medium italic">Conectado API v1</span>
        </div>
      </div>
    </aside>
  );
}