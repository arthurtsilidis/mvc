using ClientsExercise.Data;
using ClientsExercise.Data.Entities;
using ClientsExercise.Filters;
using ClientsExercise.Models;
using ClientsExercise.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsExercise.Controllers
{
    public class ClientsController : Controller
    {
        private readonly PhonesCatalogueContext _context = null;
        IDataProvider _dataProvider;
        public ClientsController(IDataProvider sender)
        {
            _context = new PhonesCatalogueContext();
            _dataProvider = sender;
        }

        [HttpGet]
        [Route("api/clients")]
        public async Task<IList<Models.Client>> GetClients()
        {
            return await Task.Run(() => _dataProvider.GetList());
        }

        [HttpPost]
        [Route("api/clients")]
        [ValidateModel]
        public async Task<object> CreateClient([FromBody] Models.Client model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            return await Task.Run(() => _dataProvider.UpdateClient(model));
        }

        [HttpPut]
        [Route("api/clients/{id}")]
        public async Task<object> UpdateClient(int id, [FromBody] Models.Client model)
        {
            object result = null; string message = "";
            if (model == null)
            {
                return BadRequest();
            }
            using (_context)
            {
                using var _ctxTransaction = _context.Database.BeginTransaction();
                try
                {
                    var entityUpdate = _context.Clients.FirstOrDefault(x => x.Id == id);
                    if (entityUpdate != null)
                    {
                        //entityUpdate.Name = model.Name;
                        //entityUpdate.Phone = model.Phone;
                        //entityUpdate.Email = model.Email;

                        await _context.SaveChangesAsync();
                    }
                    _ctxTransaction.Commit();
                    message = "Entry Updated";
                }
                catch (Exception e)
                {
                    _ctxTransaction.Rollback(); e.ToString();
                    message = "Entry Update Failed!!";
                }

                result = new
                {
                    message
                };
            }
            return result;
        }

        [HttpDelete]
        [Route("api/clients/{id}")]
        public async Task<bool> DeleteClient(int id)
        {
            return await Task.Run(() => _dataProvider.DeleteClient(id));
        }

        [HttpGet]
        [Route("api/phoneTypes")]
        public async Task<List<PhoneTypes>> GetPhoneTypes()
        {
            try
            {
                using (_context)
                {
                    return await _context.PhoneTypes.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
    }
}
