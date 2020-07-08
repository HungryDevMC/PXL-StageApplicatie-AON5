using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stage_API.Domain;

namespace Stage_API.Data.Configuration.CompanyConfiguration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.Property(e => e.Name).HasColumnName("Name")
                .IsRequired();

            builder.Property(e => e.EmployeeCount).HasColumnName("EmployeeCount")
                .IsRequired();

            builder.Property(e => e.ITEmployeeCount).HasColumnName("ITEmployeeCount")
                .IsRequired();

            builder.Property(e => e.SupportingITEmployees).HasColumnName("SupportingITEmployees")
                .IsRequired();

            builder.Property(e => e.Latitude1).HasColumnName("Latitude1")
                .IsRequired();

            builder.Property(e => e.Longitude1).HasColumnName("Longitude1")
                .IsRequired();

            builder.Property(e => e.Latitude2).HasColumnName("Latitude2")
                .IsRequired();

            builder.Property(e => e.Longitude2).HasColumnName("Longitude2")
                .IsRequired();

            builder.Property(e => e.ContactTitle).HasColumnName("ContactTitle")
                .IsRequired();

            builder.Property(e => e.ContactAccountGuid).HasColumnName("ContactAccountGuid")
                .IsRequired();

            builder.Property(e => e.CompanyTitle).HasColumnName("CompanyTitle")
                .IsRequired();

            builder.Property(e => e.CompanyAccountGuid).HasColumnName("CompanyAccountGuid")
                .IsRequired();
        }
    }
}
