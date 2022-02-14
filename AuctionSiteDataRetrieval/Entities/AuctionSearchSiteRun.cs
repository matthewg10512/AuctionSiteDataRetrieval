using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionSiteDataRetrieval.Entities
{
   public class AuctionSearchSiteRun
    {
        public int Id { get; set; }
        public int AuctionSiteId { get; set; }
        public int AuctionSearchWordId { get; set; }
        public DateTime DateSearch { get; set; }
    }
}
