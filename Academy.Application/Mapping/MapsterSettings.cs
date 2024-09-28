using Academy.Application.Academies.Dto;
using Academy.Domain.Identity;
using Academy.Shared;
using Mapster;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.Mapping
{
    public class MapsterSettings
    {
        public static void Configure()
        {
            // here we will define the type conversion / Custom-mapping
            // More details at https://github.com/MapsterMapper/Mapster/wiki/Custom-mapping

            // This one is actually not necessary as it's mapped by convention
            // TypeAdapterConfig<Product, ProductDto>.NewConfig().Map(dest => dest.BrandName, src => src.Brand.Name);

            TypeAdapterConfig<Entites.Academies, AcademiesDto>.NewConfig()
                .Map(dest => dest.Logo, src => string.Format(Constants.CloudFrontUrl, Constants.S3Directory.Logo, src.Logo))
                .Map(dest => dest.QR, src => string.Format(Constants.CloudFrontUrl, Constants.S3Directory.QR, src.QRCode));

            TypeAdapterConfig<Entites.Academies, AcademyDetailDto>.NewConfig()
                .Map(dest => dest.Logo, src => string.Format(Constants.CloudFrontUrl, Constants.S3Directory.Logo, src.Logo))
                .Map(dest => dest.QRCode, src => string.Format(Constants.CloudFrontUrl, Constants.S3Directory.QR, src.QRCode))
                .Map(dest => dest.Sports, src => src.AcademySportsMappings.Count > 0 ? src.AcademySportsMappings.Select(x => x.SportId).ToList() : new List<Guid>());

            TypeAdapterConfig<ApplicationUser, AcademyUsersDetailsDto>.NewConfig()
                .Map(dest => dest.FullName, src => src.FullName());
        }
    }
}