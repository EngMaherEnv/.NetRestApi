
using System;

namespace catalog.Dtos
{
   public record ItemDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

        public decimal Price { get; set; }

        public DateTimeOffset CreateDate { get; set; }
        
    }
}