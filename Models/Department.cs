﻿namespace bazyProjektBlazor.Models
{
    public class Department
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public User Director { get; set; }
    }
}
