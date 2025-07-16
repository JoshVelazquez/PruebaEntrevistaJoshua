using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaEntrevistaJoshua.DTOs;
using PruebaEntrevistaJoshua.Models;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace PruebaEntrevistaJoshua.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : Controller
    {
        private readonly RetoentrevistaContext _context;

        public ProductosController(RetoentrevistaContext context) {
            _context = context;
        }

        /// <summary>
        /// Api para listar todos los productos
        /// </summary>
        /// <returns>Listado de productos</returns>
        /// <response code="200">Listado de los productos</response>
        /// <response code="500">Error</response>
        [HttpGet]
        [Route("ListarTodosLosProductos")]
        public async Task<IActionResult> ListarTodosLosProductos()
        {
            try
            {
                var headers = Request.Headers;
                if (headers.TryGetValue("Authorization", out var authorizathion))
                {
                    var auth = authorizathion.ToString();
                    if (auth != "1Hm!&^U4fiyeYhmY3Tk1HyFWE0@WM1&t" || string.IsNullOrEmpty(auth))
                    {
                        return BadRequest(new { Error = "Falta autorizacion" });
                    }

                } else
                {
                    return BadRequest(new { Error = "Falta autorizacion" });
                }
                var listado = await _context.Productos.ToListAsync();
                var listadoProductos = new List<ProductoDTO>();
                foreach (var item in listado) {
                    var productoDTO = new ProductoDTO { 
                        Id = item.Id,
                        Nombre = item.Nombre,
                        Precio = item.Precio,
                    };
                    listadoProductos.Add(productoDTO);
                }
                return Ok(listadoProductos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Api para guardar un producto
        /// </summary>
        /// <returns>Producto guardado</returns>
        /// <response code="200">Api guardada</response>
        /// <response code="400">Precio mas bajo de 10</response>
        /// <response code="500">Error</response>
        [HttpPost]
        [Route("GuardarProducto")]
        public async Task<IActionResult> GuardarProducto([FromBody] ProductoDTO productoDTO)
        {
            try
            {
                var headers = Request.Headers;

                if (headers.TryGetValue("Authorization", out var authorizathion))
                {
                    var auth = authorizathion.ToString();
                    if (auth != "1Hm!&^U4fiyeYhmY3Tk1HyFWE0@WM1&t" || string.IsNullOrEmpty(auth))
                    {
                        return BadRequest(new { Error = "Falta autorizacion" });
                    }

                }
                else
                {
                    return BadRequest(new { Error = "Falta autorizacion" });
                }
                if (productoDTO.Precio < 0)
                {
                    return BadRequest(new { Error = "Los productos deben de costar mas de 0" });
                }
                if(productoDTO.Nombre.Length > 100)
                {
                    return BadRequest(new { Error = "Los nombres no pueden tener un largo mayor a 100" });
                }

                var producto = new Producto
                {
                    Nombre = productoDTO.Nombre,
                    Precio = productoDTO.Precio,
                };

                await _context.AddAsync(producto);
                await _context.SaveChangesAsync();
                productoDTO.Id = producto.Id;
                return Ok(productoDTO);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
