namespace services;

using models;

class PersonRepository : IPersonRepository
{
    private readonly Dictionary<string, IPerson> _store = new(StringComparer.OrdinalIgnoreCase);

    public void Add(IPerson person)
    {
        if (_store.ContainsKey(person.Email))
            throw new InvalidOperationException($"A person with email '{person.Email}' already exists.");

        _store[person.Email] = person;
    }

    public IPerson? GetByEmail(string email) =>
        _store.TryGetValue(email, out var person) ? person : null;

    public IEnumerable<IPerson> GetAll() => _store.Values;

    public bool Update(string email, IPerson updated)
    {
        if (!_store.ContainsKey(email))
            return false;

        if (!email.Equals(updated.Email, StringComparison.OrdinalIgnoreCase))
        {
            _store.Remove(email);
        }

        _store[updated.Email] = updated;
        return true;
    }

    public bool Delete(string email) => _store.Remove(email);
}
