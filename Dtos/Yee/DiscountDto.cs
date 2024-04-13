using HotelFuen31.APIs.Interfaces.Yee;
using Newtonsoft.Json;

namespace HotelFuen31.APIs.Dtos.Yee
{
    public class DiscountDto
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("rule")]
        public RuleDto? Rule { get; set; }

        [JsonProperty("machedIndex")]
        public IEnumerable<int>? MachedIndex { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }
    }
}
