namespace HelpTechAppWeb.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User()
        {
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.Role = string.Empty;
        }
        public User
            (string username, string password,
            string role)
        {
            this.Username = username;
            this.Password = password;
            this.Role = role;
        }
    }
}