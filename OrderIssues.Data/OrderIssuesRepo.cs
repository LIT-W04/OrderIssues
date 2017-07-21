using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderIssues.Data
{
    public class OrderIssuesRepo
    {
        private string _connectionString;

        public OrderIssuesRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Order> GetIncompleOrders()
        {
            using (var context = new OrderIssuesDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Order>(o => o.Issues);
                context.LoadOptions = loadOptions;
                return context.Orders.Where(o => !o.Completed).ToList();
            }
        }

        public void AddOrder(Order order)
        {
            using (var context = new OrderIssuesDataContext(_connectionString))
            {
                context.Orders.InsertOnSubmit(order);
                context.SubmitChanges();
            }
        }

        public IEnumerable<Issue> GetUnresolvedIssuesForOrder(int orderId)
        {
            using (var context = new OrderIssuesDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Issue>(i => i.Order);
                context.LoadOptions = loadOptions;
                return context.Issues.Where(i => i.OrderId == orderId && !i.Resolved).ToList();
            }
        }

        public void AddIssue(Issue issue)
        {
            using (var context = new OrderIssuesDataContext(_connectionString))
            {
                context.Issues.InsertOnSubmit(issue);
                context.SubmitChanges();
            }
        }

        public void ResolveIssue(int issueId)
        {
            using (var context = new OrderIssuesDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Issues SET Resolved = 1 WHERE Id = {0}", issueId);
            }
        }

        public void CompleteOrder(int orderId)
        {
            using (var context = new OrderIssuesDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Orders SET Completed = 1 WHERE Id = {0}", orderId);
            }
        }
    }
}
