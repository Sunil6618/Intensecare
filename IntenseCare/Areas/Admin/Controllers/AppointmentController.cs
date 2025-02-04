﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntenseCare.Models;

namespace IntenseCare.Areas.Admin.Controllers
{
    public class AppointmentController : Controller
    {
        AppointmentEntities10 dc = new AppointmentEntities10();
        // GET: Admin/Appointment
        public ActionResult Index()
        {
            if (Session["loginid"] != null)
            {
                var AppointDoctor = from ob in dc.tblAppoinments 
                                   join ob2 in dc.tblDoctors on ob.DoctorID equals ob2.DoctorId
                                   join ob3 in dc.tblPatients on ob.PatientID equals ob3.PatientId
                                    join ob4 in dc.tblDoctorSlots  on ob.DoctorSlotId  equals ob4.DoctorSlotId 
                                    select new Datamodel
                                   {
                                      Appoint = ob,
                                       Doctor = ob2,
                                       Patient = ob3,
                                       Slot = ob4,
                                   };
                return View(AppointDoctor);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
            
        }
        [HttpPost]
        public JsonResult Active(int id)
        {
            tblAppoinment  ad = dc.tblAppoinments.SingleOrDefault(ob => ob.AppointmentID == id);
            if (ad.IsNew == true)
            {
                ad.IsNew = false;
            }
            else
            {
                ad.IsNew = true;
            }
            dc.SaveChanges();
            return Json(ad.IsNew, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int id)
        {
            tblAppoinment  ad = dc.tblAppoinments .SingleOrDefault(ob => ob.AppointmentID  == id);
            tblPatient  patient  = (from ob2 in dc.tblPatients where ob2.PatientId == ad.PatientID select ob2).Take(1).SingleOrDefault();
            tblDoctor doctor = (from ob1 in dc.tblDoctors where ob1.DoctorId == ad.DoctorID select ob1).Take(1).SingleOrDefault();
            ViewBag.PatientName = patient.FirstName +" "+ patient.LastName;
            ViewBag.DoctorName = "Dr." + doctor.FirstName + " " + doctor.LastName;
            tblDoctorSlot slot = dc.tblDoctorSlots.SingleOrDefault(ob => ob.DoctorSlotId == ad.DoctorSlotId);
            ViewBag.DocSlot = slot.StartTime + " to " + slot.EndTime;
            //string name = ViewBag.DoctorName;
            //string pname = ViewBag.patientName;
            return View(ad);
        }
    }
}