using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestWebApplication.Dtos;
using TestWebApplication.Entities;
using TestWebApplication.Repositories;

namespace TestWebApplication.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _repository;

        public ItemsController(IItemsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<ItemDto> GetItems() => _repository.GetItems().Select(x => x.AsDto());
        
        [HttpGet("{id:guid}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = _repository.GetItem(id);
            return item is null ? NotFound() : item.AsDto();
        }
        
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto dto)
        {
            var item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            _repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto());
        }
        
        [HttpPut("{id:guid}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var item = _repository.GetItem(id);
            
            if (item is null)
                return NotFound();

            var updatedItem = item with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            
            _repository.UpdateItem(updatedItem);

            return NoContent();
        }

        [HttpDelete("{id:guid}")] 
        public ActionResult DeleteItem(Guid id)
        {
            var item = _repository.GetItem(id);
            
            if (item is null)
                return NotFound();

            _repository.DeleteItem(item.Id);

            return NoContent();
        }
    }
}