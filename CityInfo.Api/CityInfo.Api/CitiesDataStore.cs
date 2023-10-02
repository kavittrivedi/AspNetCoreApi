using CityInfo.Api.Models;

namespace CityInfo.Api
{
    public class CitiesDataStore
    {
        public List<CityDto> cities { get; set; }
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public CitiesDataStore()
        {
            cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Vadodara",
                    Description="Vadodara",
                    PointsOfInterest=new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto(){
                        Id = 1,
                        Name="Central Park",
                        Description="Small park"
                        
                        },
                        new PointOfInterestDto(){
                        Id= 2,
                        Name="Empire State Building",
                        Description="Small building"
                        },
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Surat",
                    Description="Surat",
                    PointsOfInterest=new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto(){
                        Id = 3,
                        Name="Surat Central Park",
                        Description="Small park"

                        },
                        new PointOfInterestDto(){
                        Id= 4,
                        Name="Surat Empire State Building",
                        Description="Small building"
                        },
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Indore",
                    Description="Indore",
                    PointsOfInterest=new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto(){
                        Id = 5,
                        Name="Indore Central Park",
                        Description="Small park"

                        },
                        new PointOfInterestDto(){
                        Id= 6,
                        Name="Indore Empire State Building",
                        Description="Small building"
                        },
                    }
                },
            };
        }
    }
}
