using Microsoft.EntityFrameworkCore;
using VoiceRecordAPI.Models;
using System;
using System.Collections.Generic;

namespace VoiceRecordAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasData(new List<Role>()
               {
                    new Role(){ Id = Guid.NewGuid(), Name = "user"},
                    new Role(){ Id = Guid.NewGuid(), Name = "Manager"},
                    new Role(){ Id = Guid.NewGuid(), Name = "Admin"},
                    new Role(){ Id = Guid.NewGuid(), Name = "Developer"}
               });

            modelBuilder.Entity<VoiceRecordProviders>().HasData(
                new List<VoiceRecordProviders>()
                {
                    new VoiceRecordProviders(){Id=1,Detail = "3CX",IsActive = true,Remark="3CX"},
                    new VoiceRecordProviders(){Id=2,Detail = "Ericsson",IsActive = true,Remark="Ericsson"}
                }
            );

            modelBuilder.Entity<VoiceRecordConfigurations>().HasData(
                new List<VoiceRecordConfigurations>()
                {
                    new VoiceRecordConfigurations(){ParameterName="ThreeCXPath", ValueString=""},
                    new VoiceRecordConfigurations(){ParameterName="ThreeCXPathFormat", ValueString=""},
                    new VoiceRecordConfigurations(){ParameterName="ThreeCXFileFormatCallOut", ValueString="[DisplayName]_Extension-CalledNumber_YearMonthDayHourMinuteSecond(InternalCallIdentifier)"},
                    new VoiceRecordConfigurations(){ParameterName="ThreeCXFileFormatCallIn", ValueString="[Extension%3ACalledNumber]_CalledNumber-Extension_YearMonthDayHourMinuteSecond(InternalCallIdentifier)"},
                    new VoiceRecordConfigurations(){ParameterName="EricssonPath", ValueString=""},
                    new VoiceRecordConfigurations(){ParameterName="EricssonPathFormat", ValueString=""},
                    new VoiceRecordConfigurations(){ParameterName="EricssonFileFormatCallOut", ValueString="DateTime-Extentions"},
                    new VoiceRecordConfigurations(){ParameterName="EricssonFileFormatCallIn", ValueString="DateTime-Extentions"},
                    new VoiceRecordConfigurations(){ParameterName="URLTimeout", ValueString="Day", ValueNumber=7,Remark="ValueString is unit name (minute,hour,day,month,year) ,ValueNumber is value "},
                    new VoiceRecordConfigurations(){ParameterName="WebDomainFormat", ValueString="http://localhost:50314/AudioPlayer/AudioPlayer?id={GUID}",Remark="Web client format for media player client"}
                }
            );

            modelBuilder.Entity<CallType>().HasData(
                new List<CallType>()
                {
                    //new CallType(){Id=3,Detail = "Unknown",IsActive = true,Remark="Unknown Type"},
                    new CallType(){Id=1,Detail = "Inbound",IsActive = true,Remark="Inbound calling"},
                    new CallType(){Id=2,Detail = "Outbound",IsActive = true,Remark="Outbound calling"},
                    new CallType(){Id=3,Detail = "Unknow",IsActive = true,Remark="Unknow"}
                }
            );

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<VoiceRecordDetails> VoiceRecordDetails { get; set; }
        public DbSet<VoiceRecordProviders> VoiceRecordProviders { get; set; }
        public DbSet<VoiceRecordConfigurations> VoiceRecordConfigurations { get; set; }
        public DbSet<CallType> CallTypes { get; set; }
        public DbSet<VoiceRecordURLRequest> VoiceRecordURLRequests { get; set; }
    }
}