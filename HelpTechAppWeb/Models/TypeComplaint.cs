namespace HelpTechAppWeb.Models
{
    public class TypeComplaint
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TypeComplaint()
        {
            this.Id = 0;
            this.Name = string.Empty;
        }
        public TypeComplaint
            (int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}