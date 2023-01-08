using System;
using System.Collections.Generic;

namespace Chrono.Services.Models;

public class WorkItemModel : WorkItemInfoSimple
{
    //public WorkItemModel()
    //{
    //    ItemTimes = new 
    //}

    //public Guid WorkItemId { get; set; }
    //public string? Name { get; set; }
    //public string? WorkItemType { get; set; }
    //public double Estimate { get; set; } = 0;
    public IEnumerable<DayInfo>? ItemTimes { get; set; }
    //public string? State { get; set; }
    //public bool IsEditable { get; set; } = false;
    //public DateTime ReportDate { get; set; }        
}

public class DayInfo
{
    public DateTime DayWork { get; set; }
    public WorkInfo? ComplitedWork { get; set; }
    public WorkInfo? ComplitedOver { get; set; }
}

public class WorkInfoShort
{
    public Guid Id { get; set; }
    public double? Hours { get; set; }
    public double? BillHours { get; set; }
}

public class WorkInfo : WorkInfoShort
{
    public string? ReportStatus { get; set; }
    public bool IsEditable { get; set; } = false;
}

public class WorkItemTime
{
    public double? ComplitedWork { get; set; }
    public double? ComplitedOver { get; set; }
}