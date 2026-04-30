using Microsoft.AspNetCore.Mvc;
using VendingApp.API.Data;
using VendingApp.API.Models;

namespace VendingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(VendingAppContext db) : BaseController<Product>(db) { }
