using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.DataAccess;
using ShopApi.DTOs;
using ShopApi.Entities;
using ShopApi.Repositories.IRepositories;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProduct()
        {
            return await _unitOfWork.ProductRepository.GetAll();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.Find(x=>x.Id==id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _unitOfWork.ProductRepository.Update(product);

            if (await _unitOfWork.Complete()){return NoContent();}
            return BadRequest("Failed to update task");
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _unitOfWork.ProductRepository.Add(product);
            if (await _unitOfWork.Complete()) {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product); 
            }
            return BadRequest("Failed to update task");

            
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.FindOne(id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductRepository.Remove(product);
            if (await _unitOfWork.Complete()) { return NoContent(); }
            return BadRequest("Failed to update task");

        }

        private bool ProductExists(int id)
        {
            if( _unitOfWork.ProductRepository.FindOne(id) != null)
            {
                return true;
            }
            return false;
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var product = await _unitOfWork.ProductRepository.Find(x=>x.Id == id);
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) 
                return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (product.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            
            product.Photos.Add(photo);
            
            if (await _unitOfWork.Complete())
            {
                return _unitOfWork.Map<PhotoDto> photo; //sprawdzic jak to zrobić
            }

                return BadRequest("Problem adding photo");
            
        }

    }
}
