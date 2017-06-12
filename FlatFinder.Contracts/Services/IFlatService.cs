using FlatFinder.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlatFinder.Contracts.Services
{
    public interface IFlatService
    {
        Task<Flat> Add(Flat flat);

        Task<SoldFlatsReport> GenerateReport(DateTime from, DateTime to);

        Task<Flat> Get(int id);

        Task<IEnumerable<Flat>> Get();

        Task Remove(int id);

        Task<IEnumerable<Flat>> Search(FlatSearchQuery query);

        Task<Flat> SetAsSold(int id);

        Task Update(Flat flat);
    }
}