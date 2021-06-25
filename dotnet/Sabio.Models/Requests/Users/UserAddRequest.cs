using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sabio.Models.Requests.Users
{
    public class UserAddRequest
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirm { get; set; }

        [Required]
        public string AvatarUrl { get; set; }

        [Required]
        public string UserId { get; set; }

    }
}
