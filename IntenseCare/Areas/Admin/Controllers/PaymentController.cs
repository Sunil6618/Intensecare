﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntenseCare.Models;

namespace IntenseCare.Areas.Admin.Controllers
{
    public class PaymentController : Controller
    {
        AppointmentEntities10 dc = new AppointmentEntities10();
        // GET: Admin/Payment
        public ActionResult Index()
        {
            if (Session["loginid"] != null)
            {
                var patientdocpayment = from ob in dc.tblPayments
                                     join ob2 in dc.tblDoctors on ob.DoctorId equals ob2.DoctorId
                                     join ob3 in dc.tblPatients on ob.PatientId equals ob3.PatientId
                                     join ob4 in dc.tblAdmitDetails on ob.AdmitDetailId equals ob4.AdmitDetailId 
                                     select new Datamodel
                                     {
                                         payment = ob,
                                         Doctor = ob2,
                                         Patient = ob3,
                                         Admit = ob4,
                                     };
                return View(patientdocpayment);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
        public ActionResult Payadd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Payadd(FormCollection form)
        {
            tblPayment ad = new tblPayment();
            ad.PatientId = Convert.ToInt32(form["patientid"]);
            ad.DoctorId = Convert.ToInt32(form["doctorid"]);
            if (form["admitId"] == "")
            {
                ad.AdmitDetailId = 0;
            }
            else
            {
                ad.AdmitDetailId = Convert.ToInt32(form["admitId"]);
            }
            ad.PaymentAmt = Convert.ToInt32(form["payamt"]);
            ad.TransactionId = form["tranid"];
            ad.TransactionType = form["trantype"];
            if (form["chno"] == "")
            {
                ad.ChequeNo = 0;
            }
            else { 
                ad.ChequeNo = Convert.ToInt32(form["chno"]);
            }
            ad.CardType = form["Cardtype"];
            ad.PaidOn = DateTime.Now;
            dc.tblPayments.Add(ad);
            dc.SaveChanges();
            return RedirectToAction("Index", "Payment");
        }
        public ActionResult Details(int id)
        {
            tblPayment  ad = dc.tblPayments.SingleOrDefault(ob => ob.PaymentId == id);
            tblPatient patient = (from ob2 in dc.tblPatients where ob2.PatientId == ad.PatientId select ob2).Take(1).SingleOrDefault();
            tblDoctor doctor = (from ob1 in dc.tblDoctors where ob1.DoctorId == ad.DoctorId select ob1).Take(1).SingleOrDefault();
            ViewBag.PatientName = patient.FirstName + " " + patient.LastName;
            ViewBag.DoctorName = "Dr." + doctor.FirstName + " " + doctor.LastName;
            return View(ad);
        }
    }
}