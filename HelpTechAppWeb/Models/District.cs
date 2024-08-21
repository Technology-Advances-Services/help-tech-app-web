namespace HelpTechAppWeb.Models
{
    public class District
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }

        public District()
        {
            this.Id = 0;
            this.DepartmentId = 0;
            this.Name = string.Empty;
        }
        public District
            (int id, int departmentId,
            string name)
        {
            this.Id = id;
            this.DepartmentId = departmentId;
            this.Name = name;
        }
    }
}