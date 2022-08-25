using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.User
{
    public class SaveEditViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage="Name is Required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Documents is Required")]
        [DataType(DataType.Text)]
        public string Documents { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is Required")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public int UserType { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }

        public string PhotoURL { get; set; }
        public string Error { get; set; }
        public bool HasError { get; set; }

    }
}
