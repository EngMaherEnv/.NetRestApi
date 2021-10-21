using System;
using catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using catalog.Entities;
using catalog.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace catalog.Conrollers
{
    //Get / ITEMS 
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepoitory repository;
        public ItemsController(IItemsRepoitory repository)
        {
            this.repository = repository;
        }


        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync()).Select(item => item.AsDto());
            return items;
        }

        // get /item/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        // post /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreateDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        // put/items/id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {

            var existingItem =await repository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            await repository.UpdateItemAsync(updatedItem);
            return NoContent();

        }

        //Delete /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await repository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            await repository.DeleteItemAsync(id);
            return NoContent();

        }

    }
}