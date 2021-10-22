﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<ItemDto>> GetItems()
        {
            return (await _repository.GetItemsAsync()).Select(x => x.AsDto());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ItemDto>> GetItem(Guid id)
        {
            var item = await _repository.GetItemAsync(id);
            return item is null ? NotFound() : item.AsDto();
        }
        
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto dto)
        {
            var item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto());
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var item = await _repository.GetItemAsync(id);
            
            if (item is null)
                return NotFound();

            var updatedItem = item with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            
            await _repository.UpdateItemAsync(updatedItem);

            return NoContent();
        }

        [HttpDelete("{id:guid}")] 
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            var item = await _repository.GetItemAsync(id);
            
            if (item is null)
                return NotFound();

            await _repository.DeleteItemAsync(item.Id);

            return NoContent();
        }
    }
}