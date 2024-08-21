namespace HelpTechAppWeb.Models
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Specialty()
        {
            this.Id = 0;
            this.Name = string.Empty;
        }
        public Specialty
            (int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}