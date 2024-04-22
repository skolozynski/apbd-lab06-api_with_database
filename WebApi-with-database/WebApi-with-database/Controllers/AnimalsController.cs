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
    public IActionResult GetAnimals(string? orderBy)
    {
        orderBy ??= "name";

        var values = new List<string> { "name", "description", "category", "area" };

        if (!values.Contains(orderBy))
            return NotFound($"Parameter '{orderBy}' does not exist in table Animal");

        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = $"SELECT * FROM Animal ORDER BY {orderBy} ASC";

        var reader = command.ExecuteReader();

        var animals = new List<Animal>();

        var idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        var nameOrdinal = reader.GetOrdinal("Name");
        var descriptionOrdinal = reader.GetOrdinal("Description");
        var categoryOrdinal = reader.GetOrdinal("Category");
        var areaOrdinal = reader.GetOrdinal("Area");
        
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal),
                Description = reader.GetString(descriptionOrdinal),
                Category = reader.GetString(categoryOrdinal),
                Area = reader.GetString(areaOrdinal)
            });
        }

        // _repository.AddAnimal(addAnimal); 
        
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal addAnimal)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Animal VALUES (@animalName, @description, @category, @area)";
        command.Parameters.AddWithValue("@animalName", addAnimal.Name);
        command.Parameters.AddWithValue("@description", addAnimal.Description);
        command.Parameters.AddWithValue("@category", addAnimal.Category);
        command.Parameters.AddWithValue("@area", addAnimal.Area);

        command.ExecuteNonQuery();
        
        return Created("", null);
    }

    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, AddAnimal addAnimal)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = $"SELECT * FROM Animal WHERE IdAnimal = {idAnimal}";
        if (command.ExecuteScalar() == null)
            return NotFound($"Animal with id = '{idAnimal}' not found");

        command.CommandText = $"UPDATE Animal SET Name = @animalName, Description = @description, " +
                              $"Category = @category, Area = @area WHERE IdAnimal = {idAnimal}";
        command.Parameters.AddWithValue("@animalName", addAnimal.Name);
        command.Parameters.AddWithValue("@description", addAnimal.Description);
        command.Parameters.AddWithValue("@category", addAnimal.Category);
        command.Parameters.AddWithValue("@area", addAnimal.Area);

        command.ExecuteNonQuery();

        return Ok($"Animal with id = '{idAnimal}' has been updated");
    }

    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = $"SELECT * FROM Animal WHERE IdAnimal = {idAnimal}";
        if (command.ExecuteScalar() == null)
            return NotFound($"Animal with id = '{idAnimal}' not found");

        command.CommandText = $"DELETE FROM Animal WHERE IdAnimal = {idAnimal}";

        command.ExecuteNonQuery();

        return Ok($"Successfully deleted animal with id = '{idAnimal}'");
    }
    
}