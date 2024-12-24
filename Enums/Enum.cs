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
        Procedure = 3
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
}
