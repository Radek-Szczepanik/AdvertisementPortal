using FluentValidation;
using System.Linq;

namespace AdvertisementPortal.Models.Validators
{
    public class AdvertisementQueryValidator : AbstractValidator<AdvertisementQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        public AdvertisementQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
                }
            });
        }
    }
}
