using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Logimat.API.Data;
using Logimat.API.Models;

namespace Logimat.API.Endpoints
{
    public static class MaterialEndpoints
    {
        public static void MapMaterialEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/materials"); 

            group.MapGet("/", async (AppDbContext db) =>
            {
                var materials = await db.Materials.ToListAsync();
                return materials.Any() ? Results.Ok(materials) : Results.NoContent();
            });

            group.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
            {
                var material = await db.Materials.FindAsync(id);
                return material is not null ? Results.Ok(material) : Results.NotFound($"Material with ID {id} not found");
            });

            group.MapPost("/", async ([FromBody] Material material, AppDbContext db) =>
            {
                if (string.IsNullOrEmpty(material.Name) || string.IsNullOrEmpty(material.Code))
                {
                    return Results.BadRequest("Name and Code cannot be empty.");
                }

                material.Id = Guid.NewGuid();
                await db.Materials.AddAsync(material);
                await db.SaveChangesAsync();
                return Results.Created($"/materials/{material.Id}", material);
            });

            group.MapPut("/{id:guid}", async (Guid id, [FromBody] Material updatedMaterial, AppDbContext db) =>
            {
                var material = await db.Materials.FindAsync(id);
                if (material is null) return Results.NotFound($"Material with ID {id} not found");

                material.Name = updatedMaterial.Name;
                material.Code = updatedMaterial.Code;
                material.Quantity = updatedMaterial.Quantity;
                material.Description = updatedMaterial.Description;
                material.Price = updatedMaterial.Price;

                await db.SaveChangesAsync();
                return Results.Ok(material);
            });

            // Malzeme Sil
            group.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
            {
                var material = await db.Materials.FindAsync(id);
                if (material is null) return Results.NotFound($"Material with ID {id} not found");

                db.Materials.Remove(material);
                await db.SaveChangesAsync();
                return Results.Ok($"Material with ID {id} deleted");
            });
        }
    }
}
