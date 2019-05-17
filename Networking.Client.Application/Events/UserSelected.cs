﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Networking.Client.Application.Events
{
    /// <summary>
    /// An event that occurs when a user is selected.
    /// </summary>
    class UserSelected : PubSubEvent<int>
    {
    }
}
