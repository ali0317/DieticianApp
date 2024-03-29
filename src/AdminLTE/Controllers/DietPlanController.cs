﻿using AdminLTE.Data;
using AdminLTE.Models;
using AdminLTE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdminLTE.Controllers
{
    
    public class DietPlanController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailSender _mailService;
        public DietPlanController(ApplicationDbContext dbContext, IEmailSender mailService)
        {
            _dbContext = dbContext;
            _mailService = mailService;
        }
        public IActionResult Index()
        {


            var list = (from a in _dbContext.Profile
                        join b in _dbContext.DietPlan on a.Id equals b.ProfileId
                        select new DietPlanListVM
                        {
                            Id = b.Id,
                            Date = a.Date,
                            Weight = a.Weight,
                            Height = a.Height,
                            Age = a.Age,
                            Place = a.Place,
                            Occupation = a.Occupation,
                            Fats = b.Fats,
                            Carbohydrates = b.Carbohydrates,
                            Proteins = b.Proteins,
                            RequiredCalorieIntake = b.RequiredCalorieIntake,
                            RequiredwaterIntake = b.WaterIntake,
                            CreatedDate = b.CreatedDate,
                            BMI = a.Weight/ (a.Height*a.Height)

                        }).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            if (id != 0)
            {
                Profile obj = _dbContext.Profile.Find(id);
                return View(obj);

            }
            else
            {
                Profile obj = new Profile();
                return View(obj);

            }
            
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            DietPlan model = _dbContext.DietPlan.Where(a => a.Id == Id).FirstOrDefault();
            var a = _dbContext.Profile.Where(a => a.Id == model.ProfileId).FirstOrDefault();
            ViewBag.BMIValue = a.Weight / (a.Height * a.Height);

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(IFormCollection collection)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var userId = claim.Value;
            if (collection["Id"]=="0")
            {
                Profile obj = new Profile();
                obj.Age = Convert.ToString(collection["Age"]);
                obj.Diabetes = Convert.ToString(collection["Diabetes"]);
                obj.Gender = Convert.ToString(collection["Gender"]);
                obj.Height = Convert.ToDecimal(collection["Height"]);
                obj.Hypertension = Convert.ToString(collection["Hypertension"]);
                obj.LIverKidneyDiseases = Convert.ToString(collection["LIverKidneyDiseases"]);
                obj.Occupation = Convert.ToString(collection["Occupation"]);
                obj.OtherDiseases = Convert.ToString(collection["OtherDiseases"]);
                obj.Place = Convert.ToString(collection["Place"]);
                obj.Weight = Convert.ToDecimal(collection["Weight"]);
                obj.WaterIntake = Convert.ToInt32(collection["WaterIntake"]);
                obj.CalorieIntake = Convert.ToString(collection["CalorieIntake"]);
                obj.ReasonForDietPlan = Convert.ToString(collection["ReasonForDietPlan"]);
                obj.UserId = userId;
                obj.Date = DateTime.Now;
                _dbContext.Profile.Add(obj);
                _dbContext.SaveChanges();

               
              int Id =  GenerateDietPlan(obj);

                return RedirectToAction(nameof(Details), new { Id = Id });
                //string ToEmail = _dbContext.Users.Where(a=>Convert.ToString(a.Id)==userId).FirstOrDefault().Email;
                //MailRequest email = new MailRequest();
                //email.Subject = "Diet Plan";
                //email.ToEmail = ToEmail;
                //email.Body = "Diet Plan is Created Successfully you can visit your portal ";
                // SendMail(email);
            }
            else
            {
                Profile obj = _dbContext.Profile.Find(Convert.ToInt32(collection["Id"]));
                obj.Age = Convert.ToString(collection["Age"]);
                obj.Diabetes = Convert.ToString(collection["Diabetes"]);
                obj.Gender = Convert.ToString(collection["Gender"]);
                obj.Height = Convert.ToDecimal(collection["Height"]);
                obj.Hypertension = Convert.ToString(collection["Hypertension"]);
                obj.LIverKidneyDiseases = Convert.ToString(collection["LIverKidneyDiseases"]);
                obj.Occupation = Convert.ToString(collection["Occupation"]);
                obj.OtherDiseases = Convert.ToString(collection["OtherDiseases"]);
                obj.Place = Convert.ToString(collection["Place"]);
                obj.Weight = Convert.ToDecimal(collection["Weight"]);
                obj.WaterIntake = Convert.ToInt32(collection["WaterIntake"]);
                obj.CalorieIntake = Convert.ToString(collection["CalorieIntake"]);
                obj.ReasonForDietPlan = Convert.ToString(collection["ReasonForDietPlan"]);
                obj.UserId = userId;
                obj.Date = DateTime.Now;
                _dbContext.Profile.Update(obj);
                _dbContext.SaveChanges();

                var deletePreviousPlan = _dbContext.DietPlan.Where(a => a.ProfileId==obj.Id).FirstOrDefault();

                if (deletePreviousPlan != null) { 
                _dbContext.DietPlan.Remove(deletePreviousPlan);
                _dbContext.SaveChanges();
                }

                int Id = GenerateDietPlan(obj); 
               
               
                return RedirectToAction(nameof(Details), new { Id = Id });
            }
            
           
        }



        public int GenerateDietPlan(Profile instance)
        {
            int modelId = 0;
            if(instance.ReasonForDietPlan== "Fitness")
            {
                if(instance.Gender== "male")
                {
                    if(Convert.ToInt32(instance.Age)>=19 && Convert.ToInt32(instance.Age) <= 30)
                    {
                    modelId=    FitenessMale1(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 31 && Convert.ToInt32(instance.Age) <= 59)
                    {
                        modelId = FitenessMale2(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 60)
                    {
                        modelId = FitenessMale3(instance);
                    }


                }
                else if(instance.Gender == "female")
                {
                    if (Convert.ToInt32(instance.Age) >= 19 && Convert.ToInt32(instance.Age) <= 30)
                    {
                        modelId = FitenessFemale1(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 31 && Convert.ToInt32(instance.Age) <= 59)
                    {
                        modelId = FitenessFemale2(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 60)
                    {
                        modelId = FitenessFemale3(instance);
                    }
                }

            }
            if (instance.ReasonForDietPlan == "WeightLoss")
            {
                if (instance.Gender == "male")
                {
                    if (Convert.ToInt32(instance.Age) >= 19 && Convert.ToInt32(instance.Age) <= 30)
                    {
                        modelId = WeightLossMale1(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 31 && Convert.ToInt32(instance.Age) <= 59)
                    {
                        modelId = WeightLossMale2(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 60)
                    {
                        modelId = WeightLossMale3(instance);
                    }


                }
                else if (instance.Gender == "female")
                {
                    if (Convert.ToInt32(instance.Age) >= 19 && Convert.ToInt32(instance.Age) <= 30)
                    {
                        modelId = WeightLossFemale1(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 31 && Convert.ToInt32(instance.Age) <= 59)
                    {
                        modelId = WeightLossFemale2(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 60)
                    {
                        modelId = WeightLossFemale3(instance);
                    }
                }

            }
            if (instance.ReasonForDietPlan == "MuscleGain")
            {

                if (instance.Gender == "male")
                {
                    if (Convert.ToInt32(instance.Age) >= 19 && Convert.ToInt32(instance.Age) <= 30)
                    {
                        modelId = MuscleGainMale1(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 31 && Convert.ToInt32(instance.Age) <= 59)
                    {
                        modelId = MuscleGainMale2(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 60)
                    {
                        modelId = MuscleGainMale3(instance);
                    }


                }
                else if (instance.Gender == "female")
                {
                    if (Convert.ToInt32(instance.Age) >= 19 && Convert.ToInt32(instance.Age) <= 30)
                    {
                        modelId = MuscleGainFemale1(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 31 && Convert.ToInt32(instance.Age) <= 59)
                    {
                        modelId = MuscleGainFemale2(instance);
                    }
                    if (Convert.ToInt32(instance.Age) >= 60)
                    {
                        modelId = MuscleGainFemale3(instance);
                    }
                }
            }


            return modelId;
        }

        public int FitenessMale1(Profile instance) {
            var RequiredCalorieIntake = 3000; // Daily Calorie Intake for this category

            var Carbs = (RequiredCalorieIntake * 55) / 100;

            var Proteins = (RequiredCalorieIntake * 20) / 100;

            var Fats = (RequiredCalorieIntake * 25) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;

            var PerDayWaterIntake = 15;

            ///Save These Information

            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {
                GramsInCarbs = (GramsInCarbs * 3) / 4;
                GramsInProteins = (GramsInProteins * 5) / 4;
            }

           




            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;






        }
        public int FitenessMale2(Profile instance)
        {
            var RequiredCalorieIntake = 2500;// Daily Calorie Intake for this category

            var Carbs = (RequiredCalorieIntake * 55) / 100;

            var Proteins = (RequiredCalorieIntake * 20) / 100;

            var Fats = (RequiredCalorieIntake * 25) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;
            var PerDayWaterIntake = 15;


            if (instance.Diabetes == "Yes" && instance.Hypertension == "No") {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;
               
            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }



            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;


        }
        public int FitenessMale3(Profile instance)
        {
            var RequiredCalorieIntake = 2000;// Daily Calorie Intake for this category

            var Carbs = (RequiredCalorieIntake * 55) / 100;

            var Proteins = (RequiredCalorieIntake * 20) / 100;

            var Fats = (RequiredCalorieIntake * 25) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;
            var PerDayWaterIntake = 15;


            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }


            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }

        public int FitenessFemale1(Profile instance)
        {
            var RequiredCalorieIntake = 2400;// Daily Calorie Intake for this category
            var Carbs = (RequiredCalorieIntake * 55) / 100;

            var Proteins = (RequiredCalorieIntake * 20) / 100;

            var Fats = (RequiredCalorieIntake * 25) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;

            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {
                GramsInCarbs = (GramsInCarbs * 3) / 4;
                GramsInProteins = (GramsInProteins * 5) / 4;
            }

            var PerDayWaterIntake = 12;

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.UserId = instance.UserId;
            model.CreatedDate = DateTime.Now;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }
        public int FitenessFemale2(Profile instance)
        {
            var RequiredCalorieIntake = 2000;

            var Carbs = (RequiredCalorieIntake * 55) / 100;

            var Proteins = (RequiredCalorieIntake * 20) / 100;

            var Fats = (RequiredCalorieIntake * 25) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;
            var PerDayWaterIntake = 12;

            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;

            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }
           

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.UserId = instance.UserId;
            model.CreatedDate = DateTime.Now;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }
        public int FitenessFemale3(Profile instance)
        {
            var RequiredCalorieIntake = 1600;

            var Carbs = (RequiredCalorieIntake * 55) / 100;

            var Proteins = (RequiredCalorieIntake * 20) / 100;

            var Fats = (RequiredCalorieIntake * 25) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;
            var PerDayWaterIntake = 12;

            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }
           

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }

        public int MuscleGainMale1(Profile instance)
        {
            var RequiredCalorieIntake = 3000+500; // Daily Calorie Intake for this category

            var Carbs = (RequiredCalorieIntake * 58) / 100;

            var Proteins = (RequiredCalorieIntake * 25) / 100;

            var Fats = (RequiredCalorieIntake * 17) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;
            var PerDayWaterIntake = 12;
            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {
                GramsInCarbs = (GramsInCarbs * 3) / 4;
                GramsInProteins = (GramsInProteins * 5) / 4;
            }

            ///Save These Information

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();

            return model.Id;





        }
        public int MuscleGainMale2(Profile instance)
        {
            var RequiredCalorieIntake = 2500 + 500;// Daily Calorie Intake for this category


            var Carbs = (RequiredCalorieIntake * 58) / 100;

            var Proteins = (RequiredCalorieIntake * 25) / 100;

            var Fats = (RequiredCalorieIntake * 17) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;
            var PerDayWaterIntake = 12;




            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;

            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }



            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;


        }
        public int MuscleGainMale3(Profile instance)
        {
            var RequiredCalorieIntake = 2000 + 500;// Daily Calorie Intake for this category

            var Carbs = (RequiredCalorieIntake * 58) / 100;

            var Proteins = (RequiredCalorieIntake * 25) / 100;

            var Fats = (RequiredCalorieIntake * 17) / 100;
            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;
            var PerDayWaterIntake = 12;



            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }

        public int MuscleGainFemale1(Profile instance)
        {
            var RequiredCalorieIntake = 2400 + 500;// Daily Calorie Intake for this category

            var Carbs = (RequiredCalorieIntake * 58) / 100;

            var Proteins = (RequiredCalorieIntake * 25) / 100;

            var Fats = (RequiredCalorieIntake * 17) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;

            var PerDayWaterIntake = 10;
            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {
                GramsInCarbs = (GramsInCarbs * 3) / 4;
                GramsInProteins = (GramsInProteins * 5) / 4;
            }


            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }
        public int MuscleGainFemale2(Profile instance)
        {
            var RequiredCalorieIntake = 2000 + 500;

            var Carbs = (RequiredCalorieIntake * 58) / 100;

            var Proteins = (RequiredCalorieIntake * 25) / 100;

            var Fats = (RequiredCalorieIntake * 17) / 100;
            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;

            var PerDayWaterIntake = 10;



            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;

            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }


            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }
        public int MuscleGainFemale3(Profile instance)
        {
            var RequiredCalorieIntake = 1600 + 500;


            var Carbs = (RequiredCalorieIntake * 58) / 100;

            var Proteins = (RequiredCalorieIntake * 25) / 100;

            var Fats = (RequiredCalorieIntake * 17) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;



           

            var PerDayWaterIntake = 10;
            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }
        public int WeightLossMale1(Profile instance)
        {
            var RequiredCalorieIntake = 3000 - 500; // Daily Calorie Intake for this category

            var Carbs = (RequiredCalorieIntake * 20) / 100;

            var Proteins = (RequiredCalorieIntake * 45) / 100;

            var Fats = (RequiredCalorieIntake * 35) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;

            var PerDayWaterIntake = 7;


            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {
                GramsInCarbs = (GramsInCarbs * 3) / 4;
                GramsInProteins = (GramsInProteins * 5) / 4;
            }
            ///Save These Information

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;






        }
        public int WeightLossMale2(Profile instance)
        {
            var RequiredCalorieIntake = 2500 - 500;// Daily Calorie Intake for this category


            var Carbs = (RequiredCalorieIntake * 20) / 100;

            var Proteins = (RequiredCalorieIntake * 45) / 100;

            var Fats = (RequiredCalorieIntake * 35) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;
            var PerDayWaterIntake = 7;



            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;

            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }


            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }
        public int WeightLossMale3(Profile instance)
        {
            var RequiredCalorieIntake = 2000 - 500;// Daily Calorie Intake for this category

            var Carbs = (RequiredCalorieIntake * 20) / 100;

            var Proteins = (RequiredCalorieIntake * 45) / 100;

            var Fats = (RequiredCalorieIntake * 35) / 100;
            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;
            var PerDayWaterIntake = 7;


            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }

        public int WeightLossFemale1(Profile instance)
        {
            var RequiredCalorieIntake = 2400 - 500;// Daily Calorie Intake for this category

            var Carbs = (RequiredCalorieIntake * 20) / 100;

            var Proteins = (RequiredCalorieIntake * 45) / 100;

            var Fats = (RequiredCalorieIntake * 35) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;

            var PerDayWaterIntake = 6;
            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {
                GramsInCarbs = (GramsInCarbs * 3) / 4;
                GramsInProteins = (GramsInProteins * 5) / 4;
            }

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.UserId = instance.UserId;
            model.CreatedDate = DateTime.Now;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }
        public int WeightLossFemale2(Profile instance)
        {
            var RequiredCalorieIntake = 2000 - 500;

            var Carbs = (RequiredCalorieIntake * 20) / 100;

            var Proteins = (RequiredCalorieIntake * 45) / 100;

            var Fats = (RequiredCalorieIntake * 35) / 100;
            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;

            var PerDayWaterIntake = 6;


            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;

            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }


            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();
            return model.Id;

        }
        public int WeightLossFemale3(Profile instance)
        {
            var RequiredCalorieIntake = 1600 - 500;


            var Carbs = (RequiredCalorieIntake * 20) / 100;

            var Proteins = (RequiredCalorieIntake * 45) / 100;

            var Fats = (RequiredCalorieIntake * 35) / 100;

            var GramsInCarbs = Carbs / 4;
            var GramsInProteins = Proteins / 3.8;
            var GramsInFats = Fats / 9;

            var PerDayWaterIntake = 6;



            if (instance.Diabetes == "Yes" && instance.Hypertension == "No")
            {

                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = (GramsInFats * 3) / 4;

            }

            if (instance.Diabetes == "No" && instance.Hypertension == "Yes")
            {
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            if (instance.Diabetes == "Yes" && instance.Hypertension == "Yes")
            {
                GramsInCarbs = GramsInCarbs / 2;
                GramsInFats = GramsInFats / 2;
                PerDayWaterIntake = (PerDayWaterIntake * 3) / 4;
            }

            DietPlan model = new DietPlan();
            model.Carbohydrates = GramsInCarbs;
            model.Proteins = Convert.ToDecimal(GramsInProteins);
            model.Fats = GramsInFats;
            model.WaterIntake = PerDayWaterIntake;
            model.RequiredCalorieIntake = RequiredCalorieIntake;
            model.ProfileId = instance.Id;
            model.CreatedDate = DateTime.Now;
            model.UserId = instance.UserId;
            _dbContext.DietPlan.Add(model);
            _dbContext.SaveChanges();

            return model.Id;

        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail(MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
