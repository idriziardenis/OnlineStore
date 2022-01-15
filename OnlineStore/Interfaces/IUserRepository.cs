using OnlineStore.Entities;
using OnlineStore.Enums;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Interfaces
{
    public interface IUserRepository : IRepository<AspNetUsers>
    {
        AspNetUsers GetByStringId(string id);
        IEnumerable<AspNetUsers> GetAllStaff();
        IEnumerable<AspNetUsers> GetAllClients();
        IEnumerable<AspNetUsers> GetAllClientsByGender(Gender gender);
        IEnumerable<AspNetUsers> GetClientsByWeek();
        void RemoveAllRoles(string userId);
        AspNetUserRoles GetUserRoleByUserId(string userId);
        AspNetRoles GetUserRole(string userId);
        AspNetUsers GetByEmail(string email);
    }
}
