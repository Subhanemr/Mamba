﻿namespace Mamba.Models
{
    public class Settings : BaseEntity
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
