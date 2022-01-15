using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.StoreManagement.Models
{
    public class SuccessResult
    {
        public SuccessResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public SuccessResult(bool success)
        {
            Success = success;
            if (success)
                Message = "Action completed successfully!";
            else
                Message = "Something went wrong!";
        }
        public bool Success { get; }
        public string Message { get; }
    }
}
