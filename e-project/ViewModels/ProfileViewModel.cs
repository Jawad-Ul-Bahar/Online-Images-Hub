namespace e_project.ViewModels
{
    public class ProfileViewModel
    {
        public string Email { get; set; }
        public ChangePasswordViewModel PasswordModel { get; set; } = new ChangePasswordViewModel();
    }
}
