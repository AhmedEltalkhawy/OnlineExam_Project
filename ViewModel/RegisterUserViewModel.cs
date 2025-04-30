namespace OnlineExamProject.ViewModel
{
    public class RegisterUserViewModel
    {
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [RegularExpression("^[0]{1}[1]{1}[0-1-2-5]{1}[0-9]{8}$"
            , ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

    }
}
