using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using APIPRUEBAS.Models;
using Microsoft.AspNetCore.Cors;

namespace APIPRUEBAS.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly DbapiContext _dbcontext;

        public ProductoController(DbapiContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        //Listar
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                lista = _dbcontext.Productos.Include(c => c.categoria).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista});
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message, response = lista });
            }
        }

        //Obtener
        [HttpGet]
        [Route("Obtener/{IdProducto:int}")]
        public IActionResult Obtener(int IdProducto)
        {
            Producto producto = _dbcontext.Productos.Find(IdProducto);

            if (producto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                producto = _dbcontext.Productos.Include(c => c.categoria).Where(p => p.IdProducto == IdProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = producto });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message, response = producto });
            }
        }

        //Guardar
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto producto)
        {
            try
            {
                _dbcontext.Productos.Add(producto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message });
            }
        }

        //Editar
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto eproducto)
        {
            Producto producto = _dbcontext.Productos.Find(eproducto.IdProducto);

            if (producto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                producto.CodigoBarra = eproducto.CodigoBarra is null ? producto.CodigoBarra : eproducto.CodigoBarra;
                producto.Descripcion = eproducto.Descripcion is null ? producto.Descripcion : eproducto.Descripcion;
                producto.Marca = eproducto.Marca is null ? producto.Marca : eproducto.Marca;
                producto.IdCategoria = eproducto.IdCategoria == 0 ? producto.IdCategoria : eproducto.IdCategoria;
                producto.Precio = eproducto.Precio == 0 ? producto.Precio : eproducto.Precio;

                _dbcontext.Update(producto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message });
            }
        }

        //Eliminar
        [HttpDelete]
        [Route("Eliminar/{IdProducto:int}")]
        public IActionResult Eliminar(int IdProducto)
        {
            Producto producto = _dbcontext.Productos.Find(IdProducto);

            if (producto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                _dbcontext.Productos.Remove(producto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message });
            }
        }

    }
}
