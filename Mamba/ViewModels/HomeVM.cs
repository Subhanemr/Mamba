﻿using Mamba.Models;

namespace Mamba.ViewModels
{
    public class HomeVM
    {
        public ICollection<Product> Products { get; set; }
        public ICollection<Service> Services { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
