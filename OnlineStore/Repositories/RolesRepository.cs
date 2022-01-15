using OnlineStore.Entities;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        protected readonly ApplicationDBContext _cineflexxContext;

        public RolesRepository(ApplicationDBContext cineflexxContext)
        {
            _cineflexxContext = cineflexxContext;
        }

        public void AddUserRole(AspNetUserRoles userRole)
        {
            try
            {
                _cineflexxContext.Add(userRole);
                _cineflexxContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<AspNetRoles> GetAllRoles()
        {
            try
            {
                return _cineflexxContext.AspNetRoles.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AspNetRoles GetById(string id)
        {
            try
            {
                return _cineflexxContext.AspNetRoles.Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AspNetRoles GetByUserId(string userId)
        {
            try
            {
                return _cineflexxContext.AspNetUserRoles.Where(x => x.UserId == userId).Select(x => x.Role).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
