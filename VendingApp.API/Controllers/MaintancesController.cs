using VendingApp.API.Data;
using VendingApp.API.Models;

namespace VendingApp.API.Controllers;

public class MaintenancesController(VendingAppContext db) : BaseController<Maintenance>(db) { }
