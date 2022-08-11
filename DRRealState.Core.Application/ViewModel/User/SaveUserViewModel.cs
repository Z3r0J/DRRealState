using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.User
{
    public class SaveUserViewModel
    {
        [Required(ErrorMessage="Name is Required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        public string Documents { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is Required")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Password doesn't matches.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Phone is Required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public int UserType { get; set; }
        public string Error { get; set; }
        public bool HasError { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }

        public string PhotoURL { get; set; }

    }
}
