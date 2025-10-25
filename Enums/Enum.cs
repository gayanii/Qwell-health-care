using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Enums
{
    public enum RecordTypeEnum
    {
        Medical = 1,
        Lab = 2,
        Procedure = 3,
        Channel = 4
    }

    public enum UserStatusEnum
    {
        [Description("Active")]
        Active,

        [Description("Inactive")]
        Inactive
    }

    public enum EmployeeTypeEnum
    {
        [Description("Admin")]
        Admin,

        [Description("Manager")]
        Manager,

        [Description("Staff")]
        Staff
    }

    public enum ActionTypeEnum
    {
        [Description("Update")]
        Update,

        [Description("Add")]
        Add,

        [Description("Delete")]
        Delete
    }

    public enum EntitiesEnum
    {
        [Description("Products")]
        Products,

        [Description("Product Records")]
        ProductRecords,

        [Description("Suppliers")]
        Suppliers,

        [Description("Medical Records")]
        MedicalRecords,

        [Description("Procedure Records")]
        ProcedureRecords,

        [Description("Lab Records")]
        LabRecords,

        [Description("Channel Records")]
        ChannelRecords,

        [Description("Employees")]
        Employees,

        [Description("Patients")]
        Patients,

        [Description("Lab Tests")]
        LabTests,
    }
}
