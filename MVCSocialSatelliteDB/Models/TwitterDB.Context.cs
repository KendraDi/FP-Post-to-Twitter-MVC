﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCSocialSatelliteDB.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SocialSatelliteEntities : DbContext
    {
        public SocialSatelliteEntities()
            : base("name=SocialSatelliteEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Tweet> Tweet { get; set; }
        public virtual DbSet<TwitterAccess> TwitterAccesses { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
