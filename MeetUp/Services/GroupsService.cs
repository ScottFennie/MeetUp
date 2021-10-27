using System.Collections.Generic;
using GroupMe.Models;
using GroupMe.Repositories;

namespace GroupMe.Services
{
  public class GroupsService
  {
    private readonly GroupsRepository _groupsRepo;
    private readonly GroupMembersRepository _groupMembersRepo;
    private readonly GroupEventsRepository _groupEventsRepo;
    private readonly AttendeesRepository _attendeesRepo;

    public GroupsService(GroupsRepository groupsRepo, GroupMembersRepository groupMembersRepo, GroupEventsRepository groupEventsRepo, AttendeesRepository attendeesRepo)
    {
      _groupsRepo = groupsRepo;
      _groupMembersRepo = groupMembersRepo;
      _groupEventsRepo = groupEventsRepo;
      _attendeesRepo = attendeesRepo;
    }

    internal List<Group> GetGroups()
    {
      return _groupsRepo.GetAll();
    }

    #region GroupsService
    public Group Create(Group data)
    {
      return _groupsRepo.Create(data);
    }

    public Group GetGroupById(int id)
    {
      var group = _groupsRepo.GetById(id);
      if (group == null)
      {
        throw new System.Exception("Bad Group Id");
      }
      return group;
    }

    public Group Update(string userId, Group data)
    {
      var group = IsGroupOwner(userId, data.Id);
      group.Name = data.Name ?? group.Name;
      group.Picture = data.Picture ?? group.Picture;
      group.Description = data.Description ?? group.Description;
      return _groupsRepo.Update(group);
    }

    internal List<GroupMember> GetGroupMembers(int groupId)
    {
      return _groupMembersRepo.GetAllMembersByGroupId(groupId);
    }

    internal List<GroupEvent> GetGroupEvents(int groupId)
    {
      return _groupEventsRepo.GetEventsByGroupId(groupId);
    }

    private Group IsGroupOwner(string userId, int id)
    {
      var group = GetGroupById(id);
      if (group.OwnerId != userId)
      {
        throw new System.Exception("You are not the owner of the group");
      }
      return group;
    }

    internal GroupEvent GetEventById(int eventId)
    {
      var groupEvent = _groupEventsRepo.GetById(eventId);
      if (groupEvent == null)
      {
        throw new System.Exception("Bad Event Id");
      }
      return groupEvent;
    }

    public Group DeleteGroup(string userId, int groupId)
    {

      var group = IsGroupOwner(userId, groupId);
      _groupsRepo.Delete(groupId);
      return group;
    }

    internal List<Attendee> GetAttendeesByEventId(int eventId)
    {
      return _attendeesRepo.GetAllAttendeesByEventId(eventId);
    }

    #endregion

    #region GroupMember
    public GroupMember Create(GroupMember data)
    {
      return _groupMembersRepo.Create(data);
    }
    #endregion

    public GroupEvent Create(string userId, GroupEvent data)
    {
      IsGroupOwner(userId, data.GroupId);
      return _groupEventsRepo.Create(data);
    }
    public Attendee Create(Attendee data)
    {
      return _attendeesRepo.Create(data);
    }

    public GroupMember Update(GroupMember data)
    {
      return _groupMembersRepo.Update(data);
    }
    public GroupEvent Update(string userId, GroupEvent data)
    {
      IsGroupOwner(userId, data.GroupId);
      return _groupEventsRepo.Update(data);
    }
    public Attendee Update(Attendee data)
    {
      return _attendeesRepo.Update(data);
    }

    /// <summary>
    /// This is assuming the request is from the member not the group owner
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupMemberId"></param>
    public void DeleteGroupMember(string userId, int groupMemberId)
    {
      var gm = _groupMembersRepo.GetById(groupMemberId);
      if (gm == null)
      {
        throw new System.Exception("Bad GroupMember id");
      }
      if (gm.MemberId != userId)
      {
        throw new System.Exception("nah that is not yours");
      }
      _groupMembersRepo.Delete(groupMemberId);
    }
    public void DeleteGroupEvent(string userId, int groupId, int eventId)
    {
      IsGroupOwner(userId, groupId);
      _groupEventsRepo.Delete(eventId);
    }
    public void DeleteAttendee(int id)
    {
      _attendeesRepo.Delete(id);
    }
  }
}