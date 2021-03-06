﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarStore.Domain.Abstract;
using CarStore.Domain.Entities;

namespace CarStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        ICarRepository repository;

        public AdminController(ICarRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Cars);
        }

        public ViewResult Edit(int carId)
        {
            Car car = repository.Cars
                .FirstOrDefault(g => g.CarId == carId);
            return View(car);
        }

        [HttpPost]
        public ActionResult Edit(Car car, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    car.ImageMimeType = image.ContentType;
                    car.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(car.ImageData, 0, image.ContentLength);
                }
                repository.SaveCar(car);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", car.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(car);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Car());
        }

        [HttpPost]
        public ActionResult Delete(int carId)
        {
            Car deletedCar = repository.DeleteCar(carId);
            if (deletedCar != null)
            {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена",
                    deletedCar.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
