using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestWebApplication.Entities;

namespace TestWebApplication.Repositories
{
    public class InMemItemsRepository : IItemsRepository
    {
        private readonly List<Item> _items = new()
        {
            new Item() {Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow},
            new Item() {Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow},
            new Item() {Id = Guid.NewGuid(), Name = "Bronze Shielb", Price = 18, CreatedDate = DateTimeOffset.UtcNow}
        };

        public IEnumerable<Item> GetItems()
        {
            return _items;
        }

        public Item GetItem(Guid id) => _items.FirstOrDefault(x => x.Id == id);
        
        public void CreateItem(Item item)
        {
            _items.Add(item);
        }

        public void UpdateItem(Item item)
        {
            var index = _items.FindIndex(x => x.Id == item.Id);
            _items[index] = item;
        }

        public void DeleteItem(Guid id)
        {
            var index = _items.FindIndex(x => x.Id == id);
            _items.RemoveAt(index);
        }
    }
}