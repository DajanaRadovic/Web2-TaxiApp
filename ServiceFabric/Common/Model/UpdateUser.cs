﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class UpdateUser
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public IFormFile? Image { get; set; }
        public string? Email { get; set; }
        public string? PreviousEmail { get; set; }
        public string? Address { get; set; }
        public string? Birthday { get; set; }

        public UpdateUser() { }
    }
}
