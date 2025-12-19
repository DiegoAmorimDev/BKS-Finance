namespace Domain.Core.Entities;

public class Person
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Age { get; private set; }

    public Person(string name, int age)
    {
        Id = Guid.NewGuid();
        Name = name;
        Age = age;
    }

    // Para reconstrução do Dapper
    public Person(Guid id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    public bool IsMinor() => Age < 18;
}