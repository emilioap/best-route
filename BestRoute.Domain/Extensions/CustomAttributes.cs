using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BestRoute.Domain.Extensions
{
    public class CustomAttributes
    {
        public class NotEqualAttribute : ValidationAttribute
        {
            private string OtherProperty { get; set; }

            public NotEqualAttribute(string otherProperty)
            {
                OtherProperty = otherProperty;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
                var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

                if (value.ToString().Equals(otherValue.ToString()))
                    return new ValidationResult(string.Format("Field {0} should not be equal to {1}.", validationContext.MemberName, OtherProperty));
                else
                    return ValidationResult.Success;
            }
        }

        public class NotNullAttribute : ValidationAttribute
        {
            public NotNullAttribute()
            {
            }

            public override bool IsValid(object value)
            {
                string strValue = value as string;
                if (!string.IsNullOrEmpty(strValue))
                    return false;
                else
                    return true;
            }
        }
    }
}
