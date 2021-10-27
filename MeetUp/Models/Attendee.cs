namespace GroupMe.Models
{
  public class Attendee : DbItem<int>
  {
    public int GroupId { get; set; }
    public int EventId { get; set; }
    public string MemberId { get; set; }

    public Group Group { get; set; }
    public GroupEvent Event { get; set; }
    public Profile Member { get; set; }
  }
}