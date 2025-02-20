using System.Data;
using Dapper;

namespace Persistence;

public class DateOnlyHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(TimeOnly.MinValue); // Convert DateOnly to DateTime
    }

    public override DateOnly Parse(object value)
    {
        if (value is DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime); // Convert DateTime to DateOnly
        }
        throw new InvalidCastException($"Unable to convert {value} to DateOnly.");
    }
}