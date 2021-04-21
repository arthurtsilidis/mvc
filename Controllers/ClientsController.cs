using ClientsExercise.Data;
using ClientsExercise.Data.Entities;
using ClientsExercise.Filters;
using ClientsExercise.Models;
using ClientsExercise.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        public ClientsController(IDataProvider sender, PhonesCatalogueContext dbContext)
        {
            _context = dbContext;
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
            try
            {
                return await Task.Run(() => _dataProvider.CreateClient(model));
            }
            catch 
            {
                throw new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/clients")]
        public async Task<object> UpdateClient([FromBody] Models.Client model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return await Task.Run(() => _dataProvider.UpdateClient(model));
            }
            catch
            {
                throw new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
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
