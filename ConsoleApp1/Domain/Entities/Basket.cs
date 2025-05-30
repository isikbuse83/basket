using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.Features;

namespace ConsoleApp1.Domain.Entities
{
    public class Basket
    {
        [Key] public int Id { get; private set; }

        public int UserId { get; private set; }
        public User User { get; }

        private readonly List<BasketItem> _items = new();
        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        public Basket(int userId)
        {
            UserId = userId;
        }
        
        public void AddBasketItem(BasketItem basketItem) => _items.Add(basketItem);
        
        public void ClearBasket() => _items.Clear();
        
        public void RemoveBasketItem(BasketItem item)
        {
            _items.Remove(item);
        }
    }
}