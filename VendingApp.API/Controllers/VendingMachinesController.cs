using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VendingApp.API.Data;
using VendingApp.API.Models;

namespace VendingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendingMachinesController(VendingAppContext db) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Получение списка всех аппаратов")]
    public ActionResult<List<VendingMachine>> GetMachines()
    {
        var machines = db.VendingMachines.ToList();
        return Ok(machines);
    }

    [HttpGet("on")]
    [SwaggerOperation(Summary = "Получение списка работающих аппаратов")]
    public ActionResult<List<VendingMachine>> GetMachinesOn()
    {
        var machinesOn = db
            .VendingMachines.Where(m => m.Status == VendingMachineStatus.On)
            .ToList();
        return Ok(machinesOn);
    }

    [HttpGet("off")]
    [SwaggerOperation(Summary = "Получение списка не работающих аппаратов")]
    public ActionResult<List<VendingMachine>> GetMachinesOff()
    {
        var machinesOn = db
            .VendingMachines.Where(m => m.Status == VendingMachineStatus.Off)
            .ToList();
        return Ok(machinesOn);
    }

    [HttpGet("service")]
    [SwaggerOperation(Summary = "Получение списка аппаратов в сервисе")]
    public ActionResult<List<VendingMachine>> GetMachinesService()
    {
        var machinesOn = db
            .VendingMachines.Where(m => m.Status == VendingMachineStatus.Service)
            .ToList();
        return Ok(machinesOn);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Добавление автомата")]
    public ActionResult<VendingMachine> CreateMachine()
    {
        return Created("", new VendingMachine());
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Редактирование автомата")]
    public ActionResult<VendingMachine> EditMachine()
    {
        return Ok();
    }

    [HttpPost("detach/{id}")]
    [SwaggerOperation(Summary = "Отвязка автомата от модема")]
    public ActionResult<bool> DetachMachineFromModem()
    {
        return Ok(true);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Удаление автомата")]
    public ActionResult<VendingMachine> RemoveMachine()
    {
        return Ok();
    }
}
