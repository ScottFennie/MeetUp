using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using GroupMe.Interfaces;
using GroupMe.Models;

namespace GroupMe.Repositories
{
  public class GroupMembersRepository : IRepository<GroupMember>
  {
    private readonly IDbConnection _db;

    public GroupMembersRepository(IDbConnection db)
    {
      _db = db;
    }

    public GroupMember Create(GroupMember data)
    {
      var sql = @"INSERT INTO group_members(
        groupId,
        memberId,
        role
      ) VALUES(
        @GroupId,
        @MemberId,
        @Role
      ); SELECT LAST_INSERT_ID();";
      data.Id = _db.ExecuteScalar<int>(sql, data);
      return data;
    }

    public List<GroupMember> GetAll()
    {
      throw new NotSupportedException();
    }

    public List<GroupMember> GetAllMembersByGroupId(int groupId)
    {
      var sql = @"
      SELECT gm.*, p.* FROM group_members gm
      JOIN accounts p ON p.id = gm.memberId
      WHERE gm.groupId = @groupId AND gm.role != 'Removed'";

      return _db.Query<GroupMember, Profile, GroupMember>(sql, (gm, p) =>
      {
        gm.Member = p;
        return gm;
      }, new { groupId }).ToList();
    }

    public GroupMember GetById(int id)
    {
      var sql = @"
        SELECT gm.*, p.* FROM group_members 
        JOIN accounts p ON p.id = gm.memberId
        WHERE id = @id AND role != 'Removed';
      ";

      return _db.Query<GroupMember, Profile, GroupMember>(sql, (gm, p) =>
      {
        gm.Member = p;
        return gm;
      }, new { id }).FirstOrDefault();
    }

    public GroupMember Update(GroupMember data)
    {
      var sql = @"
        UPDATE group_members 
          SET 
            role = @Role
          WHERE
            id = @Id
          LIMIT 1;
      ";

      _db.Execute(sql, data);
      return data;
    }
    public void Delete(int id)
    {
      var sql = @"UPDATE group_members 
        SET
          role = 'Removed' 
        WHERE id = @id";
      _db.Execute(sql, new { id });
    }
  }
}