using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using GroupMe.Interfaces;
using GroupMe.Models;

namespace GroupMe.Repositories
{
  public class AttendeesRepository : IRepository<Attendee>
  {
    private readonly IDbConnection _db;

    public AttendeesRepository(IDbConnection db)
    {
      _db = db;
    }

    public Attendee Create(Attendee data)
    {
      var sql = @"
      INSERT INTO 
      attendees(groupId, eventId, memberId) 
      VALUES(@GroupId, @EventId, @MemberId);
      SELECT LAST_INSERT_ID();
      ";
      data.Id = _db.ExecuteScalar<int>(sql, data);
      return data;
    }

    public void Delete(int id)
    {
      var sql = "DELETE FROM attendees WHERE id = @id";
      _db.Execute(sql);
    }

    public List<Attendee> GetAllAttendeesByEventId(int eventId)
    {
      var sql = @"
      SELECT a.*, p.* FROM attendees a
      JOIN accounts p ON p.id = a.memberId 
      WHERE a.eventId = @eventId;
      ";
      return _db.Query<Attendee, Profile, Attendee>(sql, (a, p) =>
      {
        a.Member = p;
        return a;
      }, new { eventId }).ToList();
    }

    public List<Attendee> GetAll()
    {
      throw new NotSupportedException();
    }

    public Attendee GetById(int id)
    {
      throw new NotImplementedException();
    }

    public Attendee Update(Attendee data)
    {
      throw new NotSupportedException();
    }
  }
}