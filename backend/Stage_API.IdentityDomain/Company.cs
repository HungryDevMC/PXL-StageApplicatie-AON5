// Stage_API - (c) Saze 2020

using System;

namespace Stage_API.Domain
{
    public class Company : Entity
    {
        public string Name { get; set; }
        
        public int EmployeeCount { get; set; }

        public int ITEmployeeCount { get; set; }

        public int SupportingITEmployees { get; set; }

        public float Latitude1 { get; set; }

        public float Longitude1 { get; set; }

        public float Latitude2 { get; set; }

        public float Longitude2 { get; set; }

        public string ContactTitle { get; set; }

        public Guid ContactAccountGuid { get; set; }

        public string CompanyTitle { get; set; }

        public Guid CompanyAccountGuid { get; set; }
    }
}