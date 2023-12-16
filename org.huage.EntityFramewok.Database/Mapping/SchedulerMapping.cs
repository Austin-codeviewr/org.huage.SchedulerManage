using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using org.huage.EntityFramewok.Database.Table;

namespace org.huage.EntityFramewok.Database.Mapping;

public class SchedulerMapping : IEntityTypeConfiguration<Scheduler>
{
    public void Configure(EntityTypeBuilder<Scheduler> builder)
    {
        builder.Property(_ => _.MethodName).HasMaxLength(50);
        builder.HasIndex(_ => _.MethodName).IsUnique();
        builder.Property(_ => _.CronExpression).IsRequired();
    }
}