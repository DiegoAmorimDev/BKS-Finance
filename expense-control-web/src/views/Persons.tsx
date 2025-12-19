import { useEffect, useState } from 'react';
import { apiService } from '../services/api';
import type { Person } from '../types';

export function Persons() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [name, setName] = useState('');
  const [age, setAge] = useState<number | ''>('');

  // 1. LISTAGEM: Chama o group.MapGet("/") do seu C#
  const loadPersons = async () => {
    try {
      const data = await apiService.getPersons();
      setPersons(data);
    } catch (e) {
      console.error("Erro ao carregar pessoas");
    }
  };

  useEffect(() => { loadPersons(); }, []);

  // 2. CRIAÇÃO: Chama o group.MapPost("/") do seu C#
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name || age === '') return;

    try {
      // O seu DTO de request espera { Name, Age }
      // O axios/v8 vai enviar como json { name, age } que o .NET entende perfeitamente
      await apiService.createPerson({ name, age: Number(age) });
      setName(''); 
      setAge('');
      loadPersons(); // Recarrega a lista após criar
    } catch (err) {
      // O erro de validação (ValidationProblem) será pego pelo interceptor do api.ts
    }
  };

  // 3. DELEÇÃO: Chama o group.MapDelete("/{id:guid}") do seu C#
  const handleDelete = async (id: string) => {
    if (confirm("Deseja mesmo excluir?")) {
      await apiService.deletePerson(id);
      loadPersons();
    }
  };

  return (
    <div className="space-y-8">
      <h2 className="text-3xl font-black text-slate-900">Pessoas</h2>

      <form onSubmit={handleSubmit} className="bg-white p-6 rounded-3xl border border-slate-200 flex gap-4 items-end shadow-sm">
        <div className="flex-1">
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Nome</label>
          <input 
            className="w-full border p-3 rounded-xl outline-none focus:ring-2 focus:ring-blue-500"
            value={name} onChange={e => setName(e.target.value)} required 
          />
        </div>
        <div className="w-32">
          <label className="block text-xs font-bold text-slate-500 uppercase mb-2">Idade</label>
          <input 
            type="number" className="w-full border p-3 rounded-xl outline-none"
            value={age} onChange={e => setAge(e.target.value === '' ? '' : Number(e.target.value))} required 
          />
        </div>
        <button className="bg-blue-600 text-white px-8 py-3.5 rounded-xl font-bold hover:bg-blue-700 transition-all">
          Adicionar
        </button>
      </form>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {persons.map(p => (
          <div key={p.id} className="bg-white p-5 rounded-2xl border border-slate-200 flex justify-between items-center group hover:border-blue-300">
            <div>
              <p className="font-bold text-slate-800">{p.name}</p>
              <p className="text-xs text-slate-500 font-medium">{p.age} anos</p>
            </div>
            <button onClick={() => handleDelete(p.id)} className="text-slate-300 hover:text-red-500 p-2">
              🗑️
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}