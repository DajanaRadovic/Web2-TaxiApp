using Common.DTOs;
using Common.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace WebService.Helpers
{
   
        public class ValidationServices
        {
            public IActionResult ValidateUserRegistration(Signup user)
            {
                if (string.IsNullOrEmpty(user.Email) || !ValidEmail(user.Email))
                    return new BadRequestObjectResult("Invalid email format");

                if (string.IsNullOrEmpty(user.Password))
                    return new BadRequestObjectResult("Password cannot be null or empty");

                if (string.IsNullOrEmpty(user.Username))
                    return new BadRequestObjectResult("Username cannot be null or empty");

                if (string.IsNullOrEmpty(user.FirstName))
                    return new BadRequestObjectResult("First name cannot be null or empty");

                if (string.IsNullOrEmpty(user.LastName))
                    return new BadRequestObjectResult("Last name cannot be null or empty");

                if (string.IsNullOrEmpty(user.Address))
                    return new BadRequestObjectResult("Address cannot be null or empty");

                if (string.IsNullOrEmpty(user.TypeUser))
                    return new BadRequestObjectResult("Type of user must be selected!");

                if (string.IsNullOrEmpty(user.Birthday))
                    return new BadRequestObjectResult("Birthday need to be selected!");

                if (user.ImageUrl.Length == 0)
                    return new BadRequestObjectResult("You need to send image while doing registration!");

                return null;
            }

            private bool ValidEmail(string email)
            {
                const string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
                return Regex.IsMatch(email, pattern);
            }

        }
    }

