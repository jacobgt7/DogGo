using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DogGo.Models;
using System;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DogGo.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepo;

        public DogsController(IDogRepository dogRepo)
        {
            _dogRepo = dogRepo;
        }

        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.AddDog(dog);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            int currentUserId = GetCurrentUserId();
            
            if (dog == null || dog.OwnerId != currentUserId)
            {
                return NotFound();
            }
            
            return View(dog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Dog dog) 
        {
            try
            {
                _dogRepo.UpdateDog(dog);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            int currentUserId = GetCurrentUserId();

            if (dog == null || dog.OwnerId != currentUserId)
            {
                return NotFound();
            }

            return View(dog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}