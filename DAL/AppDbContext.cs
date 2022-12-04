﻿using back_project.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace back_project.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<TestimonialSlider> TestimonialSliders { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
