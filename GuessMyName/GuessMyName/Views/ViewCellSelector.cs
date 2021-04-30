/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 3.1 Alpha */

using System;
using Xamarin.Forms;
using GuessMyName.Models;

namespace GuessMyName.Views
{
    class ViewCellSelector: DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        // Constructor
        public ViewCellSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as MessageModel;
            if (messageVm == null)
            {
                return null;
            }

            return (messageVm.Sender == MainPage.Me.UserName) ? outgoingDataTemplate : incomingDataTemplate;
        }
    }
}
