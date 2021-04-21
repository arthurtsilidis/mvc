using ClientsExercise.Data;
using ClientsExercise.Data.Entities;
using ClientsExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientsExercise.Services
{
    public class EntityFrameworkDataProvider : IDataProvider
    {
        private readonly PhonesCatalogueContext _context = null;
        public EntityFrameworkDataProvider(PhonesCatalogueContext context)
        {
            _context = context;
        }

        public Models.Client CreateClient(Models.Client model)
        {
            using (_context)
            {
                try
                {
                    var phoneTypes = _context.PhoneTypes.ToList();
                    var clientPhones = from phoneType in phoneTypes
                                       join modelPhoneType in model.PhoneNumbers on phoneType.Name equals modelPhoneType.Type
                                       select new ClientPhones 
                                       {
                                           PhoneTypeId = phoneType.Id,
                                           PhoneNumber = modelPhoneType.Number
                                       };
                    if (model.PhoneNumbers.Any(x => !phoneTypes.Select(p => p.Name).Contains(x.Type)))
                    {
                        throw new Exception("check phone types");
                    }

                    if (!model.PhoneNumbers.Any())
                    {
                        throw new Exception("check phone numbers");
                    }
                    var client = new Data.Entities.Client
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        Email = model.Email,
                        ClientPhones = clientPhones.ToList()
                    };
                    _context.Clients.Add(client);
                    _context.SaveChanges();

                    return _context.Clients
                                .Where(x => x.Id == client.Id)
                                .Select(x => new Models.Client
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

        public IList<Models.Client> GetList()
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

        public Models.Client UpdateClient(Models.Client model)
        {
            using (_context)
            {
                using var _ctxTransaction = _context.Database.BeginTransaction();
                try
                {
                    var entity = _context.Clients.FirstOrDefault(x => x.Id == model.Id);
                    if (entity == null )
                    {
                        throw new Exception("nothing to update");
                    }

                    if (!model.PhoneNumbers.Any())
                    {
                        throw new Exception("check phone numbers");
                    }

                    var phoneTypes = _context.PhoneTypes.ToList();
                    var clientPhones = from phoneType in phoneTypes
                                       join modelPhoneType in model.PhoneNumbers on phoneType.Name equals modelPhoneType.Type
                                       select new ClientPhones
                                       {
                                           PhoneTypeId = phoneType.Id,
                                           PhoneNumber = modelPhoneType.Number
                                       };
                    if (model.PhoneNumbers.Any(x => !phoneTypes.Select(p => p.Name).Contains(x.Type)))
                    {
                        throw new Exception("check phone types");
                    }

                    if (entity != null)
                    {
                        entity.FirstName = model.FirstName;
                        entity.LastName = model.LastName;
                        entity.Address = model.Address;
                        entity.Email = model.Email;
                        _context.ClientPhones.RemoveRange(_context.ClientPhones.Where(x => x.ClientId == model.Id));
                        entity.ClientPhones = clientPhones.ToList();
                        _context.SaveChanges();
                    }
                    _ctxTransaction.Commit();
                    return _context.Clients
                               .Where(x => x.Id == model.Id)
                               .Select(x => new Models.Client
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
                    _ctxTransaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

    }
}
