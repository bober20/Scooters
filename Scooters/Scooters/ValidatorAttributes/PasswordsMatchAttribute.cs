using System.ComponentModel.DataAnnotations;

namespace Scooters.ValidatorAttributes;

public sealed class PasswordsMatchAttribute : ValidationAttribute
{
    public PasswordsMatchAttribute(string otherPropertyName)
    { 
        PropertyName = otherPropertyName;
    }
    
    public string PropertyName { get; }
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        object
            instance = validationContext.ObjectInstance,
            otherValue = instance.GetType().GetProperty(PropertyName).GetValue(instance);

        if (((IComparable)value).CompareTo(otherValue) == 0)
        {
            return ValidationResult.Success;
        }

        return new("Passwords do not match.");
    }
}