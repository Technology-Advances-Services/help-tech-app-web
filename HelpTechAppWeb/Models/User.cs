namespace HelpTechAppWeb.Models
{
    public class User
    {
        public int Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User()
        {
            this.Username = 0;
            this.Password = string.Empty;
            this.Role = string.Empty;
        }
        public User
            (int username, string password,
            string role)
        {
            this.Username = username;
            this.Password = password;
            this.Role = role;
        }
    }
}