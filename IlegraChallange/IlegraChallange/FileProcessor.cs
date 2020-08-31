using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace IlegraChallange
{
    public static class FileProcessor
    {
        public static async Task ProcessAsync(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            var report = await Task.Run(() => ProduceReport(filePath));
            if (report is null)
            {
                Console.WriteLine($"Arquivo {fileName} - está sendo usado por outro processo ou é inválido.");
            }
            else
            {
                Console.WriteLine($"Arquivo {fileName} - lido");
                await Task.Run(() => GenerateReportFile(report, string.Concat("resultado - ", fileName)));
            }
        }

        private static Report ProduceReport(string filePath)
        {
            try
            {
                var report = new Report();

                using var reader = new StreamReader(filePath);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var register = line.Split('ç');
                    switch (Convert.ToInt32(register[0]))
                    {
                        case 1:
                            var salesman = new Salesman
                            {
                                CPF = register[1],
                                Name = register[2],
                                Salary = decimal.Parse(register[3], CultureInfo.InvariantCulture)
                            };

                            if (!report.Salesmen.Exists(s => s.CPF.Equals(salesman.CPF, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                report.Salesmen.Add(salesman);
                            }
                            break;
                        case 2:
                            var customer = new Customer
                            {
                                CNPJ = register[1],
                                Name = register[2],
                                BusinessArea = register[3]
                            };

                            if (!report.Customers.Exists(c => c.CNPJ.Equals(customer.CNPJ, StringComparison.InvariantCultureIgnoreCase)))
                            {
                                report.Customers.Add(customer);
                            }

                            break;
                        case 3:
                            var sale = new Sale
                            {
                                Id = register[1],
                                Salesman = register[3]
                            };

                            if (!report.Sales.Exists(s => s.Id == sale.Id))
                            {
                                var items = register[2].Replace('[', ' ').Replace(']', ' ').Split(',');
                                foreach (var item in items)
                                {
                                    var reg = item.Split('-');

                                    sale.Items.Add(new Item()
                                    {
                                        Sequence = Convert.ToInt32(reg[0]),
                                        Id = Convert.ToInt32(reg[1]),
                                        Price = decimal.Parse(reg[2], CultureInfo.InvariantCulture)
                                    });
                                }

                                report.Sales.Add(sale);
                            }
                            break;
                    }
                }

                return report;
            }
            catch
            {
                return null;
            }
        }

        private static void GenerateReportFile(Report report, string fileName)
        {
            try
            {
                using StreamWriter file = new StreamWriter(Path.Combine(Utils.GetHomePath(), "data", "out", fileName));
                file.Write(report.GetStatistics());
            }
            catch
            {
            }
        }
    }
}