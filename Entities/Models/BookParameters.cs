namespace Entities.Models
{
    public class BookParameters : QueryStringParameters
    {
        //ctor for default orderby option
        public BookParameters()
        {
            OrderBy = "Name";
        }
        //filtering is usually performed using boolean flags or ranges (e.g. max price $100)

        //search input is usually free text manually entered by the user
        public string Name { get; set; }

        public string Author { get; set; }
    }
}
