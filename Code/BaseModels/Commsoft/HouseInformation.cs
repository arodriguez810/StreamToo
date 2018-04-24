using System.Collections.Generic;

namespace Admin.BaseModels.Commsoft
{
    public class RequestSearchAddress
    {
        public string state { get; set; }
        public string pageSize { get; set; }
        public string securityToken { get; set; }
    }

    public class RequestSearchAddressResponse
    {
        public Outbody outbody { get; set; }
    }

    public class Outbody
    {
        public OutSearchAddr outsearchaddr { get; set; }
    }

    public class OutSearchAddr
    {
        public List<OutSearchAddrItem> item { get; set; }
    }

    public class OutSearchAddrItem
    {
        public string state { get; set; }
        public string city { get; set; }
        public string streetDirection { get; set; }
        public string street { get; set; }
        public string streetSuffix { get; set; }
        public string streetNumber { get; set; }
        public string streetNumberSuffix { get; set; }
        public string apartment { get; set; }
        public string location { get; set; }
        public string houseId { get; set; }
        public string lot { get; set; }
        public string box { get; set; }
        public string suite { get; set; }
        public string postDirection { get; set; }
        public string property { get; set; }
        public string building { get; set; }
        public string siteDescription { get; set; }
        public string streetName2 { get; set; }
        public string area { get; set; }
        public string site { get; set; }
        public string cityCode { get; set; }
        public string communityCode { get; set; }
        public string countyCode { get; set; }
        public string country { get; set; }
        public string zip5 { get; set; }
        public string zip4 { get; set; }
        public string zip2 { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressLine3 { get; set; }
        public string addressLine4 { get; set; }
        public string wired { get; set; }
        public string rightOfWay { get; set; }
        public string exchange { get; set; }
        public string tarCode { get; set; }
        public string deliveryPoint { get; set; }
        public string carrierRoute { get; set; }
        public string checkDigit { get; set; }
        public string centralOffice { get; set; }
        public string servedById { get; set; }
        public string cableId { get; set; }
        public string msagCommunity { get; set; }
        public string postalCommunity { get; set; }
        public string leftInFound { get; set; }
        public string drivingDirections { get; set; }
        public OutSearchAddrAttributes outsearchaddrattributes { get; set; }
    }

    public class OutSearchAddrAttributes
    {
        public List<OutSearchAddrAttributes> item { get; set; }
    }

    public class OutSearchAddrAttributesItem
    {
        public string attributeType { get; set; }
        public string attributeNumber { get; set; }
        public string attributeValue { get; set; }
    }
}