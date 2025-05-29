using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Domain.Entities;

namespace ConsoleApp1.Domain.Entities
{
    public class Basket
    {
        [Key] public int BasketId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
    }

}