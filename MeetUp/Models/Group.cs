namespace GroupMe.Models
{
  public class Group : DbItem<int>
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Picture { get; set; }
    public string OwnerId { get; set; }
    public Profile Owner { get; set; }
  }
}