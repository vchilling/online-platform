using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class DirectionIEDirectionLine
    {
        public Direction DirectionData { get; set; }
        public IEnumerable<DirectionLine> DirectionLineData { get; set; }
    }
}