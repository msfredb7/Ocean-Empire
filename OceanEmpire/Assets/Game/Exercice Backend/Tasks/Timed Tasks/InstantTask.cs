﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantTask : TimedTask
{
    public override TaskReport BuildReport()
    {
        return new TaskReport(null, DateTime.Now, plannedOn.dateTime);
    }
}
