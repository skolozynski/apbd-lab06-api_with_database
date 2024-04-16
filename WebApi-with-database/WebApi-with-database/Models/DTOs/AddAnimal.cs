﻿using System.ComponentModel.DataAnnotations;

namespace WebApi_with_database.Models.DTOs;

public class AddAnimal
{
    [Required]
    [MinLength(3)]
    [MaxLength(200)]
    public string Name { get; set; }
    [MinLength(3)]
    [MaxLength(200)]
    public string? Description { get; set; }
    [MinLength(3)]
    [MaxLength(200)]
    public string Category { get; set; }
    [MinLength(3)]
    [MaxLength(200)]
    public string Area { get; set; }
}