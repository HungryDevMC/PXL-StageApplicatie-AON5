namespace Stage_API.Dto
{
    public class CompanyUpdate
    {
        public string CompanyName { get; set; }

        public int EmployeeCount { get; set; }

        public int ITEmployeeCount { get; set; }

        public int SupportingITEmployees { get; set; }

        public float Lat1 { get; set; }

        public float Lng1 { get; set; }

        public float Lat2 { get; set; }

        public float Lng2 { get; set; }

        public string Contact_Title { get; set; }

        public string Company_Title { get; set; }
    }
}
