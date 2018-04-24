using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Admin.Models.Mapping;

namespace Admin.Models
{
    public partial class Context : DbContext
    {
        static Context()
        {
            Database.SetInitializer<Context>(null);
        }

        public Context()
            : base("Name=Context")
        {
        }

        public DbSet<BaseAccount> BaseAccounts { get; set; }
        public DbSet<BaseAction> BaseActions { get; set; }
        public DbSet<BaseColumn> BaseColumns { get; set; }
        public DbSet<BaseController> BaseControllers { get; set; }
        public DbSet<BaseCountry> BaseCountries { get; set; }
        public DbSet<BaseDisabled> BaseDisableds { get; set; }
        public DbSet<BaseDynamicColumnList> BaseDynamicColumnLists { get; set; }
        public DbSet<BaseDynamicList> BaseDynamicLists { get; set; }
        public DbSet<BaseEmail> BaseEmails { get; set; }
        public DbSet<BaseGraph> BaseGraphs { get; set; }
        public DbSet<BaseLanguage> BaseLanguages { get; set; }
        public DbSet<BaseLog> BaseLogs { get; set; }
        public DbSet<BaseLogAction> BaseLogActions { get; set; }
        public DbSet<BaseMenu> BaseMenus { get; set; }
        public DbSet<BaseMessage> BaseMessages { get; set; }
        public DbSet<BaseMessageCategory> BaseMessageCategories { get; set; }
        public DbSet<BaseProfile> BaseProfiles { get; set; }
        public DbSet<BaseProfileMenu> BaseProfileMenus { get; set; }
        public DbSet<BaseQueryGraph> BaseQueryGraphs { get; set; }
        public DbSet<BaseSystemTable> BaseSystemTables { get; set; }
        public DbSet<BaseTable> BaseTables { get; set; }
        public DbSet<BaseTableContrain> BaseTableContrains { get; set; }
        public DbSet<BaseTemplateDataSource> BaseTemplateDataSources { get; set; }
        public DbSet<BaseTemplatePage> BaseTemplatePages { get; set; }
        public DbSet<BaseTemplate> BaseTemplates { get; set; }
        public DbSet<BaseTemplateTable> BaseTemplateTables { get; set; }
        public DbSet<BaseTemplateVariable> BaseTemplateVariables { get; set; }
        public DbSet<BaseTimeZone> BaseTimeZones { get; set; }
        public DbSet<BaseTypesGraph> BaseTypesGraphs { get; set; }
        public DbSet<BaseUIIcon> BaseUIIcons { get; set; }
        public DbSet<BaseUser> BaseUsers { get; set; }
        public DbSet<BaseUserAction> BaseUserActions { get; set; }
        public DbSet<BaseUserMenu> BaseUserMenus { get; set; }
        public DbSet<BaseUserSession> BaseUserSessions { get; set; }
        public DbSet<BaseUserStatu> BaseUserStatus { get; set; }
        public DbSet<BaseViewRelation> BaseViewRelations { get; set; }
        public DbSet<BaseWebSocketChannelBlackListProfile> BaseWebSocketChannelBlackListProfiles { get; set; }
        public DbSet<BaseWebSocketChannelBlackListUser> BaseWebSocketChannelBlackListUsers { get; set; }
        public DbSet<BaseWebSocketChannel> BaseWebSocketChannels { get; set; }
        public DbSet<BaseWidget> BaseWidgets { get; set; }
        public DbSet<RDCategory> RDCategories { get; set; }
        public DbSet<RDChannel> RDChannels { get; set; }
        public DbSet<RDClient> RDClients { get; set; }
        public DbSet<RDClientChannel> RDClientChannels { get; set; }
        public DbSet<RDEvent> RDEvents { get; set; }
        public DbSet<RDProgramming> RDProgrammings { get; set; }
        public DbSet<RDType> RDTypes { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<BaseAudit> BaseAudits { get; set; }
        public DbSet<BaseBGColor> BaseBGColors { get; set; }
        public DbSet<BaseDiagnostic> BaseDiagnostics { get; set; }
        public DbSet<BaseDiagnosticBase> BaseDiagnosticBases { get; set; }
        public DbSet<BaseGuestColumn> BaseGuestColumns { get; set; }
        public DbSet<BaseGuestConstraint> BaseGuestConstraints { get; set; }
        public DbSet<BaseGuestTable> BaseGuestTables { get; set; }
        public DbSet<VWBaseActionController> VWBaseActionControllers { get; set; }
        public DbSet<VWBaseallsystem> VWBaseallsystems { get; set; }
        public DbSet<VWBaseColumnDescription> VWBaseColumnDescriptions { get; set; }
        public DbSet<VWBaseDynamicList> VWBaseDynamicLists { get; set; }
        public DbSet<VWBaseKeyWord> VWBaseKeyWords { get; set; }
        public DbSet<VWBaseMonth> VWBaseMonths { get; set; }
        public DbSet<VWBaseNiiAll> VWBaseNiiAlls { get; set; }
        public DbSet<VWBaseNiiColumn> VWBaseNiiColumns { get; set; }
        public DbSet<VWBaseNiiFk> VWBaseNiiFks { get; set; }
        public DbSet<VWBaseProfile> VWBaseProfiles { get; set; }
        public DbSet<VWBaseRealRelation> VWBaseRealRelations { get; set; }
        public DbSet<VWBaseRelation> VWBaseRelations { get; set; }
        public DbSet<VWBSAlertRuleCondition> VWBSAlertRuleConditions { get; set; }
        public DbSet<VWBSAlertTransaction> VWBSAlertTransactions { get; set; }
        public DbSet<VWBSStageStatuStage> VWBSStageStatuStages { get; set; }
        public DbSet<VWDataSourceColumn> VWDataSourceColumns { get; set; }
        public DbSet<VWGraphListView> VWGraphListViews { get; set; }
        public DbSet<VWHAWKColumn> VWHAWKColumns { get; set; }
        public DbSet<VWISMultipleRElation> VWISMultipleRElations { get; set; }
        public DbSet<VWISRElation> VWISRElations { get; set; }
        public DbSet<VWListaUsuario> VWListaUsuarios { get; set; }
        public DbSet<VWOCHouseInformation> VWOCHouseInformations { get; set; }
        public DbSet<VWOCHouseInformationExport> VWOCHouseInformationExports { get; set; }
        public DbSet<VWSqlTable> VWSqlTables { get; set; }
        public DbSet<VWUser> VWUsers { get; set; }
        public DbSet<VWUserAdmin> VWUserAdmins { get; set; }
        public DbSet<VWUserInApp> VWUserInApps { get; set; }
        public DbSet<VWUserInClient> VWUserInClients { get; set; }
        public DbSet<VWUserStatu> VWUserStatus { get; set; }
        public DbSet<VWWidgetGraph> VWWidgetGraphs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BaseAccountMap());
            modelBuilder.Configurations.Add(new BaseActionMap());
            modelBuilder.Configurations.Add(new BaseColumnMap());
            modelBuilder.Configurations.Add(new BaseControllerMap());
            modelBuilder.Configurations.Add(new BaseCountryMap());
            modelBuilder.Configurations.Add(new BaseDisabledMap());
            modelBuilder.Configurations.Add(new BaseDynamicColumnListMap());
            modelBuilder.Configurations.Add(new BaseDynamicListMap());
            modelBuilder.Configurations.Add(new BaseEmailMap());
            modelBuilder.Configurations.Add(new BaseGraphMap());
            modelBuilder.Configurations.Add(new BaseLanguageMap());
            modelBuilder.Configurations.Add(new BaseLogMap());
            modelBuilder.Configurations.Add(new BaseLogActionMap());
            modelBuilder.Configurations.Add(new BaseMenuMap());
            modelBuilder.Configurations.Add(new BaseMessageMap());
            modelBuilder.Configurations.Add(new BaseMessageCategoryMap());
            modelBuilder.Configurations.Add(new BaseProfileMap());
            modelBuilder.Configurations.Add(new BaseProfileMenuMap());
            modelBuilder.Configurations.Add(new BaseQueryGraphMap());
            modelBuilder.Configurations.Add(new BaseSystemTableMap());
            modelBuilder.Configurations.Add(new BaseTableMap());
            modelBuilder.Configurations.Add(new BaseTableContrainMap());
            modelBuilder.Configurations.Add(new BaseTemplateDataSourceMap());
            modelBuilder.Configurations.Add(new BaseTemplatePageMap());
            modelBuilder.Configurations.Add(new BaseTemplateMap());
            modelBuilder.Configurations.Add(new BaseTemplateTableMap());
            modelBuilder.Configurations.Add(new BaseTemplateVariableMap());
            modelBuilder.Configurations.Add(new BaseTimeZoneMap());
            modelBuilder.Configurations.Add(new BaseTypesGraphMap());
            modelBuilder.Configurations.Add(new BaseUIIconMap());
            modelBuilder.Configurations.Add(new BaseUserMap());
            modelBuilder.Configurations.Add(new BaseUserActionMap());
            modelBuilder.Configurations.Add(new BaseUserMenuMap());
            modelBuilder.Configurations.Add(new BaseUserSessionMap());
            modelBuilder.Configurations.Add(new BaseUserStatuMap());
            modelBuilder.Configurations.Add(new BaseViewRelationMap());
            modelBuilder.Configurations.Add(new BaseWebSocketChannelBlackListProfileMap());
            modelBuilder.Configurations.Add(new BaseWebSocketChannelBlackListUserMap());
            modelBuilder.Configurations.Add(new BaseWebSocketChannelMap());
            modelBuilder.Configurations.Add(new BaseWidgetMap());
            modelBuilder.Configurations.Add(new RDCategoryMap());
            modelBuilder.Configurations.Add(new RDChannelMap());
            modelBuilder.Configurations.Add(new RDClientMap());
            modelBuilder.Configurations.Add(new RDClientChannelMap());
            modelBuilder.Configurations.Add(new RDEventMap());
            modelBuilder.Configurations.Add(new RDProgrammingMap());
            modelBuilder.Configurations.Add(new RDTypeMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new BaseAuditMap());
            modelBuilder.Configurations.Add(new BaseBGColorMap());
            modelBuilder.Configurations.Add(new BaseDiagnosticMap());
            modelBuilder.Configurations.Add(new BaseDiagnosticBaseMap());
            modelBuilder.Configurations.Add(new BaseGuestColumnMap());
            modelBuilder.Configurations.Add(new BaseGuestConstraintMap());
            modelBuilder.Configurations.Add(new BaseGuestTableMap());
            modelBuilder.Configurations.Add(new VWBaseActionControllerMap());
            modelBuilder.Configurations.Add(new VWBaseallsystemMap());
            modelBuilder.Configurations.Add(new VWBaseColumnDescriptionMap());
            modelBuilder.Configurations.Add(new VWBaseDynamicListMap());
            modelBuilder.Configurations.Add(new VWBaseKeyWordMap());
            modelBuilder.Configurations.Add(new VWBaseMonthMap());
            modelBuilder.Configurations.Add(new VWBaseNiiAllMap());
            modelBuilder.Configurations.Add(new VWBaseNiiColumnMap());
            modelBuilder.Configurations.Add(new VWBaseNiiFkMap());
            modelBuilder.Configurations.Add(new VWBaseProfileMap());
            modelBuilder.Configurations.Add(new VWBaseRealRelationMap());
            modelBuilder.Configurations.Add(new VWBaseRelationMap());
            modelBuilder.Configurations.Add(new VWBSAlertRuleConditionMap());
            modelBuilder.Configurations.Add(new VWBSAlertTransactionMap());
            modelBuilder.Configurations.Add(new VWBSStageStatuStageMap());
            modelBuilder.Configurations.Add(new VWDataSourceColumnMap());
            modelBuilder.Configurations.Add(new VWGraphListViewMap());
            modelBuilder.Configurations.Add(new VWHAWKColumnMap());
            modelBuilder.Configurations.Add(new VWISMultipleRElationMap());
            modelBuilder.Configurations.Add(new VWISRElationMap());
            modelBuilder.Configurations.Add(new VWListaUsuarioMap());
            modelBuilder.Configurations.Add(new VWOCHouseInformationMap());
            modelBuilder.Configurations.Add(new VWOCHouseInformationExportMap());
            modelBuilder.Configurations.Add(new VWSqlTableMap());
            modelBuilder.Configurations.Add(new VWUserMap());
            modelBuilder.Configurations.Add(new VWUserAdminMap());
            modelBuilder.Configurations.Add(new VWUserInAppMap());
            modelBuilder.Configurations.Add(new VWUserInClientMap());
            modelBuilder.Configurations.Add(new VWUserStatuMap());
            modelBuilder.Configurations.Add(new VWWidgetGraphMap());
        }
    }
}
