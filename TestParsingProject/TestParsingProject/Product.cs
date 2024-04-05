namespace TestParsingProject
{
    public class Product
    {
        private string? _name;
        private string? _category;
        private string? _brand;
        private float _price;

        public string? Name { get => _name; set => _name = value; }
        public string? Category { get => _category; set => _category = value; }
        public string? Brand { get => _brand; set => _brand = value; }
        public float Price { get => _price; set => _price = value; }
        public Product() 
        {
            Name = "";
            Category = "";
            Brand = "";
            Price = .0f;
        }

        public override string ToString()
        {
            return $"{Name} {Category} {Brand} {Price}";
        }
    }
}
