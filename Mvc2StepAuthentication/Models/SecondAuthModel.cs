using System.ComponentModel.DataAnnotations;

namespace Mvc2StepAuthentication.Models
{
    public class SecondAuthModel
    {
        [Required]
        [Display(Name = "Authorization Token")]
        public string Token { get; set; }
    }
}