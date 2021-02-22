﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koshop.DomainClasses
{
    public class Options
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsAutoload { get; set; }

        public string Title { get; set; }

        public string Group { get; set; }
    }
}
