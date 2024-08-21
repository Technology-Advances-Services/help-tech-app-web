namespace HelpTechAppWeb.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Policies { get; set; }

        public Membership()
        {
            this.Id = 0;
            this.Name = string.Empty;
            this.Price = 0m;
            this.Policies = string.Empty;
        }
        public Membership
            (int id, string name,
            decimal price, string policies)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Policies = policies;
        }
    }
}