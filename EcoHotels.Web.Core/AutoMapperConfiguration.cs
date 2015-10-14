namespace EcoHotels.Web.Core
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            //Mapper.CreateMap<RegisterModel, Organization>()
            //    .ForMember(entity => entity.Id, expr => expr.Ignore())
            //    .ForMember(entity => entity.Hotels, expr => expr.Ignore())
            //    .ForMember(entity => entity.AvailableCurrencies, expr => expr.Ignore())
            //    .ForMember(entity => entity.AvailableLanguages  , expr => expr.Ignore())
            //    .ForAllMembers(expression => expression.NullSubstitute(string.Empty));

            //Mapper.CreateMap<RegisterUserModel, User>()
            //    .ForMember(entity => entity.Id, expr => expr.Ignore())
            //    //.ForMember(entity => entity.HotelId, expr => expr.Ignore())
            //    .ForAllMembers(expression => expression.NullSubstitute(string.Empty));

            //Mapper.CreateMap<EditHotelInfoModel, Hotel>()
            //    .ForMember(entity => entity.Id, expr => expr.Ignore())
            //    .ForMember(entity => entity.OrganizationId  , expr => expr.Ignore())
            //    .ForMember(entity => entity.Users, expr => expr.Ignore())
            //    .ForMember(entity => entity.IsActive, expr => expr.Ignore())
            //    .ForAllMembers(expression => expression.NullSubstitute(string.Empty));


            //Mapper.AssertConfigurationIsValid();
        }
    }
}