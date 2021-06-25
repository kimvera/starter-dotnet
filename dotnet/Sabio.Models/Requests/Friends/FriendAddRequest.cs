using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sabio.Models.Requests.Friends
{
    public class FriendAddRequest
    {
        public string Title { get; set; }
        public string Bio { get; set; }
        public string Summary { get; set; }
        public string Headline { get; set; }
        public string Slug { get; set; }
        [Required]
        public string StatusId { get; set; }
        [Required]
        public string PrimaryImage { get; set; }
        public int ImageTypeId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        public DateTime DateModified { get; set; }
    }
}
