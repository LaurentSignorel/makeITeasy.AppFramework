﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using makeITeasy.AppFramework.Models;

namespace makeITeasy.CarCatalog.Models
{
    public partial class Country : IBaseEntity
    {
        public Country()
        {
            Brands = new HashSet<Brand>();
        }

        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
    }
}