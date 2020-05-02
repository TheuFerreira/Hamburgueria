using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Hamburgueria.Sql
{
    public class Sale : IDisposable
    {
        private readonly Connection con;

        public Sale()
        {
            con = new Connection();
        }

        public void Insert(DateTime date, decimal totalBruto, decimal desconto, decimal total, string pagamento, List<Item> items)
        {
            Tables.Sale sale = new Tables.Sale(date, totalBruto, desconto, total, pagamento);
            con.Sales.Add(sale);
            con.SaveChanges();

            int saleId = con.Sales.Max(x => x.Id);

            con.SaleTables.Add(new Tables.SaleTable(saleId, -1));
            con.SaveChanges();

            foreach (Item it in items)
            {
                Tables.ProductSale productSale = new Tables.ProductSale(saleId, it.Id, it.Name, it.Price, it.Quantity, it.Total);
                con.ProductSales.Add(productSale);
            }
            con.SaveChanges();
        }

        public void Insert(int numTable, DateTime date, decimal totalBruto, decimal desconto, decimal total, string pagamento, List<Item> items)
        {
            Tables.Sale sale = new Tables.Sale(date, totalBruto, desconto, total, pagamento);
            con.Sales.Add(sale);
            con.SaveChanges();

            int saleId = con.Sales.Max(x => x.Id);

            con.SaleTables.Add(new Tables.SaleTable(saleId, numTable));
            con.SaveChanges();

            foreach (Item it in items)
            {
                Tables.ProductSale productSale = new Tables.ProductSale(saleId, it.Id, it.Name, it.Price, it.Quantity, it.Total);
                con.ProductSales.Add(productSale);
            }
            con.SaveChanges();
        }

        public void Insert(Tables.Client client, DateTime date, decimal totalBruto, decimal desconto, decimal total, string pagamento, List<Item> items)
        {
            Tables.Sale sale = new Tables.Sale(date, totalBruto, desconto, total, pagamento);
            con.Sales.Add(sale);
            con.SaveChanges();

            int saleId = con.Sales.Max(x => x.Id);
            
            con.SaleDeliveries.Add(new Tables.SaleDelivery(saleId, client.Name, client.Street + ", Nº" + client.Number + ", " + client.District + ", " + client.Complement));
            con.SaveChanges();

            foreach (Item it in items)
            {
                Tables.ProductSale productSale = new Tables.ProductSale(saleId, it.Id, it.Name, it.Price, it.Quantity, it.Total);
                con.ProductSales.Add(productSale);
            }
            con.SaveChanges();
        }

        void IDisposable.Dispose()
        {
            con.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
