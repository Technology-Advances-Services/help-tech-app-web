namespace HelpTechAppWeb.Models
{
    public class Technical
    {
        public string Id { get; set; }
        public int SpecialtyId { get; set; }
        public int DistrictId { get; set; }
        public string ProfileUrl { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Genre { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Availability { get; set; }

        public Technical()
        {
            this.Id = string.Empty;
            this.SpecialtyId = 0;
            this.DistrictId = 0;
            this.ProfileUrl = string.Empty;
            this.Firstname = string.Empty;
            this.Lastname = string.Empty;
            this.Age = 0;
            this.Genre = string.Empty;
            this.Phone = 0;
            this.Email = string.Empty;
            this.Code = string.Empty;
            this.Availability = string.Empty;
        }

        public Technical
            (string id, int specialtyId, int districtId,
            string profileUrl, string firstname,
            string lastname, int age, string genre,
            int phone, string email, string code,
            string availability)
        {
            this.Id = id;
            this.SpecialtyId = specialtyId;
            this.DistrictId = districtId;
            this.ProfileUrl = profileUrl;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Age = age;
            this.Genre = genre;
            this.Phone = phone;
            this.Email = email;
            this.Code = code;
            this.Availability = availability;
        }
    }
}