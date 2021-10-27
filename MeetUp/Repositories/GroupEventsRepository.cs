using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using GroupMe.Interfaces;
using GroupMe.Models;

namespace GroupMe.Repositories
{
  public class GroupEventsRepository : IRepository<GroupEvent>
  {
    private readonly IDbConnection _db;

    public GroupEventsRepository(IDbConnection db)
    {
      _db = db;
    }

    public GroupEvent Create(GroupEvent data)
    {
      var sql = @"INSERT INTO group_events(
        name,
        description,
        startTime,
        date,
        location
      ) VALUES(
        @Name,
        @Description,
        @StartTime,
        @Date,
        @Location
      ); SELECT LAST_INSERT_ID();";
      data.Id = _db.ExecuteScalar<int>(sql, data);
      return data;
    }

    public List<GroupEvent> GetAll()
    {
      var sql = @"
      SELECT e.*, g.*, a.* 
      FROM group_events
      JOIN groups g ON g.id = e.groupId
      JOIN accounts a ON g.ownerId = a.id;
      ";

      return _db.Query<GroupEvent, Group, Profile, GroupEvent>(sql, (e, g, p) =>
      {
        g.Owner = p;
        e.Group = g;
        return e;
      }).ToList();

    }
    public List<GroupEvent> GetEventsByGroupId(int groupId)
    {
      var sql = @"
      SELECT e.*, g.*, a.* 
      FROM group_events
      JOIN groups g ON g.id = e.groupId
      JOIN accounts a ON g.ownerId = a.id;
      WHERE e.groupId = @groupId
      ";

      return _db.Query<GroupEvent, Group, Profile, GroupEvent>(sql, (e, g, p) =>
      {
        g.Owner = p;
        e.Group = g;
        return e;
      }, new { groupId }).ToList();

    }

    public GroupEvent GetById(int id)
    {
      var sql = @"
      SELECT e.*, g.*, a.* 
      FROM group_events
      JOIN groups g ON g.id = e.groupId
      JOIN accounts a ON g.ownerId = a.id
      WHERE g.id = @id;
      ";

      return _db.Query<GroupEvent, Group, Profile, GroupEvent>(sql, (e, g, p) =>
      {
        g.Owner = p;
        e.Group = g;
        return e;
      }, new { id }).FirstOrDefault();
    }

    public GroupEvent Update(GroupEvent data)
    {
      var sql = @"
        UPDATE group_events 
          SET 
            name = @Name,
            description = @Description,
            location = @Location,
            startTime = @StartTime,
            date = @Date
          WHERE
            id = @Id
          LIMIT 1;
      ";

      _db.Execute(sql, data);
      return data;
    }

    public void Delete(int id)
    {
      var sql = "DELETE FROM group_events WHERE id = @id";
      _db.Execute(sql, new { id });
    }
  }
}