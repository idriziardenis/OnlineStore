using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Interfaces
{
    public interface IUserService
    {
        string GetUserId();
        string GetUserName();
        string GetUserEmail();
        string GetUserPhoneNumber();
        string GetUserRole();
        string GetFullName();
        //string GetProfilePicture(bool thumbnail);
    }
}
