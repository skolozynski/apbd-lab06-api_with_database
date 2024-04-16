using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApi_with_database.Models;
using WebApi_with_database.Models.DTOs;

namespace WebApi_with_database.Controllers;

[ApiController]
// [Route("/api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public IActionResult GetAnimals()
    {
        // Start connection to database
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        // Define command
        using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal";
        
        // Run command QUERY
        var reader = command.ExecuteReader();
        
        List<Animal> animals = new List<Animal>();
        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        int descOrdinal = reader.GetOrdinal("Description");
        int catOrdinal = reader.GetOrdinal("Category");
        int areaOrdinal = reader.GetOrdinal("Area");
        
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal),
                Description = reader.GetString(descOrdinal),
                Category = reader.GetString(catOrdinal),
                Area = reader.GetString(areaOrdinal)
            });
            
        }

        // tak powinno byc
        // var animals = _repository.GetAnimals();
        
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal addAnimal)
    {
        // Start connection to database
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        // Define command
        using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText =
            "INSERT INTO Animal VALUES(@animalName, @animalDescription, @animalCategory, @animalArea)";
        command.Parameters.AddWithValue("@animalName", addAnimal.Name);
        command.Parameters.AddWithValue("@animalDescription", addAnimal.Description);
        command.Parameters.AddWithValue("@animalCategory", addAnimal.Category);
        command.Parameters.AddWithValue("@animalArea", addAnimal.Area);
        
        // Execute Command
        command.ExecuteNonQuery();

        // Tak powinno być
        // _repository.AddAnimal(addAnimal);
        
        return Created("", null);
    }
    
}