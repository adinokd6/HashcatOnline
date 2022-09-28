using System;
using System.ComponentModel.DataAnnotations;
using static WebHash.Models.Enums.Enums;

namespace WebHash.Models
{
    public class InputHash
    {
        public string InputValue { get; set; }
        public string HashType { get; set; }
        public string AttackMethod { get; set; }

    }
}