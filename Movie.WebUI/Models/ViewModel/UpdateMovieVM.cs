﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Movie.WebUI.Models.ViewModel
{
    public class UpdateMovieVM
    {
        public UpdateMovieDto MovieDto { get; set; }

        [BindNever]
        public List<SelectListItem>? GenreOptions { get; set; }
    }
}