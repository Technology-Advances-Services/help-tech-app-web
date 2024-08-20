namespace HelpTechAppWeb.Models
{
    public class Department
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Department()
        {
            this.Id = 0;
            this.Name = string.Empty;
        }
        public Department
            (int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}