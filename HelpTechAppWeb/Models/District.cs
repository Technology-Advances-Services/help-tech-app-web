namespace HelpTechAppWeb.Models
{
    public class District
    {
        public int Id { get; private set; }
        public int DepartmentId { get; private set; }
        public string Name { get; private set; }

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