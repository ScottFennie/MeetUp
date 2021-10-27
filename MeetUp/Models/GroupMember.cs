namespace GroupMe.Models
{
  public class GroupMember : DbItem<int>
  {
    public string Role { get; set; } = "Member";
    public int GroupId { get; set; }
    public string MemberId { get; set; }
    public Group Group { get; set; }
    public Profile Member { get; set; }
  }
}