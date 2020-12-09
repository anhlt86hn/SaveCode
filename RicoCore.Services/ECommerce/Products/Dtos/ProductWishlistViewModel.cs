using System;
using System.Collections.Generic;
using System.Text;
using RicoCore.Services.Systems.Users.Dtos;

namespace RicoCore.Services.ECommerce.Products.Dtos
{
    public class ProductWishlistViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
