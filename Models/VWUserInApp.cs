using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWUserInApp
    {
        public int ID { get; set; }
        public string name { get; set; }
        public Nullable<int> userStatusID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string activeKey { get; set; }
        public Nullable<System.DateTime> lastVisitAt { get; set; }
        public bool superUser { get; set; }
        public Nullable<int> departmentID { get; set; }
        public Nullable<int> companyID { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUser { get; set; }
        public Nullable<int> positionID { get; set; }
        public string imageUrl { get; set; }
        public string lastName { get; set; }
        public string lastName2 { get; set; }
        public Nullable<bool> externalUser { get; set; }
        public string confirmationToken { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string phone2 { get; set; }
        public Nullable<int> defaultActionID { get; set; }
        public string email { get; set; }
        public string lastIP { get; set; }
        public Nullable<int> TipoDocumentoID { get; set; }
        public string identification { get; set; }
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        public string cookie { get; set; }
        public Nullable<int> failedAttempts { get; set; }
        public Nullable<bool> firstTime { get; set; }
        public Nullable<int> widgetsToShow { get; set; }
        public string tryAction { get; set; }
        public string tryController { get; set; }
        public Nullable<int> countryID { get; set; }
        public string sector { get; set; }
        public string motherFamilyName { get; set; }
        public string favoriteColor { get; set; }
        public string firstTrip { get; set; }
        public Nullable<int> randomQuestionId { get; set; }
        public Nullable<bool> superAdminShowHiddenMenu { get; set; }
        public Nullable<bool> isCompany { get; set; }
        public string companyContactName { get; set; }
        public string companyContactLastName { get; set; }
        public Nullable<int> imageID { get; set; }
    }
}
