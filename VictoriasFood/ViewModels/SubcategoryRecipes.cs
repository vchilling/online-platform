﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class SubcategoryRecipes
    {
        public Subcategory SubcategoryData { get; set; }
        public IEnumerable<Recipe> RecipeData { get; set; }            
    }
}