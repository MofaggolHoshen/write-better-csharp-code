namespace services;

using models;

interface IPersonRepository
{
    void Add(IPerson person);
    IPerson? GetByEmail(string email);
    IEnumerable<IPerson> GetAll();
    bool Update(string email, IPerson updated);
    bool Delete(string email);
}
