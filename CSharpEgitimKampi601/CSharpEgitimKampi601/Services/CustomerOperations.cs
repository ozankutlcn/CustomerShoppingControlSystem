using CSharpEgitimKampi601.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEgitimKampi601.Services
{
    public class CustomerOperations
    {
      public void AddCustomer(Customer customer)
      {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();

            var document = new BsonDocument 
            {
                {"CustomerName", customer.CustomerName },
                { "CustomerSurname", customer.CustomerSurname },
                { "CustomerCity", customer.CustomerCity },
                { "CustomerBalance", customer.CustomerBalance },
                { "CustomerShoppingCount", customer.CustomerShoppingCount }
            };

            customerCollection.InsertOne(document); //Insert işlemi yapılır.
      }

        public List<Customer> GetAllCustomer() 
        {
            var connection = new MongoDbConnection(); //Burada MongoDB bağlantısı oluşturulur.
            var customerCollection = connection.GetCustomersCollection(); //Burada Customers Collection'ı alınır.
            var customers = customerCollection.Find(new BsonDocument()).ToList(); //Burada Customers Collection'ı içerisindeki tüm kayıtlar hafızaya alınır.
            List<Customer> customerList = new List<Customer>(); //Boş bir Customer listesi oluşturulur.
            foreach (var c in customers)
            {
                customerList.Add(new Customer //Her bir kayıt Customer tipine dönüştürülerek Customer Listesine eklenir.
                {
                    CustomerId = c["_id"].ToString(),
                    CustomerBalance = decimal.Parse(c["CustomerBalance"].ToString()),
                    CustomerCity = c["CustomerCity"].ToString(),
                    CustomerName = c["CustomerName"].ToString(),
                    CustomerShoppingCount = int.Parse(c["CustomerShoppingCount"].ToString()),
                    CustomerSurname = c["CustomerSurname"].ToString()
                });
            }
            return customerList;
        }

        public void DeleteCustomer(string id)
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            customerCollection.DeleteOne(filter); //Delete işlemi yapılır.
        }

        public void UpdateCustomer(Customer customer)
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(customer.CustomerId));
            var updatedValue = Builders<BsonDocument>.Update
                .Set("CustomerName",customer.CustomerName)
                .Set("CustomerSurname", customer.CustomerSurname)
                .Set("CustomerCity", customer.CustomerCity)
                .Set("CustomerBalance", customer.CustomerBalance)
                .Set("CustomerShoppingCount", customer.CustomerShoppingCount);
            customerCollection.UpdateOne(filter, updatedValue); //Update işlemi yapılır.
        }

        public Customer GetCustomerById(string id)
        {
            var connection = new MongoDbConnection();
            var customerCollection = connection.GetCustomersCollection();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var customer = customerCollection.Find(filter).FirstOrDefault();
            return new Customer
            {
                CustomerId = customer["_id"].ToString(),
                CustomerBalance = decimal.Parse(customer["CustomerBalance"].ToString()),
                CustomerCity = customer["CustomerCity"].ToString(),
                CustomerName = customer["CustomerName"].ToString(),
                CustomerShoppingCount = int.Parse(customer["CustomerShoppingCount"].ToString()),
                CustomerSurname = customer["CustomerSurname"].ToString()
            };
        }
    }
}
