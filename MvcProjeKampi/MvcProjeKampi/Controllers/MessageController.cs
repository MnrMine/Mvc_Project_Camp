using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
	public class MessageController : Controller
	{
		// GET: Message
		MessageManager cm = new MessageManager(new EFMessageDal());
		MessageValidator messagevalidator = new MessageValidator();
		[Authorize]
		public ActionResult Inbox(string p)
		{
			var messagelist = cm.GetListInbox(p);
			return View(messagelist);
		}

		public ActionResult Sendbox(string p)
		{
			var messagelist = cm.GetListSendbox(p);
			return View(messagelist);
		}

		public ActionResult GetInboxMessageDetails(int id)
		{
			var values = cm.GetByID(id);
			values.MessageStatus = true;
			cm.MessageUpdate(values);
			return View(values);
		}
		public ActionResult GetSendBoxMessageDetails(int id)
		{
			var values = cm.GetByID(id);
			values.MessageStatus = true;
			cm.MessageUpdate(values);
			return View(values);
		}

		[HttpGet]
		public ActionResult NewMessage()
		{
			return View();
		}

		[HttpPost]
		public ActionResult NewMessage(Message p)
		{
			ValidationResult results = messagevalidator.Validate(p);
			if (results.IsValid)
			{
				p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
				cm.MessageAdd(p);
				return RedirectToAction("SendBox");
			}
			else
			{
				foreach (var item in results.Errors)
				{
					ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
				}
			}
			return View();
		}
	}
}