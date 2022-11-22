using ForumApiDataService.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumApiWebLibrary.Client.Pages
{
    public partial class Register : ComponentBase
    {
        public RegistrationModel SignUpUser { get; set; } = new RegistrationModel()
        {
            //Email = "johny.doe@email.com",
            //Password = "testpassword",
        };

        public bool ValidSubmit { get; set; } = false;

        async void HandleValidSubmit()
        {
            ValidSubmit = true;

            await Task.Delay(2000);

            ValidSubmit = false;

            StateHasChanged();
        }

        void HandleInvalidSubmit()
        {
            ValidSubmit = false;
        }

    }
}
