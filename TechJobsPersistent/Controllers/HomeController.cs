﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        public JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            List<Employer> employers = context.Employers.ToList();
            List<Skill> skills = context.Skills.ToList();
            AddJobViewModel viewModel = new AddJobViewModel(employers, skills);
            return View(viewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Job newJob = new Job
                {
                    Name = viewModel.Name,
                    Employer = context.Employers.Find(viewModel.EmployerId)
                };

                foreach (var selectedskill in viewModel.SelectedSkills)
                {
                    JobSkill newJobSkill = new JobSkill
                    {
                        Job = newJob,
                        Skill = context.Skills.Find(int.Parse(selectedskill))
                    };
                    context.JobSkills.Add(newJobSkill);
                }

                context.Jobs.Add(newJob);
                context.SaveChanges();
                return Redirect("/Home");
            }

                return View("AddJob", viewModel);
            }

            public IActionResult Detail(int id)
            {
                Job theJob = context.Jobs
                    .Include(j => j.Employer)
                    .Single(j => j.Id == id);

                List<JobSkill> jobSkills = context.JobSkills
                    .Where(js => js.JobId == id)
                    .Include(js => js.Skill)
                    .ToList();

                JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
                return View(viewModel);
            }
        }
    }
