using System.Data.Entity.Spatial;

namespace Admin.CustomCode
{
    public static class DbGeographyExtensions
    {
        public static DbGeography FromLongitudeAndLatitude(this DbGeography dbGeography, double longitude, double latitude)
        {
            var point = string.Format("POINT({0} {1})", longitude, latitude);
            return DbGeography.FromText(point);
        }
    }
}