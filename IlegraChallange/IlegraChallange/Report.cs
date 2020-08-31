using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

namespace IlegraChallange
{
    public class Report
    {
        public struct SalesmanStatitic
        {
            public Salesman Salesman;
            public decimal Total;
        }

        public List<Salesman> Salesmen { get; set; } = new List<Salesman>();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Sale> Sales { get; set; } = new List<Sale>();
        public Sale GetMostExpensiveSale()
        {
            return Sales
                .OrderByDescending(s => s.Total)
                .FirstOrDefault();
        }
        public SalesmanStatitic GetWorstSalesman()
        {
            var result = Sales
               .GroupBy(s => s.Salesman)
               .Select(s => new
               {
                   Salesman = s.Key,
                   Total = s.Sum(s => s.Total)
               })
               .OrderBy(s => s.Total)
               .FirstOrDefault();

            return new SalesmanStatitic()
            {
                Salesman = Salesmen.FirstOrDefault(s => s.Name.Equals(result.Salesman, StringComparison.InvariantCultureIgnoreCase)),
                Total = result.Total
            };
        }

        public string GetStatistics()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Quantidade de vendedores: {Salesmen.Count}");
            sb.AppendLine($"Quantidade de clientes: {Customers.Count}");

            var mostExpensiveSale = GetMostExpensiveSale();
            sb.AppendLine($"Venda mais cara: {mostExpensiveSale.Id} - valor: {mostExpensiveSale.Total}");

            var worstSalesman = GetWorstSalesman();
            sb.AppendLine($"Pior vendedor: {worstSalesman.Salesman.Name} - CPF: {worstSalesman.Salesman.CPF} - total vendido: {worstSalesman.Total} ");

            return sb.ToString();
        }
    }
}