using CommonDomain;
using Persistence.Repository;
using Persistence.Service.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Service
{
    public class SaleService
    {
        private SaleRepository repository;
        private SaleValidator validator;

        public SaleService(SaleRepository repository)
        {
            this.repository = repository;
            this.validator = new SaleValidator();
        }

        public void AddSale(string buyerName, int ticketsBought, int showId)
        {
            Sale sale = new Sale(buyerName, ticketsBought, showId);
            validator.validate(sale);
            repository.Add(sale);
        }
    }
}