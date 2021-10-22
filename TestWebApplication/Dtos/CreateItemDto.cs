using System;
using System.ComponentModel.DataAnnotations;

namespace TestWebApplication.Dtos
{
    public record CreateItemDto
    {
        [Required]
        public string Name { get; init; }
        
        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal Price { get; init; }
    }
}