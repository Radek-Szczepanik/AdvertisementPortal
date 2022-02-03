using AdvertisementPortal.Entities;
using FluentValidation;

namespace AdvertisementPortal.Models.Validators
{
    public class AdvertisementQueryValidator : AbstractValidator<AdvertisementQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };

        private string[] allowedSortByColumnNames = { nameof(Advertisement.Title), nameof(Advertisement.Content) };
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

            RuleFor(r => r.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                                  .WithMessage($"Sort by is optional or must be in [{string.Join(",", allowedSortByColumnNames)}]");
                 


        }
    }
}
