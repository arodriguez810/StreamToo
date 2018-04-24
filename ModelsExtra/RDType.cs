using Admin.CustomCode;
using Admin.ModelsExtra;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
namespace Admin.Models
{
    public partial class RDType : IScopeble
    {
        
        public BoolString BeforeSave(Context db)
        {
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString BeforeEdit(Context db)
        {
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString BeforeCreate(Context db)
        {
        active=true;
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString BeforeDelete(Context db)
        {
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString BeforeActive(Context db)
        {
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString BeforeInactive(Context db)
        {
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString AfterSave(Context db)
        {
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString AfterEdit(Context db)
        {
        Helper.Audit(db, "RDType", AuditMode.Edit, id,this);
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString AfterCreate(Context db)
        {
        Helper.Audit(db, "RDType", AuditMode.Create, id,this);
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString AfterDelete(Context db)
        {
        Helper.Audit(db, "RDType", AuditMode.Delete, id,this);
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString AfterActive(Context db)
        {
        Helper.Audit(db, "RDType", AuditMode.Active, id,this);
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
        public BoolString AfterInactive(Context db)
        {
        Helper.Audit(db, "RDType", AuditMode.Inactive, id,this);
            List<string> messages = new List<string>();
            return ScopeHelper.RegulateMessages(messages);
        }
    }
}
