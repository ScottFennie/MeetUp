using System;

namespace GroupMe.Models
{
  public class DbItem<T>
  {
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}