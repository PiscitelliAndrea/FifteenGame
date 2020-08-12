﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quindici.Data
{
    public class Tile
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int number { get; set; }
        public string backgroundColor { get; set; }

        public string Number 
        {
            get
            {
                return number != 0 ? number.ToString() : "";
            }
            //set 
            //{
            //    number = Convert.ToInt32(value);
            //} 
        }
    }
}
