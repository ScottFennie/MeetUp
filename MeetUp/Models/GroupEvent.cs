using System;

namespace GroupMe.Models
{
  public class GroupEvent : DbItem<int>
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string StartTime { get; set; }
    public DateTime Date { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
  }
}