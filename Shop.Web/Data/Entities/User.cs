namespace Shop.Web.Data.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser //hereda la tabla AspNetUsers (usuarios)
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
