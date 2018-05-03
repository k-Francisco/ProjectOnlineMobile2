using Microsoft.EntityFrameworkCore;
using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Models.PSPL;
using ProjectResult = ProjectOnlineMobile2.Models.PSPL.Result;
using TasksResult = ProjectOnlineMobile2.Models.ResourceAssignmentModel.Result;
using TimesheetPeriodsResult = ProjectOnlineMobile2.Models.TSPL.Result;
using LineResult = ProjectOnlineMobile2.Models.TLL.Result;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace ProjectOnlineMobile2.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<LineWorkChangesModel> Changes { get; set; }

        public DbSet<ProjectResult> Projects { get; set; }

        public DbSet<TasksResult> UserTasks { get; set; }

        public DbSet<TimesheetPeriodsResult> Periods { get; set; }

        //public DbSet<LineResult> Lines { get; set; }

        private string _databasePath { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LineWorkChangesModel>().HasKey(k => k.StartDate);
        }

        public DatabaseContext(String path)
        {
            _databasePath = path;
            Database.EnsureCreated();
            
            //if(Device.RuntimePlatform == Device.iOS)
            //{
            //    Snapshot.MaxGenericTypes = 2;
            //}
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}
