using OnlineStore.Entities;
using OnlineStore.Enums;
using OnlineStore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Repositories
{
    public class UserRepository : Repository<AspNetUsers>, IUserRepository
    {
        protected readonly ApplicationDBContext _onlineStoreContext;
        public UserRepository(ApplicationDBContext onlineStoreContext) : base(onlineStoreContext)
        {
            _onlineStoreContext = onlineStoreContext;
        }

        public IEnumerable<AspNetUsers> GetAllClients()
        {
            try
            {
                return _onlineStoreContext.AspNetUserRoles.Include(x => x.User).Where(x => x.User.IsDeleted == false && x.RoleId == "1").Select(x => x.User);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<AspNetUsers> GetAllClientsByGender(Gender gender)
        {
            try
            {
                var isMan = (gender == Gender.Man ? true : false);
                return _onlineStoreContext.AspNetUserRoles.Include(x => x.User).Where(x => x.User.IsDeleted == false && x.RoleId == "1" && x.User.Gender == isMan).Select(x => x.User);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<AspNetUsers> GetClientsByWeek()
        {
            try
            {
                var startDate = DateTime.Now.AddDays(-(double)DateTime.Now.DayOfWeek);
                return _onlineStoreContext.AspNetUserRoles.Include(x => x.User).Where(x => x.User.IsDeleted == false && x.RoleId == "1").Select(x => x.User).Where(x => x.IsDeleted == false && x.CreateOnDate >= startDate).OrderBy(x => x.CreateOnDate);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<AspNetUsers> GetAllStaff()
        {
            try
            {
                return _onlineStoreContext.AspNetUserRoles.Include(x=>x.User).Where(x => x.User.IsDeleted == false && x.RoleId != "1").Select(x => x.User);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AspNetUsers GetByStringId(string id)
        {
            try
            {
                return _onlineStoreContext.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public AspNetUserRoles GetUserRoleByUserId(string userId)
        {
            try
            {
                return _onlineStoreContext.AspNetUserRoles.Where(x => x.UserId == userId).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void RemoveAllRoles(string userId)
        {
            try
            {
                var userRoles = _onlineStoreContext.AspNetUserRoles.Where(x => x.UserId == userId);
                _onlineStoreContext.AspNetUserRoles.RemoveRange(userRoles);
                _onlineStoreContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateUserRole(AspNetUserRoles userRole)
        {
            try
            {
                _onlineStoreContext.Update(userRole);
                _onlineStoreContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AspNetRoles GetUserRole(string userId)
        {
            try
            {
                return _onlineStoreContext.AspNetUserRoles.Include(x => x.User).Include(x=>x.Role).Where(x => x.User.IsDeleted == false && x.UserId == userId).Select(x => x.Role).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AspNetUsers GetByEmail(string email)
        {
            try
            {
                return _onlineStoreContext.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
