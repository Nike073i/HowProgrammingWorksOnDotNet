using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CrudProject.Pages.Entry.Create;

public class CreateBindingModel
{
    [Required, StringLength(300)]
    public string Content { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    // [HiddenInput]
    public DateTime CreatedAt { get; set; }
}
