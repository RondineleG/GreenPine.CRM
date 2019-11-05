namespace GreenPine.CRM.Model.Shared.Requests
{
    public class ResetPassword
    {
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
