using System.Collections.Generic;
using System.Threading.Tasks;
using GroupMe.Models;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GroupMe.Services;
using MeetUp.Models;

namespace GroupMe.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class GroupsController : ControllerBase
  {

    private readonly GroupsService _gs;

    public GroupsController(GroupsService gs)
    {
      _gs = gs;
    }

    [HttpGet]
    public ActionResult<List<Group>> GetGroups()
    {
      try
      {
        List<Group> groups = _gs.GetGroups();
        return Ok(groups);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{groupId}")]
    public ActionResult<Group> GetGroup(int groupId)
    {
      try
      {
        Group group = _gs.GetGroupById(groupId);
        return Ok(group);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{groupId}/members")]
    public ActionResult<List<GroupMember>> GetGroupMembers(int groupId)
    {
      try
      {
        List<GroupMember> members = _gs.GetGroupMembers(groupId);
        return Ok(members);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{groupId}/events")]
    public ActionResult<List<GroupEvent>> GetGroupEvents(int groupId)
    {
      try
      {
        List<GroupEvent> events = _gs.GetGroupEvents(groupId);
        return Ok(events);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{groupId}/events/{eventId}")]
    public ActionResult<GroupEvent> GetEvent(int groupId, int eventId)
    {
      try
      {
        GroupEvent groupEvent = _gs.GetEventById(eventId);
        return Ok(groupEvent);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{groupId}/events/{eventId}/attendees")]
    public ActionResult<List<Attendee>> GetAttendees(int groupId, int eventId)
    {
      try
      {
        List<Attendee> attendees = _gs.GetAttendeesByEventId(eventId);
        return Ok(attendees);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Group>> CreateGroup([FromBody] Group data)
    {
      try
      {
        var userInfo = await HttpContext.GetUserInfoAsync<Account>();
        data.OwnerId = userInfo.Id;
        Group group = _gs.Create(data);
        return Ok(group);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [Authorize]
    [HttpPost("{groupId}/events")]
    public async Task<ActionResult<GroupEvent>> CreateEvent([FromBody] GroupEvent data)
    {
      try
      {
        var userInfo = await HttpContext.GetUserInfoAsync<Account>();
        var groupEvent = _gs.Create(userInfo.Id, data);
        return Ok(groupEvent);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [Authorize]
    [HttpPut("{groupId}")]
    public async Task<ActionResult<Group>> EditGroup(int groupId, [FromBody] Group gData)
    {
      try
      {
        gData.Id = groupId;
        var userInfo = await HttpContext.GetUserInfoAsync<Account>();
        Group group = _gs.Update(userInfo.Id, gData);
        return Ok(group);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [Authorize]
    [HttpDelete("{groupId}")]
    public async Task<ActionResult<Group>> DeleteGroup(int groupId)
    {
      try
      {
        var userInfo = await HttpContext.GetUserInfoAsync<Account>();
        Group group = _gs.DeleteGroup(userInfo.Id, groupId);
        return Ok(group);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }

}