using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGame.Client.Classes
{
    public class FormButtonStatus
    {
        public enum ButtonStatus
        {
            Idle, Loading, Success, Error
        }

        public ButtonStatus Status { get; set; }
        public string Message { get; set; }
    }
}
