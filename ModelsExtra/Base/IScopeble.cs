using Admin.CustomCode;
using Admin.Models;

namespace Admin.ModelsExtra
{
    public interface IScopeble
    {
        BoolString BeforeSave(Context db);
        BoolString BeforeEdit(Context db);
        BoolString BeforeCreate(Context db);
        BoolString BeforeDelete(Context db);
        BoolString BeforeInactive(Context db);
        BoolString BeforeActive(Context db);

        BoolString AfterSave(Context db);
        BoolString AfterEdit(Context db);
        BoolString AfterCreate(Context db);
        BoolString AfterDelete(Context db);
        BoolString AfterInactive(Context db);
        BoolString AfterActive(Context db);
    }
}