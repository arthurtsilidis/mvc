using ClientsExercise.Data;
using ClientsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsExercise.Services
{
    public class EntityFrameworkDataProvider : IDataProvider
    {
        private readonly PhonesCatalogueContext _context = null;
        public EntityFrameworkDataProvider()
        {
            _context = new PhonesCatalogueContext();
        }

        public Client CreateClient(Client model)
        {
            using (_context)
            {
                try
                {
                    var client = new Data.Entities.Client
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        Email = model.Email
                    };
                    _context.Clients.Add(client);
                    _context.SaveChanges();

                    return _context.Clients
                                .Where(x => x.Id == client.Id)
                                .Select(x => new Client
                                {
                                    Id = x.Id,
                                    LastName = x.LastName,
                                    FirstName = x.FirstName,
                                    Address = x.Address,
                                    Email = x.Email,
                                    PhoneNumbers = x.ClientPhones.Select(p => new PhoneNumber
                                    {
                                        Type = p.PhoneType.Name,
                                        Number = p.PhoneNumber
                                    }).ToList()
                                }).FirstOrDefault();

                }
                catch (Exception e)
                {
                    e.ToString();
                    throw new Exception(e.Message);
                }
            }
        }

        public bool DeleteClient(int id)
        {
            using (_context)
            {
                using var _ctxTransaction = _context.Database.BeginTransaction();
                try
                {
                    var toDelete = _context.Clients.SingleOrDefault(x => x.Id == id);
                    if (toDelete != null)
                    {
                        _context.Clients.Remove(toDelete);
                        _context.SaveChanges();
                    }
                    _ctxTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    _ctxTransaction.Rollback();
                    e.ToString();
                    return false;
                }
            }
        }

        public IList<Client> GetList()
        {
            try
            {
                using (_context)
                {
                    return _context.Clients.Select(x => new Models.Client
                    {
                        Id = x.Id,
                        LastName = x.LastName,
                        FirstName = x.FirstName,
                        Address = x.Address,
                        Email = x.Email,
                        PhoneNumbers = x.ClientPhones.Select(p => new Models.PhoneNumber
                        {
                            Type = p.PhoneType.Name,
                            Number = p.PhoneNumber
                        }).ToList()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        public Client UpdateClient(Client model)
        {
            return new Client();
        }

    }
}
