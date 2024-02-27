namespace EndPoint.ApI.Models.ViewModels.AuthenticationViewModel
{
    public class SignInViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }=string.Empty;
        public string Url { get; set; }
    }
}
