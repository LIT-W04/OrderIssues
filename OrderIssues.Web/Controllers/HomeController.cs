using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrderIssues.Data;
using OrderIssues.Web.Models;

namespace OrderIssues.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetIncompleteOrders()
        {
            var repo = new OrderIssuesRepo(Properties.Settings.Default.ConStr);
            return Json(repo.GetIncompleOrders().Select(o => new
            {
                id = o.Id,
                title = o.Title,
                date = o.Date,
                completed = o.Completed,
                amount = o.Amount,
                resolvedIssueCount = o.Issues.Count(i => i.Resolved),
                unresolvedIssueCount = o.Issues.Count(i => !i.Resolved)
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewOrder(Order order)
        {
            var repo = new OrderIssuesRepo(Properties.Settings.Default.ConStr);
            order.Date = DateTime.Now;
            repo.AddOrder(order);
            return RedirectToAction("Index");
        }

        public ActionResult ViewIssues(int orderId)
        {
            var vm = new ViewIssuesViewModel { OrderId = orderId };
            return View(vm);
        }

        public ActionResult GetIssuesForOrder(int orderId)
        {
            var repo = new OrderIssuesRepo(Properties.Settings.Default.ConStr);
            var issues = repo.GetUnresolvedIssuesForOrder(orderId);
            if (!issues.Any())
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            Order order = issues.First().Order;
            return Json(new
            {
                order = new
                {
                    title = order.Title,
                    date = order.Date,
                    amount = order.Amount,
                },
                issues = issues.Select(i => new
                {
                    note = i.Note,
                    id = i.Id
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void AddIssue(Issue issue)
        {
            var repo = new OrderIssuesRepo(Properties.Settings.Default.ConStr);
            repo.AddIssue(issue);
        }

        [HttpPost]
        public void ResolveIssue(int issueId)
        {
            var repo = new OrderIssuesRepo(Properties.Settings.Default.ConStr);
            repo.ResolveIssue(issueId);
        }

        [HttpPost]
        public void CompleteOrder(int orderId)
        {
            var repo = new OrderIssuesRepo(Properties.Settings.Default.ConStr);
            repo.CompleteOrder(orderId);
        }
    }
}