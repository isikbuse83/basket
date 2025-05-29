using System.Collections.Generic;
using ConsoleApp1.Reponse;


namespace ConsoleApp1.Reponse
{
    public class BasketResponse
    {
        public int BasketId { get; set; }
        public List<BasketItemResponse> BasketItems { get; set; }
    }
    
}
