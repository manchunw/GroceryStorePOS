using System;
namespace Core.Models
{
    public enum CommandType
    {
        None,
        Scan,
        Cancel,
        Print,
        DiscountByPercentage,
        DiscountByQuantity
    }
}
