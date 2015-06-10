using DealsWhat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsWhat.ViewModels
{
    public class DealSpecificProductCommentViewModel
    {
        public string PosterName { get; set; }
        public string Message { get; set; }
        public DateTime DatePosted { get; set; }

        public DealSpecificProductCommentViewModel(DealComment model)
        {
            PosterName = string.Format("{0} {1}", model.Poster.FirstName, model.Poster.LastName);
            Message = model.Message;
            DatePosted = model.DatePosted;
        }
    }
}