using System.ComponentModel.DataAnnotations;

namespace MainApp.Models.Enums
{
    public class Enums
    {
        public enum HashType
        {
            [Display(Name = "SHA1")]
            SHA1 = 100,
            [Display(Name = "MD5")]
            MD5 = 0,
            [Display(Name = "MD4")]
            MD4 = 900,
            [Display(Name = "SHA2-512")]
            SHA2 = 1700,

        }

        public enum AttackMethod
        {
            [Display(Name = "Straight")]
            Straight = 0,
            [Display(Name = "Combination")]
            Combination = 1,
            [Display(Name = "Brute-force")]
            BruteForce = 3,

        }
    }
}
