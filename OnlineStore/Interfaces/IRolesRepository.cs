using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Interfaces
{
    public interface IRolesRepository
    {
        IEnumerable<AspNetRoles> GetAllRoles();
        AspNetRoles GetByUserId(string userId);
        AspNetRoles GetById(string id);
        void AddUserRole(AspNetUserRoles userRole);

    }
}
