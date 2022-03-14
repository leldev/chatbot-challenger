namespace Jobsity.Chatbot.Api.Common.Services
{
    public class SecurityService
    {
        public static string HashText(string text)
        {
            var data = System.Text.Encoding.ASCII.GetBytes(text);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            var hash = System.Text.Encoding.ASCII.GetString(data);

            return hash;
        }
    }
}
