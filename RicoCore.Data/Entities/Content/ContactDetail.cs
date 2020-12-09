using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("ContactDetails")]
    public class ContactDetail : DomainEntity<int>
    {
        public ContactDetail()
        {
        }

        public ContactDetail(string name, string phone, string email,
            string website, string address, string other, double? longtitude, double? latitude, Status status)
        {           
            Name = name;
            Phone = phone;
            Email = email;
            Website = website;
            Address = address;
            Other = other;
            Lng = longtitude;
            Lat = latitude;
            Status = status;
        }

        [MaxLength(100)]
        [Required]
        public string Name { set; get; }

        [MaxLength(50)]
        public string Phone { set; get; }

        [MaxLength(100)]
        public string Email { set; get; }

        [MaxLength(100)]
        public string Website { set; get; }

        [MaxLength(500)]
        public string Address { set; get; }

        public string Other { set; get; }

        public double? Lat { set; get; }

        public double? Lng { set; get; }

        public Status Status { set; get; }
    }
}