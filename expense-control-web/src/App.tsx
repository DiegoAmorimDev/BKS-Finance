import { useState } from 'react';
import { Sidebar } from './components/Sidebar';
import { Dashboard } from './views/Dashboard';
import { Persons } from './views/Persons';
import { Categories } from './views/Categories';
import { Transactions } from './views/Transactions';

function App() {
  const [currentView, setView] = useState('dashboard');

  return (
    <div className="flex min-h-screen bg-[#F8FAFC]">
      <Sidebar currentView={currentView} setView={setView} />
      
      <main className="ml-64 flex-1 p-8">
        <div className="max-w-6xl mx-auto">
          {currentView === 'dashboard' && <Dashboard />}
          {currentView === 'persons' && <Persons />}
          {currentView === 'categories' && <Categories />}
          {currentView === 'transactions' && <Transactions />}
        </div>
      </main>
    </div>
  );
}

export default App;