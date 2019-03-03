﻿using System.ComponentModel.DataAnnotations;

namespace TopCalAPI.UserManagement.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginSuccessResponseModel
    {
        public string Message { get; set; }
    }
}
