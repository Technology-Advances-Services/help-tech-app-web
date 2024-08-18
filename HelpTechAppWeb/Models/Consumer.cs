namespace HelpTechAppWeb.Models
{
    public class Consumer
    {
        public int Id { get; private set; }
        public int DistrictId { get; private set; }
        public string ProfileUrl { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public int Age { get; private set; }
        public string Genre { get; private set; }
        public int Phone { get; private set; }
        public string Email { get; private set; }
        public string Code { get; private set; }

        public Consumer()
        {
            this.Id = 0;
            this.DistrictId = 0;
            this.ProfileUrl = string.Empty;
            this.Firstname = string.Empty;
            this.Lastname = string.Empty;
            this.Age = 0;
            this.Genre = string.Empty;
            this.Phone = 0;
            this.Email = string.Empty;
            this.Code = string.Empty;
        }

        public Consumer
            (int id, int districtId, string profileUrl,
            string firstname,string lastname, int age,
            string genre, int phone, string email,
            string code)
        {
            this.Id = id;
            this.DistrictId = districtId;
            this.ProfileUrl = profileUrl;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Age = age;
            this.Genre = genre;
            this.Phone = phone;
            this.Email = email;
            this.Code = code;
        }
    }
}