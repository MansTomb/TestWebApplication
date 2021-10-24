using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            _logger.LogInformation("Retrieving items...");
            return (await _repository.GetItemsAsync()).Select(x => x.AsDto());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _repository.GetItemAsync(id);
            if (item is null)
                return NotFound();
            return item.AsDto();
        }
        
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto dto)
        {
            var item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _repository.CreateItemAsync(item);

            // ReSharper disable once Mvc.ActionNotResolved
            return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id}, item.AsDto());
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
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
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var item = await _repository.GetItemAsync(id);
            
            if (item is null)
                return NotFound();

            await _repository.DeleteItemAsync(item.Id);

            return NoContent();
        }
    }
}