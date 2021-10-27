using System.Collections.Generic;

namespace GroupMe.Interfaces
{
  public interface IRepository<T>
  {
    List<T> GetAll();
    T Create(T data);
    T GetById(int id);
    T Update(T data);
    void Delete(int id);
  }
}